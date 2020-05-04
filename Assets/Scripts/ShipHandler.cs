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
    [SerializeField] private GameObject explosionPrefab;
    [SyncVar] private Color shipColor;

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

    // Called when ship trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer)
        {
            return;
        }
        
        if (other.tag == "Ship")
        {
            PlayExplosion();
            Destroy(gameObject);
        }
        else if (other.tag == "CurrentPad")
        {
            // TODO: Get pad color and check if its the same, if it is increase score else play explosion
            Color padColor = other.gameObject.GetComponent<MeshRenderer>().material.color;
            Debug.Log("Landed ship");
            Destroy(gameObject);
        }
    }

    // Spawns, plays and destroys the explosion at ships position
    private void PlayExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(explosion);
        Destroy(explosion, 2f);
    }

    // Setter for color
    public Color ShipColor
    {
        set { shipColor = value; }
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

    // Register components on start
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GetComponent<MeshRenderer>().material.color = shipColor;

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


