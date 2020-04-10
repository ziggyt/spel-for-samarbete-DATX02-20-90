using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private static Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow
    };

    public static Color GetRandomColor()
    {
        int randomInt = Random.Range(0, colors.Length);
        return colors[randomInt];
    }
}
