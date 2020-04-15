using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorHandler : MonoBehaviour
{
    // Calls the DeathSequence after 5 seconds when colliding with outer wall (so tail will not dissapear weirdly)
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Invoke("DeathSequence", 5f);
        }
    }
    
    // Destroys the meteor game object
    private void DeathSequence()
    {
        Destroy(gameObject);
    }
}
