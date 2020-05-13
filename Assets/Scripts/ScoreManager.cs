using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    // Variables
    [SyncVar] [SerializeField] private int score = 0;
    private Text scoreText;

    private void Awake()
    {
    }

    void Start()
    {
        DontDestroyOnLoad(this.transform.parent);
        scoreText = GetComponent<Text>();
        UpdateScoreText();
    }

    // Update text with current score
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Add score
    [Server]
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
    [Server]
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
