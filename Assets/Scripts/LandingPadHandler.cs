using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LandingPadHandler : NetworkBehaviour
{
    // Variables
    [SerializeField] private GameObject currentPad;
    [SerializeField] private GameObject nextPad;
    [SyncVar] private Color currentPadColor;
    [SyncVar] private Color nextPadColor;

    // Assigns the synced pad colors
    private void AssignPadColors()
    {
        currentPad.GetComponent<MeshRenderer>().material.color = currentPadColor;
        nextPad.GetComponent<MeshRenderer>().material.color = nextPadColor;
    }

    // Gets random pad colors and registers them
    private void RegisterPadColors()
    {
        currentPadColor = ColorCoordinator.GetRandomColor();
        nextPadColor = ColorCoordinator.GetRandomColor();
        ColorCoordinator.RegisterPadColor(currentPadColor);
        ColorCoordinator.RegisterPadColor(nextPadColor);
    }

    // Registers colors if server and assings pad start colors
    void Start()
    {
        if (isServer)
        {
            RegisterPadColors();
        }

        AssignPadColors();
    }
}
