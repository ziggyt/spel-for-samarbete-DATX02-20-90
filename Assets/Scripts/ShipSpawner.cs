using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
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
        RandomSpawn();
    }

    private void CalculateWorldEdges()
    {
        worldLeftEdge = 0;
        worldRightEdge = world.localScale.x;
        worldBottomEdge = 0;
        worldTopEdge = world.localScale.z;
    }

    private Vector3 CalculateStartVelocity(GameObject ship)
    {
        float radians = ship.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float zSpeed = Mathf.Cos(radians) * speed;
        float xSpeed = Mathf.Sin(radians) * speed;
        return new Vector3(xSpeed, 0f, zSpeed);
    }

    private void RandomSpawn()
    {
        SpawnShip();
        
        float randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", randomTime);
    }

    private void SpawnShip()
    {
        Vector3 startPosition = GenerateStartLocation();
        Quaternion startRotation = GenerateStartRotation(startPosition);
        
        // TODO: Warn player that a ship is approaching
        
        GameObject ship = Instantiate(prefab, startPosition, startRotation, transform);

        if (ship.tag == "Ship")
        {
            ShipHandler shipHandler = ship.GetComponent<ShipHandler>();
            shipHandler.CurrentDirection = CalculateStartVelocity(ship);
            shipHandler.Speed = speed;
        }
        else
        {
            ship.GetComponent<Rigidbody>().velocity = CalculateStartVelocity(ship);
        }
    }

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
