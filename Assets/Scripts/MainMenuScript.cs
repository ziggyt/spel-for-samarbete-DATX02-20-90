using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    private ArrayList _networkComponents = new ArrayList();

    public void PlayGame()
    {
        _networkComponents.AddRange(GameObject.FindGameObjectsWithTag("NetworkComponent"));
        
        Debug.Log(_networkComponents.Count);

        foreach (GameObject networkComponent in _networkComponents)
        {
            networkComponent.SetActive(true);
        }
        
        
        Debug.Log("Enabled ship spawner!");
        GameObject.Find("MainMenuCanvas").SetActive(false);
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}
