using UnityEngine;
using UnityEngine.Networking;

public class ShipHandler : NetworkBehaviour
{
    // Variables
    private int fingerId;
    private SyncListVector path = new SyncListVector();
    private LineRenderer lineRenderer;
    private Rigidbody rigidBody;
    private Vector3 currentDirection;
    [SerializeField] private float speed = 2f;

    // Register components on start
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (!isServer)
        {
            return;
        }

        rigidBody = GetComponent<Rigidbody>();
    }
    
    // Update movement
    void Update()
    {
        DrawLines();

        if (!isServer)
        {
            return;
        }

        HandleMovement();
    }

    // Renders the current path with lines
    private void DrawLines()
    {
        lineRenderer.positionCount = path.Count;
        Vector3[] pathArr = new Vector3[path.Count];
        int i = 0;

        foreach (Vector3 vec in path)
        {
            pathArr[i] = vec;
            i++;
        }

        lineRenderer.SetPositions(pathArr);
    }

    // Handle movement based on path exist
    private void HandleMovement()
    {
        if (path.Count == 0)
        {
            MoveFreely();
        }
        else
        {
            MoveToPoint();
        }
    }

    // Make object go in a straight line after last point
    private void MoveFreely()
    {
        rigidBody.isKinematic = false;
        rigidBody.velocity = currentDirection;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentDirection, transform.up), 0.15f);
    }

    // Move to the next point in path
    private void MoveToPoint()
    {
        rigidBody.isKinematic = true;

        // Move one step towards current point
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, path[0], step);

        if (transform.position.Equals(path[0]))
        {
            // Delete point if reached
            path.RemoveAt(0);
        }
        else
        {
            // Save ccurrent direction
            currentDirection = (path[0] - transform.position).normalized * speed;

            // Face right direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentDirection, transform.up), 0.15f);
        }
    }

    // Setter for speed
    public float Speed
    {
        set { speed = value; }
    }

    // Getter and setter for fingerId
    public int FingerId
    {
        get { return fingerId; }
        set { fingerId = value; }
    }
   
    // Setter for currentDirection
    public Vector3 CurrentDirection
    {
        set { currentDirection = value; }
    }

    // Adds a point to the path and redraws the path
    public void AddPointToPath(Vector3 point)
    {
        path.Add(point);
    }
    
    // Clear the path
    public void ClearPath()
    {
        path.Clear();
    }
}

// Class for a Vector3 List that syncs to clients
public class SyncListVector : SyncList<Vector3> {
     protected override void SerializeItem(NetworkWriter writer, Vector3 item)
     {
         writer.Write(item);
     }

     protected override Vector3 DeserializeItem(NetworkReader reader)
     {
         return reader.ReadVector3();
     }
 }


