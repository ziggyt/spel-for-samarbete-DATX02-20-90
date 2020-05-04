using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    // Variables
    private float flightHeight = 10f;
    private float planeBottomEdge;
    private float planeLeftEdge;
    private float planeRightEdge;
    private float planeTopEdge;
    private SpawnSide spawnSide;
    [SerializeField] private float maxSpawnTime = 20f;
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float spawnOffset = 20f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform plane;
    [SerializeField] private bool isActive = false;

    private enum SpawnSide
    {
        Top,
        Right,
        Bottom,
        Left
    };

    // Returns the initial velocity based on the objects start angle
    private Vector3 CalculateStartVelocity(GameObject entity)
    {
        float radians = entity.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float zSpeed = Mathf.Cos(radians) * speed;
        float xSpeed = Mathf.Sin(radians) * speed;
        return new Vector3(xSpeed, 0f, zSpeed);
    }

    // Returns a random location on the plane edge
    private Vector3 GenerateStartLocation()
    {
        float randomValue = Random.value;
        float xPos;
        float zPos;
        
        if (randomValue < 0.25f)
        {
            // Spawn on top
            spawnSide = SpawnSide.Top;
            xPos = Random.Range(planeLeftEdge + spawnOffset, planeRightEdge - spawnOffset);
            zPos = planeTopEdge;
        }
        else if (randomValue < 0.5f)
        {
            // Spawn to the right
            spawnSide = SpawnSide.Right;
            xPos = planeRightEdge;
            zPos = Random.Range(planeBottomEdge + spawnOffset, planeTopEdge - spawnOffset);
        }
        else if (randomValue < 0.75f)
        {
            // Spawn on bottom
            spawnSide = SpawnSide.Bottom;
            xPos = Random.Range(planeLeftEdge + spawnOffset, planeRightEdge - spawnOffset);
            zPos = planeBottomEdge;
        }
        else
        {
            // Spawn to the left
            spawnSide = SpawnSide.Left;
            xPos = planeLeftEdge;
            zPos = Random.Range(planeBottomEdge + spawnOffset, planeTopEdge - spawnOffset);
        }
        
        return new Vector3(xPos, flightHeight, zPos);
    }

    // Sets the edges to the size of the plane space, with lower left corner at origo
    private void CalculateEdges()
    {
        planeLeftEdge = 0;
        planeRightEdge = plane.localScale.x * 10;
        planeBottomEdge = 0;
        planeTopEdge = plane.localScale.z * 10;
    }

    // Spawns an object then waits a randomized time before doing it again
    private void RandomSpawn()
    {
        SpawnEntity();
        
        float randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", randomTime);
    }

    // Spawns an object in the world at random edge position and random angle
    private void SpawnEntity()
    {
        Vector3 startPosition = GenerateStartLocation();
        Quaternion startRotation = GenerateStartRotation(startPosition);
        
        // TODO: Warn player that a entity is approaching
        
        GameObject entity = Instantiate(prefab, startPosition, startRotation, transform);

        if (entity.tag == "Ship")
        {
            ShipHandler shipHandler = entity.GetComponent<ShipHandler>();
            shipHandler.CurrentDirection = CalculateStartVelocity(entity);
            shipHandler.Speed = speed;
        }
        else
        {
            entity.GetComponent<Rigidbody>().velocity = CalculateStartVelocity(entity);
        }

        NetworkServer.Spawn(entity);
    }
    
    // Returns a random direction based on spawnside
    private Quaternion GenerateStartRotation(Vector3 startPosition)
    {
        float yRot;

        switch (spawnSide)
        {
            case SpawnSide.Top:
                yRot = Random.Range(135f, 225f);
                break;
            case SpawnSide.Right:
                yRot = Random.Range(225f, 315f);
                break;
            case SpawnSide.Bottom:
                yRot = Random.Range(-45f, 45f);
                break;
            default:
                yRot = Random.Range(45f, 135f);
                break;
        }

        return Quaternion.Euler(0f, yRot, 0f);
    }

    // Called on server start
    public override void OnStartServer()
    {
        CalculateEdges();

        // Initial spawn
        float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", randomTime);
    }
}
