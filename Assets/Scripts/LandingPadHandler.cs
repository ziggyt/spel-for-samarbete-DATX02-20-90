using UnityEngine;
using UnityEngine.Networking;

public class LandingPadHandler : NetworkBehaviour
{
    // Variables
    [SerializeField] private GameObject currentPad;
    [SerializeField] private Light currentPadLight;
    [SerializeField] private GameObject nextPad;
    [SyncVar] private Color currentPadColor;
    [SyncVar] private Color currentPadColorLight;
    [SyncVar] private Color nextPadColor;
    private ColorCoordinator colorCoordinator;

    // Sets colors variables and assigns colors to renderers
    private void AssignPadColors(Color currentColor, Color nextColor)
    {
        currentPadColor = currentColor;
        nextPadColor = nextColor;
        currentPad.GetComponent<MeshRenderer>().material.color = currentColor;
        currentPadLight.color = currentColor;
        nextPad.GetComponent<MeshRenderer>().material.color = nextColor;
    }

    // Register random color with coordinator and returns it
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

    // Deregister and assign new pad colors
    [Server]
    public void NewPadColors(Color oldColor)
    {
        DeregisterPadColor(currentPadColor);
        currentPadColor = nextPadColor;
        nextPadColor = RegisterPadColor();
        AssignPadColors(currentPadColor, nextPadColor);
        RpcAssignClientPadColors(currentPadColor, nextPadColor);
    }

    // Assigns colors to client renderers
    [ClientRpc]
    private void RpcAssignClientPadColors(Color currentColor, Color nextColor)
    {
        currentPad.GetComponent<MeshRenderer>().material.color = currentColor;
        nextPad.GetComponent<MeshRenderer>().material.color = nextColor;
    }

    // Registers start colors if server and assigns pad start colors
    void Start()
    {
        if (isServer)
        {
            colorCoordinator = FindObjectOfType<ColorCoordinator>();
            currentPadColor = RegisterPadColor();
            nextPadColor = RegisterPadColor();
        }
        AssignPadColors(currentPadColor, nextPadColor);
    }
}
