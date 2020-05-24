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
    [FormerlySerializedAs("_networkManager")] public NetworkManager networkManager;
    public Image buttonBackground;
    public TextMeshProUGUI buttonText;
    public GameObject audioButton;

    [FormerlySerializedAs("audio")] public AudioSource menuAudioSource;
    [FormerlySerializedAs("gameMusic")] public AudioSource gameAudioSource;
    [FormerlySerializedAs("muteButtonBackgroundImage")] [FormerlySerializedAs("audioImage")] public Image audioButtonBackgroundImage;


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
            
            networkManager.networkAddress = IP;
            networkManager.networkPort = 7777;
            networkManager.StartClient();
            
            _hasConnected = true;
            
            buttonBackground.color = Color.cyan;
            buttonText.text = "Connect to game";

            gameAudioSource.enabled = false;
            audioButton.SetActive(false);

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

            networkManager.StartHost();
            
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
            audioButtonBackgroundImage.color = Color.green;
        }
        else
        {
            audioButtonBackgroundImage.color = Color.white;
        }
    }

    public void ChangeSettings()
    {
        Debug.Log("Settings management is not implemented yet");
    }
}
