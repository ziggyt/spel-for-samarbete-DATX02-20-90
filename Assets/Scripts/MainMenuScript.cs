using System;
using TMPro;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{
    private DiscoveryHelper _discoveryHelper;
    private bool _hasConnected = false;
    public NetworkManager _networkManager;
    //public GameObject connectionInfo;


    private void Awake()
    {
        _discoveryHelper = gameObject.AddComponent<DiscoveryHelper>();
    }

    private void Update()
    {
        //hasFoundHostInfo.GetComponent<Text>().text = "HasFoundBroadcast: " + _discoveryHelper.HasFoundBroadcast + "\nHasConnected: " + _hasConnected;

        if (!_discoveryHelper.HasFoundBroadcast && !_discoveryHelper.running)
        {
            _discoveryHelper.initDiscoveryHelper();
        }

        //Debug.Log("Discovery helper: " + _discoveryHelper.running);

        if (_discoveryHelper.HasFoundBroadcast && !_hasConnected)
        {
            _discoveryHelper.StopAllCoroutines();
            _discoveryHelper.StopBroadcast();

            //Debug.Log("Found host!");
            String IP = _discoveryHelper.ServerIp;

            //connectionInfo.GetComponent<Text>().text = "Found host: " + IP;
            _networkManager.networkAddress = IP;
            _networkManager.networkPort = 7777;
            _networkManager.StartClient();

            //Debug.Log("Connected to " + IP);
            _hasConnected = true;
        }
    }

    public void PlayGame()
    {
        if (!_discoveryHelper.HasFoundBroadcast)
        {
            //connectionInfo.GetComponent<Text>().text = "Started host";
            //NetworkServer.Reset();
            _discoveryHelper.StopBroadcast();

            _networkManager.StartHost();
            //Debug.Log("Started host server");
            _discoveryHelper.StartAsServer();


            //Debug.Log("Started broadcast on " + _networkManager.networkAddress + ":" + _discoveryHelper.broadcastPort);
        }

        GameObject[] uiComponents = GameObject.FindGameObjectsWithTag("MenuUIComponent");

        foreach (GameObject uiComponent in uiComponents)
        {
            uiComponent.SetActive(false);
        }

        //Debug.Log("Closed main menu");
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}
