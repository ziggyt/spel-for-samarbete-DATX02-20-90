using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ShipHandler : MonoBehaviour
{
    float speed;
    private List<Vector3> path = new List<Vector3>();
    private Rigidbody rigidBody;
    private LineRenderer lineRenderer;
    private Camera camera;
    private bool clickedObject = false;
    private Vector3 currentDirection;
    private ParticleSystem explosion;
    private int fingerId = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
        explosion = transform.GetComponentInChildren<ParticleSystem>();
        ShipColor = ColorManager.GetRandomColor();
    }

    public Vector3 CurrentDirection
    {
        set { currentDirection = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship" || other.tag == "Wall")
        {
            explosion.Play();
            GetComponent<MeshRenderer>().enabled = false;
            lineRenderer.enabled = false;
            Invoke("DeathSequence", 2f);
        }
        else if (other.tag == "Finish")
        {
            // TODO: Finish sequence
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
        clickedObject = true;
        path.Clear();

        // Mark as kinematic to disable velocity
        rigidBody.isKinematic = true;
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
        if (path.Count != 0)
        {
            rigidBody.isKinematic = true; 
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
        rigidBody.isKinematic = false;
        rigidBody.velocity = currentDirection;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(currentDirection, transform.up), 0.15f);
    }

    private void MoveToPoint()
    {
        // Move one step towards current point
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, path[0], step);

        if (transform.position.Equals(path[0]))
        {
            // Delete point if reached
            path.RemoveAt(0);

            // Recalculate line renderer
            DrawLines();
        }
        else
        {
            // Save current direction
            currentDirection = (path[0] - transform.position).normalized * speed;

            // Face right direction
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(currentDirection, transform.up), 0.15f);
        }
    }

    private void DrawLines()
    {
        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());
    }

    public void AddPointToPath(Vector3 point)
    {
        path.Add(point);
        DrawLines();
    }

    private void RecordPath()
    {
        if (Input.GetMouseButton(0) && clickedObject && Input.touchCount == 0)
        {
            Vector3 point;
            // Add mouse coordinates as a point to path
            point = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.y - transform.position.y));

            path.Add(point);
            DrawLines();
        }
        else if (clickedObject)
        {
            // Mouse released so no more adding points
            clickedObject = false;
        }
    }

    public void SetFingerId(int id)
    {
        path.Clear();
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

    public Color ShipColor
    {
        get { return gameObject.GetComponent<MeshRenderer>().material.color; }
        set { gameObject.GetComponent<MeshRenderer>().material.color = value; }
    }

    public float Speed
    {
        set { speed = value; }
    }
}
