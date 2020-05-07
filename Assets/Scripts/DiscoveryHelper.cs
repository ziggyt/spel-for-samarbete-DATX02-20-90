using System;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;


public class DiscoveryHelper : NetworkDiscovery
{
    bool hasFoundBroadcast = false;
    String serverIp = String.Empty;


    private void Awake()
    {
        Initialize();
        StartAsClient();
       // broadcastPort = 
        showGUI = false;
        broadcastInterval = 250;
        //useNetworkManager = true; //todo om något går fel kolla denna
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        hasFoundBroadcast = true;
        serverIp = fromAddress;
        
        base.OnReceivedBroadcast(fromAddress, data);
        
        Debug.Log("Recieved broadcast from: " + fromAddress + " with data: " + data);
    }
    
    

    public string ServerIp => serverIp;
    public bool HasFoundBroadcast => hasFoundBroadcast;
}