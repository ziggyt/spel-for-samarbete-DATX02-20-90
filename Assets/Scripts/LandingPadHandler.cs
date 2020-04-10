using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPadHandler : MonoBehaviour
{
    private Color currentColor;

    private void Start()
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
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
            Debug.Log("Wrong colored ship landed");
        }
    }

    private void ChangeColor()
    {
        currentColor = ColorManager.GetRandomColor();
        gameObject.GetComponent<MeshRenderer>().material.color = currentColor;
    }
}
