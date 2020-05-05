using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LandingPadHandler : NetworkBehaviour
{
    // Variables
    [SerializeField] private GameObject currentPad;
    [SerializeField] private GameObject nextPad;
    private Color currentPadColor;
    private Color nextPadColor;
    private ColorCoordinator colorCoordinator;

    [Server]
    private void AssignPadColors(Color currentColor, Color nextColor)
    {
        currentPadColor = currentColor;
        nextPadColor = nextColor;
        currentPad.GetComponent<MeshRenderer>().material.color = currentColor;
        nextPad.GetComponent<MeshRenderer>().material.color = nextColor;
    }

    [Server]
    private Color RegisterPadColor()
    {
        Color padColor = colorCoordinator.GetRandomColor();
        colorCoordinator.RegisterPadColor(padColor);
        return padColor;
    }

    // Deregister color from coordinator
    [Server]
    private void DeregisterPadColor(Color color)
    {
        colorCoordinator.DeregisterPadColor(color);
    }

    [Server]
    // Deregister and assign new pad colors
    public void NewPadColors(Color oldColor)
    {
        DeregisterPadColor(oldColor);
        currentPadColor = RegisterPadColor();
        nextPadColor = RegisterPadColor();
        AssignPadColors(currentPadColor, nextPadColor);
        RpcAssignClientPadColors(currentPadColor, nextPadColor);
    }

    [ClientRpc]
    private void RpcAssignClientPadColors(Color currentColor, Color nextColor)
    {
        currentPad.GetComponent<MeshRenderer>().material.color = currentColor;
        nextPad.GetComponent<MeshRenderer>().material.color = nextColor;
        CmdServerMsg("ClientRpc call");
    }

    [Command]
    private void CmdServerMsg(string msg)
    {
        Debug.Log(msg);
    }

    // Registers start colors if server and assigns pad start colors
    void Start()
    {
        if (!isServer) return;
        colorCoordinator = FindObjectOfType<ColorCoordinator>();
        currentPadColor = RegisterPadColor();
        nextPadColor = RegisterPadColor();
        AssignPadColors(currentPadColor, nextPadColor);
        RpcAssignClientPadColors(currentPadColor, nextPadColor);
    }
}
