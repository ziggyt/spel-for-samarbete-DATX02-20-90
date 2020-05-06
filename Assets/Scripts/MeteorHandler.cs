using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeteorHandler : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
