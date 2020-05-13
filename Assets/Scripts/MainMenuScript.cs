using System;
using TMPro;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{
    private DiscoveryHelper _discoveryHelper;
    private bool _hasConnected = false;
    public NetworkManager _networkManager;
    public Image buttonBackground;
    public TextMeshProUGUI buttonText;

    [FormerlySerializedAs("audio")] public AudioSource menuAudioSource;
    [FormerlySerializedAs("gameMusic")] public AudioSource gameAudioSource;
    [FormerlySerializedAs("audioImage")] public Image muteButtonBackgroundImage;
    //public GameObject connectionInfo;


    private void Awake()
    {
        _discoveryHelper = gameObject.AddComponent<DiscoveryHelper>(); 
    }

    private void Start()
    {
        gameAudioSource.enabled = false;
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
            
            buttonBackground.color = Color.cyan;
            buttonText.text = "Connect to game";

        }
    }

    public void PlayGame()
    {
        if (!_discoveryHelper.HasFoundBroadcast)
        {
            //Reset score
            FindObjectOfType<ScoreManager>().ResetScore();
            menuAudioSource.enabled = false;
            gameAudioSource.enabled = true;
            gameAudioSource.mute = menuAudioSource.mute;

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


    public void ChangeAudio()
    {
        menuAudioSource.mute = !menuAudioSource.mute;
        gameAudioSource.mute = !gameAudioSource.mute;

        if (menuAudioSource.mute)
        {
            muteButtonBackgroundImage.color = Color.green;
        }
        else
        {
            muteButtonBackgroundImage.color = Color.white;
        }
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}
