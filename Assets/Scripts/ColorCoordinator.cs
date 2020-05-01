using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorCoordinator : NetworkBehaviour
{
    // Variables
    private static Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };
    private static List<Color> registeredPadColors = new List<Color>();

    // Removes the first occurence of the registered color
    public static bool DeregisterPadColor(Color color)
    {
        return registeredPadColors.Remove(color);
    }

    // Returns a random color from the list
    public static Color GetRandomColor()
    {
        int randomInt = Random.Range(0, colors.Length);
        return colors[randomInt];
    }

    // Return a random registered pad color
    public static Color GetRandomPadColor()
    {
        int randomInt = Random.Range(0, registeredPadColors.Count);
        return registeredPadColors[randomInt];
    }

    // Registers a color to the list
    public static void RegisterPadColor(Color color)
    {
        if (!registeredPadColors.Contains(color))
        {
            registeredPadColors.Add(color);
        }
    }
}
