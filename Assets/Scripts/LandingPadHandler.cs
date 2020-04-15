using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPadHandler : MonoBehaviour
{
    private Color currentColor;
    private Color nextColor;

    private void Start()
    {
        nextColor = ColorManager.GetRandomColor();
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ship")
        {
            ShipHandler shipHandler = other.gameObject.GetComponent<ShipHandler>();
            
            if (shipHandler.ShipColor == currentColor)
            {
                // TODO: Add points or something
                ChangeColor();
            }
            else
            {
                // TODO: Detract points or something, crash ships?
            }
        }
    }

    private void ChangeColor()
    {
        currentColor = nextColor;
        nextColor = ColorManager.GetRandomColor();
        AssignPadColors();
    }

    private void AssignPadColors()
    {
        var padMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer padMesh in padMeshes)
        {
            if (padMesh.tag == "CurrentLandingPad")
            {
                padMesh.material.color = currentColor;
            }
            else if (padMesh.tag == "NextLandingPad")
            {
                padMesh.material.color = nextColor;
            }
        }
    }
}
