using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TouchManager : NetworkBehaviour
{
    // Variables
    private Dictionary<int, GameObject> fingerIdToShip = new Dictionary<int, GameObject>();
    private Camera cam;

    // Set up camera at start
    void Start()
    {
        cam = GetComponent<Camera>();

        if (isLocalPlayer) return;
        cam.enabled = false;
    }

    // Handles all touches on screen each frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch currentTouch = Input.touches[i];

                switch (currentTouch.phase)
                {
                    case TouchPhase.Began:
                        InitTouch(currentTouch);
                        break;
                    case TouchPhase.Moved:
                        RegisterTouchToShip(currentTouch);
                        break;
                    case TouchPhase.Ended:
                        DeregisterShip(currentTouch);
                        break;
                }
            }
        }
    }

    // Set finger id to -1 on ship and remove from dict
    private void DeregisterShip(Touch currentTouch)
    {
        int currentFingerId = currentTouch.fingerId;

        if (fingerIdToShip.ContainsKey(currentFingerId))
        {
            GameObject shipObject = fingerIdToShip[currentFingerId];

            // Handle if ship is destroyed while deregistering
            if (shipObject != null)
            {
                shipObject.GetComponent<ShipHandler>().FingerId = -1;
            }

            fingerIdToShip.Remove(currentFingerId);
        }
    }

    // Set up ship and finger id if touch on ship
    private void InitTouch(Touch currentTouch)
    {
        Ray ray = cam.ScreenPointToRay(currentTouch.position);
        RaycastHit hit;

        // Send out ray and see if ship got touched
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Ship")
            {
                // Save ship to dict
                GameObject shipObject = hit.transform.gameObject;
                fingerIdToShip.Add(currentTouch.fingerId, shipObject);

                // Set finger id to ship
                ShipHandler shipHandler = shipObject.GetComponent<ShipHandler>();
                shipHandler.FingerId = currentTouch.fingerId;
                CmdClearPath(shipObject.GetComponent<NetworkIdentity>());
            }
        }
    }

    // Add point to path if ship is clicked
    private void RegisterTouchToShip(Touch currentTouch)
    {
        int currentFingerId = currentTouch.fingerId;
        
        if (fingerIdToShip.ContainsKey(currentFingerId))
        {
            GameObject shipObject = fingerIdToShip[currentFingerId];

            // Handle if ship is destroyed while drawing path
            if (shipObject != null)
            {
                ShipHandler shipHandler = shipObject.GetComponent<ShipHandler>();
                if (currentFingerId == shipHandler.FingerId)
                {
                    Vector3 point = cam.ScreenToWorldPoint(new Vector3(currentTouch.position.x, currentTouch.position.y, cam.transform.position.y - shipObject.transform.position.y));
                    CmdAddPointToPath(shipObject.GetComponent<NetworkIdentity>(), point);
                }
            }
        }
    }

    // Add point to path on the server version of the ship
    [Command]
    private void CmdAddPointToPath(NetworkIdentity netId, Vector3 point)
    {
        NetworkServer.objects[netId.netId].gameObject.GetComponent<ShipHandler>().AddPointToPath(point);
    }

    // Clear the path list on the server version of the ship
    [Command]
    private void CmdClearPath(NetworkIdentity netId)
    {
        NetworkServer.objects[netId.netId].gameObject.GetComponent<ShipHandler>().ClearPath();
    }
}
