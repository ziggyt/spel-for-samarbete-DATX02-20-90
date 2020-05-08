using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class MainMenuScript : MonoBehaviour
{
    private DiscoveryHelper _discoveryHelper;
    private bool _hasConnected = false;
    public NetworkManager _networkManager;


    private void Awake()
    {
        _discoveryHelper = gameObject.AddComponent<DiscoveryHelper>();
    }

    private void Update()
    {
        if (_discoveryHelper.HasFoundBroadcast && !_hasConnected)
        {
            Debug.Log("Found host!");
            String IP = _discoveryHelper.ServerIp;
            _networkManager.SetMatchHost(IP, 7777, false);
            _networkManager.StartClient();
            Debug.Log("Connected to " + IP);
            _hasConnected = true;
        }
    }

    public void PlayGame()
    {
        if (!_discoveryHelper.HasFoundBroadcast)
        {
            _networkManager.StartHost();
            Debug.Log("Started host server");
            _discoveryHelper.StopBroadcast();
            _discoveryHelper.StartAsServer(); //Starthost vs startasserver?
            Debug.Log("Started broadcast on " + _networkManager.networkAddress + ":" + _networkManager.networkPort);
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