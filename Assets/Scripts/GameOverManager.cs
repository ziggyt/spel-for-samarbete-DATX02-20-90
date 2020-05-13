﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    private void ShutDownNetwork()
    {
        Destroy(NetworkManager.singleton.gameObject);
        NetworkManager.Shutdown();
    }

    public void PlayAgain()
    {
        ShutDownNetwork();
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        ShutDownNetwork();
        Application.Quit();
    }

    private void Start()
    {
        Transform scoreTransform = transform.Find("Score");
        Text scoreText = scoreTransform.GetComponent<Text>();
        scoreText.text = "Score: " + PlayerPrefs.GetInt("score");

        playAgainButton.onClick.AddListener(PlayAgain);
    }
}
