using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 20f;
    [SerializeField] private float flightHeight = 5f;
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
        
        float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("RandomSpawn", randomTime);
    }

    private void SpawnShip()
    {
        Vector3 startPosition = GenerateStartLocation();
        Quaternion startRotation = GenerateStartRotation(startPosition);
        
        Instantiate(prefab, startPosition, startRotation, transform);
    }

    private Vector3 GenerateStartLocation()
    {
        float randomValue = Random.value;
        float xPos;
        float zPos;
        
        if (randomValue < 0.25f)
        {
            // Spawn on top
            _spawnSide = SpawnSide.Top;
            xPos = Random.Range(_worldLeftEdge, _worldRightEdge);
            zPos = _worldTopEdge;
        }
        else if (randomValue < 0.5f)
        {
            // Spawn to the right
            _spawnSide = SpawnSide.Right;
            xPos = _worldRightEdge;
            zPos = Random.Range(_worldBottomEdge, _worldTopEdge);
        }
        else if (randomValue < 0.75f)
        {
            // Spawn on bottom
            _spawnSide = SpawnSide.Bottom;
            xPos = Random.Range(_worldLeftEdge, _worldRightEdge);
            zPos = _worldBottomEdge;
        }
        else
        {
            // Spawn to the left
            _spawnSide = SpawnSide.Left;
            xPos = _worldLeftEdge;
            zPos = Random.Range(_worldBottomEdge, _worldTopEdge);
        }
        
        return new Vector3(xPos, flightHeight, zPos);
    }
    
    private Quaternion GenerateStartRotation(Vector3 startPosition)
    {
        float yRot;

        switch (_spawnSide)
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
}
