using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LivesManager : NetworkBehaviour
{
    // Variables
    [SyncVar] [SerializeField] private int lives = 10;
    private Text livesText;

    void Start()
    {
        livesText = GetComponent<Text>();
        UpdateText();
    }

    // Updates the lives text with current lives
    private void UpdateText()
    {
        livesText.text = "Lives: " + lives;
    }

    // Returns current lives
    public int GetLives()
    {
        return lives;
    }
    
    // Decreases life, updates text and returns lives
    public void RemoveLife()
    {
        lives--;
        UpdateText();
    }
}
