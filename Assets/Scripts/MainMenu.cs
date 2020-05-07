using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public DiscoveryHelper _discoveryHelper;
    public NetworkManager _networkManager;
    

    private void Awake()
    {
    }


    private void Update()
    {
        if (_discoveryHelper.HasFoundBroadcast)
        {
            String IP = _discoveryHelper.ServerIp;
            _networkManager.SetMatchHost(IP, 7777, false);
        }

    }


    public void PlayGame()
    {

        if (!_discoveryHelper.HasFoundBroadcast)
        {
            _networkManager.StartHost();
            _discoveryHelper.StartAsServer();

        }

        GameObject.Find("MainMenuCanvas").SetActive(false);
        Debug.Log("Closed main menu");
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}


//TODO en host en client debug?
//kanske börja kolla detta i en splash screen som visas innan
// if broadcast recieved join 
// else startserver 
// problem om alla har igång startskärmen 

// kanske flytta networkdiscover till en annan ny klass för att kunna extenda monobehaviour igen
// i update, kolla hela tiden om den har fått en broadcast
// isf joina
// annars vid PlayGame startserver


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