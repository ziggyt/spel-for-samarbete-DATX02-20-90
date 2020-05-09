using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Variables
    public static int scoreValue;
    private Text scoreText;
    
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    void Update()
    {
        scoreText.text = "Score: " + scoreValue;
    }
}
