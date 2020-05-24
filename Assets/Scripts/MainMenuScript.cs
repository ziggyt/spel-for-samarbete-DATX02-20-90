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
        
        if (!_discoveryHelper.HasFoundBroadcast && !_discoveryHelper.running)
        {
            _discoveryHelper.initDiscoveryHelper();
        }
        

        if (_discoveryHelper.HasFoundBroadcast && !_hasConnected)
        {
            _discoveryHelper.StopAllCoroutines();
            _discoveryHelper.StopBroadcast();
            
            String IP = _discoveryHelper.ServerIp;
            
            _networkManager.networkAddress = IP;
            _networkManager.networkPort = 7777;
            _networkManager.StartClient();
            
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
            ScoreManager sm = FindObjectOfType<ScoreManager>();
            if (sm != null)
            {
                sm.ResetScore();
            }
            menuAudioSource.enabled = false;
            gameAudioSource.enabled = true;
            gameAudioSource.mute = menuAudioSource.mute;
            
            _discoveryHelper.StopBroadcast();

            _networkManager.StartHost();
            
            _discoveryHelper.StartAsServer();
            
        }

        GameObject[] uiComponents = GameObject.FindGameObjectsWithTag("MenuUIComponent");

        foreach (GameObject uiComponent in uiComponents)
        {
            uiComponent.SetActive(false);
        }
        
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
