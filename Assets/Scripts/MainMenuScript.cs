using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public NetworkDiscovery _networkDiscovery;
    public NetworkManager _networkManager;
    
    private string IP;


    private void Awake()
    {
        _networkDiscovery.Initialize();
        _networkDiscovery.StartAsClient();
        
        Debug.Log("Discovery started");
    }

    public void PlayGame()
    {
        
        IP = _networkManager.networkAddress;
        Debug.Log(IP);
        
        _networkManager.StartHost();

        GameObject.Find("MainMenuCanvas").SetActive(false);
        Debug.Log("Closed main menu");
    }
    
    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }

    void OnBroadCastRecieved()
    {
        
        
    }
}


//private ArrayList _networkComponents = new ArrayList();
    
/*
  _networkComponents.AddRange(GameObject.FindGameObjectsWithTag("NetworkComponent"));
  
  Debug.Log(_networkComponents.Count);

  foreach (GameObject networkComponent in _networkComponents)
  {
      networkComponent.SetActive(true);
  }
  
  //        GameObject.Find("Spawners").SetActive(true);
//        Debug.Log("Enabled ship spawner!");
  */