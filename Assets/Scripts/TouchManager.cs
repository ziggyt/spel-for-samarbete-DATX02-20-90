using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    Dictionary<int, GameObject> fingerIdToShip = new Dictionary<int, GameObject>();

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch currentTouch = Input.touches[i];

                switch (currentTouch.phase)
                {
                    case TouchPhase.Began:
                        LinkTouchToShip(currentTouch);
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

    private void DeregisterShip(Touch currentTouch)
    {
        int currentFingerId = currentTouch.fingerId;

        if (fingerIdToShip.ContainsKey(currentFingerId))
        {
            GameObject ship = fingerIdToShip[currentFingerId];

            // Handle if ship is destroyed while deregistering
            if (ship != null)
            {
                ship.GetComponent<InputHandler>().ResetFingerId();
            }
            
            fingerIdToShip.Remove(currentFingerId);
        }
    }

    private void LinkTouchToShip(Touch currentTouch)
    {
        Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Ship")
            {
                // Save ship to dict
                GameObject ship = hit.transform.gameObject;
                fingerIdToShip.Add(currentTouch.fingerId, ship);

                // Set finger id to ship
                InputHandler inputHandler = ship.GetComponent<InputHandler>();
                inputHandler.SetFingerId(currentTouch.fingerId);
            }
        }
    }
    private void RegisterTouchToShip(Touch currentTouch)
    {
        int currentFingerId = currentTouch.fingerId;

        if (fingerIdToShip.ContainsKey(currentFingerId)) 
        {
            GameObject ship = fingerIdToShip[currentFingerId];

            // Handle if ship is destroyed while drawing path
            if (ship != null)
            {
                InputHandler inputHandler = ship.GetComponent<InputHandler>();
                int shipFingerId = inputHandler.GetFingerId();
                if (currentFingerId == shipFingerId)
                {
                    Camera camera = Camera.main;
                    Vector3 point = camera.ScreenToWorldPoint(new Vector3(currentTouch.position.x, currentTouch.position.y, camera.transform.position.y - ship.transform.position.y));
                    inputHandler.AddPointToPath(point);
                }
            }
            else
            {
                DeregisterShip(currentTouch);
            }
        }
    }
}
