using System;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;


public class DiscoveryHelper : NetworkDiscovery
{
    bool hasFoundBroadcast = false;
    String serverIp = String.Empty;
    Random random = new Random();


    private void Awake()
    {
        initDiscoveryHelper();
    }
    
    
    
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        hasFoundBroadcast = true;
        serverIp = fromAddress.Replace("::ffff:", "");
        
        //base.OnReceivedBroadcast(fromAddress, data);
        
        Debug.Log("Recieved broadcast from: " + serverIp + " with data: " + data);
        
        StopBroadcast();
    }

    public void initDiscoveryHelper()
    {
        Initialize();
        StartAsClient();
        showGUI = false;
        broadcastInterval = 250;
        useNetworkManager = true;
    }



    public string ServerIp => serverIp;
    public bool HasFoundBroadcast => hasFoundBroadcast;
}