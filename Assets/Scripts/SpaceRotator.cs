using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRotator : MonoBehaviour
{
    [SerializeField] private float rotationsPerMinute = 10.0f;
    void Update()
    {

        transform.Rotate(0, (float) 6.0 * rotationsPerMinute * Time.deltaTime, 0);
    }
}
