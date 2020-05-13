using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Text scoreText;

    private void ShutDownNetwork()
    {
        Destroy(NetworkManager.singleton.gameObject);
        NetworkManager.Shutdown();
    }

    public void PlayAgain()
    {
        ShutDownNetwork();
        Destroy(GameObject.FindGameObjectWithTag("ScoreCanvas"));
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        ShutDownNetwork();
        Application.Quit();
    }

    private void Start()
    {
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        int score = sm.GetScore();
        Debug.Log("Game over: " + score);
        scoreText.text = "Score: " + score;
        PlayerPrefs.DeleteAll();

        playAgainButton.onClick.AddListener(PlayAgain);
    }
}
