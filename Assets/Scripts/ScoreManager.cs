using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    // Variables
    [SyncVar] [SerializeField] private int score = 0;
    private int value = 1;
    private Text scoreText;
    
    void Start()
    {
        scoreText = GetComponent<Text>();
        UpdateScoreText();
    }

    // Save score to local storage when scene close
    private void OnDisable()
    {
        PlayerPrefs.SetInt("score", score);
    }

    // Update text with current score
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Add score
    public void AddScore()
    {
        score++; 
        UpdateScoreText();
    }

    // Return score
    public int GetScore()
    {
        return score;
    }

    // Set score to 0
    public void ResetScore()
    {
//        score = 0;
//        UpdateScoreText();
    }
}
