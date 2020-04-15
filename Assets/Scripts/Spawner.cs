using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime = 5f;
    [SerializeField] float maxSpawnTime = 20f;
    [SerializeField] float flightHeight = 5f;
    [SerializeField] float spawnOffset = 20f;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform world;
    [SerializeField] float speed = 5f;

    float worldLeftEdge;
    float worldRightEdge;
    float worldBottomEdge;
    float worldTopEdge;
    SpawnSide spawnSide;

    enum SpawnSide
    {
        Top,
        Right,
        Bottom,
        Left
    };

    void Start()
    {
        CalculateWorldEdges();

        // Initial spawn
        float randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", randomTime);
    }

    // Sets the world edges the the size of the world space, with lower left corner at origo
    private void CalculateWorldEdges()
    {
        worldLeftEdge = 0;
        worldRightEdge = world.localScale.x;
        worldBottomEdge = 0;
        worldTopEdge = world.localScale.z;
    }

    // Returns the initial velocity based on the objects start angle
    private Vector3 CalculateStartVelocity(GameObject entity)
    {
        float radians = entity.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float zSpeed = Mathf.Cos(radians) * speed;
        float xSpeed = Mathf.Sin(radians) * speed;
        return new Vector3(xSpeed, 0f, zSpeed);
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
    }

    // Returns a random location on the world edge
    private Vector3 GenerateStartLocation()
    {
        float randomValue = UnityEngine.Random.value;
        float xPos;
        float zPos;
        
        if (randomValue < 0.25f)
        {
            // Spawn on top
            spawnSide = SpawnSide.Top;
            xPos = UnityEngine.Random.Range(worldLeftEdge + spawnOffset, worldRightEdge - spawnOffset);
            zPos = worldTopEdge;
        }
        else if (randomValue < 0.5f)
        {
            // Spawn to the right
            spawnSide = SpawnSide.Right;
            xPos = worldRightEdge;
            zPos = UnityEngine.Random.Range(worldBottomEdge + spawnOffset, worldTopEdge - spawnOffset);
        }
        else if (randomValue < 0.75f)
        {
            // Spawn on bottom
            spawnSide = SpawnSide.Bottom;
            xPos = UnityEngine.Random.Range(worldLeftEdge + spawnOffset, worldRightEdge - spawnOffset);
            zPos = worldBottomEdge;
        }
        else
        {
            // Spawn to the left
            spawnSide = SpawnSide.Left;
            xPos = worldLeftEdge;
            zPos = UnityEngine.Random.Range(worldBottomEdge + spawnOffset, worldTopEdge - spawnOffset);
        }
        
        return new Vector3(xPos, flightHeight, zPos);
    }
    
    // Returns a random direction based on spawnside
    private Quaternion GenerateStartRotation(Vector3 startPosition)
    {
        float yRot;

        switch (spawnSide)
        {
            case SpawnSide.Top:
                yRot = UnityEngine.Random.Range(135f, 225f);
                break;
            case SpawnSide.Right:
                yRot = UnityEngine.Random.Range(225f, 315f);
                break;
            case SpawnSide.Bottom:
                yRot = UnityEngine.Random.Range(-45f, 45f);
                break;
            default:
                yRot = UnityEngine.Random.Range(45f, 135f);
                break;
        }

        return Quaternion.Euler(0f, yRot, 0f);
    }
}
