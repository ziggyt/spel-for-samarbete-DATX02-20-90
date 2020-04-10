using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class InputHandler : MonoBehaviour
{

    [SerializeField] private float speed = 0.5f;
    private List<Vector3> _path;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private bool _clickedObject;
    private LineRenderer _lineRenderer;
    private Vector3 _currentDirection;
    private ParticleSystem explosion;
    private int fingerId = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        _path = new List<Vector3>();
        _rigidBody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
        _clickedObject = false;
        _currentDirection = CalculateStartVelocity();
        explosion = transform.GetComponentInChildren<ParticleSystem>();
    }

    private Vector3 CalculateStartVelocity()
    {
        float radians = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float zSpeed = Mathf.Cos(radians) * speed;
        float xSpeed = Mathf.Sin(radians) * speed;
        return new Vector3(xSpeed, 0f, zSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            explosion.Play();
            GetComponent<MeshRenderer>().enabled = false;
            _lineRenderer.enabled = false;
            Invoke("DeathSequence", 2f);
        }
        else if (other.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
    
    private void DeathSequence()
    {
           Destroy(gameObject);
    }

    // Called when the object is pressed
    private void OnMouseDown()
    {
        // Mark the object clicked and clear array for new path
        _clickedObject = true;
        _path.Clear();

        // Mark as kinematic to disable velocity
        _rigidBody.isKinematic = true;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (fingerId == -1)
        {
            RecordPath();
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_path.Count != 0)
        {
            _rigidBody.isKinematic = true; 
            MoveToPoint();
        }
        else
        {
            MoveFreely();
        }
    }

    private void MoveFreely()
    {
        // Make object go in a straight line after last point
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = _currentDirection;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_currentDirection, transform.up), 0.15f);
    }

    private void MoveToPoint()
    {
        // Move one step towards current point
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _path[0], step);

        if (transform.position.Equals(_path[0]))
        {
            // Delete point if reached
            _path.RemoveAt(0);

            // Recalculate line renderer
            DrawLines();
        }
        else
        {
            // Save current direction
            _currentDirection = (_path[0] - transform.position).normalized * speed;

            // Face right direction
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_currentDirection, transform.up), 0.15f);
        }
    }

    private void DrawLines()
    {
        _lineRenderer.positionCount = _path.Count;
        _lineRenderer.SetPositions(_path.ToArray());
    }

    public void AddPointToPath(Vector3 point)
    {
        _path.Add(point);
        DrawLines();
    }

    private void RecordPath()
    {
        if (Input.GetMouseButton(0) && _clickedObject && Input.touchCount == 0)
        {
            Vector3 point;
            // Add mouse coordinates as a point to path
            point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y - transform.position.y));

            _path.Add(point);
            DrawLines();
        }
        else if (_clickedObject)
        {
            // Mouse released so no more adding points
            _clickedObject = false;
        }
    }

    public void SetFingerId(int id)
    {
        _path.Clear();
        fingerId = id;
    }

    public int GetFingerId()
    {
        return fingerId;
    }

    public void ResetFingerId()
    {
        fingerId = -1;
    }
}
