using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class MainMenuScript : NetworkDiscovery
{
    public NetworkDiscovery _networkDiscovery;
    public NetworkManager _networkManager;
    
    private string IP;
    private void Awake()
    {
        _networkDiscovery.Initialize();
        _networkDiscovery.StartAsClient(); 
        //TODO en host en client debug?
        //kanske börja kolla detta i en splash screen som visas innan
        // if broadcast recieved join 
        // else startserver 
        // problem om alla har igång startskärmen 
        
        // kanske flytta networkdiscover till en annan ny klass för att kunna extenda monobehaviour igen
        // i update, kolla hela tiden om den har fått en broadcast
        // isf joina
        
        Debug.Log("Discovery started");
    }

    private void FixedUpdate()
    {
        
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

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //base.OnReceivedBroadcast(fromAddress, data);
        Debug.Log(fromAddress);
        Debug.Log(data);
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