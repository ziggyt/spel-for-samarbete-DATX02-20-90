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

    [Server]
    // Gets random pad colors and registers them
    private void RegisterPadColors()
    {
        ColorCoordinator colorCoordinator = FindObjectOfType<ColorCoordinator>();
        currentPadColor = colorCoordinator.GetRandomColor();
        nextPadColor = colorCoordinator.GetRandomColor();
        colorCoordinator.RegisterPadColor(currentPadColor);
        colorCoordinator.RegisterPadColor(nextPadColor);
    }

    // Registers start colors if server and assigns pad start colors
    void Start()
    {
        RegisterPadColors();
        AssignPadColors();
    }
}
