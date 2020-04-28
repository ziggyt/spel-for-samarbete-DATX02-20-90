using UnityEngine;
using UnityEngine.Networking;

public class MouseManager : NetworkBehaviour
{
    private Camera cam;
    private GameObject ship;

    // Disable touch register on mouse click at start
    void Start()
    {
        Input.simulateMouseWithTouches = false;
        cam = GetComponent<Camera>();
    }

    // Check for mouse click on each frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (ship != null && Input.GetMouseButton(0))
        {
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 90f)); // TODO: Make z-position dynamic
            CmdAddPointToPath(ship.GetComponent<NetworkIdentity>(), point);
        }
        else
        {
            ship = null;
            InitClick();
        }
    }

    // Use raycast on mouse click to register ship clicked
    private void InitClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Send out ray to see if ship got clicked
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ship")
                {
                    ship = hit.collider.gameObject;
                    CmdClearPath(ship.GetComponent<NetworkIdentity>());
                }
            }
        }
    }

    // Add a point to the server version of the ship
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
