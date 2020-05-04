using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Loaded Scene!");
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}
