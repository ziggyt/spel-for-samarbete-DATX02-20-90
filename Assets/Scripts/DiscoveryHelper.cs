using System;
using UnityEngine;
using UnityEngine.Networking;


public class DiscoveryHelper : NetworkDiscovery
{
    bool hasFoundBroadcast = false;
    String serverIp = String.Empty;
    

    private void Awake()
    {
        Initialize();
        StartAsClient();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        hasFoundBroadcast = true;
        serverIp = fromAddress;
        
        base.OnReceivedBroadcast(fromAddress, data);
    }
    
    

    public string ServerIp => serverIp;
    public bool HasFoundBroadcast => hasFoundBroadcast;
}