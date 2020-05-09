using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorCoordinator : NetworkBehaviour
{
    // Variables
    // private Color lightBlue = new Color(0.0f,0.0f,1.0f,1.0f);
    private Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow
    };
    private List<Color> registeredPadColors = new List<Color>();

    // Removes the first occurence of the registered color
    public bool DeregisterPadColor(Color color)
    {
        return registeredPadColors.Remove(color);
    }

    // Returns a random color from the list
    public Color GetRandomColor()
    {
        int randomInt = Random.Range(0, colors.Length);
        return colors[randomInt];
    }

    // Return a random registered pad color
    public Color GetRandomPadColor()
    {
        int randomInt = Random.Range(0, registeredPadColors.Count);
        return registeredPadColors[randomInt];
    }
    
    // Registers a color to the list
    public void RegisterPadColor(Color color)
    {
        registeredPadColors.Add(color);
    }
}
