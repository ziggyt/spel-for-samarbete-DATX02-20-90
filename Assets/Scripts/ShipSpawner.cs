using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 20f;
    [SerializeField] private float flightHeight = 5f;
    [SerializeField] float spawnOffset = 20f;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform world;

    private float _worldLeftEdge;
    private float _worldRightEdge;
    private float _worldBottomEdge;
    private float _worldTopEdge;
    private SpawnSide _spawnSide;

    enum SpawnSide
    {
        Top,
        Right,
        Bottom,
        Left
    };

    // Start is called before the first frame update
    void Start()
    {
        CalculateWorldEdges();
        RandomSpawn();
    }

    private void CalculateWorldEdges()
    {
        _worldLeftEdge = 0;
        _worldRightEdge = world.localScale.x;
        _worldBottomEdge = 0;
        _worldTopEdge = world.localScale.z;
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
        AddRandomColor(ship);
    }

    private void AddRandomColor(GameObject ship)
    {
        ShipHandler shipHandler = ship.GetComponent<ShipHandler>();
        shipHandler.ShipColor = ColorManager.GetRandomColor();
    }

    private Vector3 GenerateStartLocation()
    {
        float randomValue = UnityEngine.Random.value;
        float xPos;
        float zPos;
        
        if (randomValue < 0.25f)
        {
            // Spawn on top
            _spawnSide = SpawnSide.Top;
            xPos = UnityEngine.Random.Range(_worldLeftEdge + spawnOffset, _worldRightEdge - spawnOffset);
            zPos = _worldTopEdge;
        }
        else if (randomValue < 0.5f)
        {
            // Spawn to the right
            _spawnSide = SpawnSide.Right;
            xPos = _worldRightEdge;
            zPos = UnityEngine.Random.Range(_worldBottomEdge + spawnOffset, _worldTopEdge - spawnOffset);
        }
        else if (randomValue < 0.75f)
        {
            // Spawn on bottom
            _spawnSide = SpawnSide.Bottom;
            xPos = UnityEngine.Random.Range(_worldLeftEdge + spawnOffset, _worldRightEdge - spawnOffset);
            zPos = _worldBottomEdge;
        }
        else
        {
            // Spawn to the left
            _spawnSide = SpawnSide.Left;
            xPos = _worldLeftEdge;
            zPos = UnityEngine.Random.Range(_worldBottomEdge + spawnOffset, _worldTopEdge - spawnOffset);
        }
        
        return new Vector3(xPos, flightHeight, zPos);
    }
    
    private Quaternion GenerateStartRotation(Vector3 startPosition)
    {
        float yRot;

        switch (_spawnSide)
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
