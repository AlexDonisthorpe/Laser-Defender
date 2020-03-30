using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TMP_Text scoreText;
    GameSession gameSession;
    // Update is called once per frame

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        scoreText.text = gameSession.GetScore().ToString();
        
    }
}
