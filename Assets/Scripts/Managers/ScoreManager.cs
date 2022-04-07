using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public static float survival_time;
    public PlayerHealth playerHealth;

    public int interval = 1;

    // Game mode manager.
    public GameModeManager gameModeManager;

    Text text;
    public TMP_Text gameOverTimeText;
    public TMP_Text gameOverScoreText;


    void Awake ()
    {
        // Mendapatkan reference komponen.
        text = GetComponent <Text> ();
        gameModeManager = GameObject.Find("GameModeManager").GetComponent<GameModeManager>();

        // Variable inisialisasi.
        score = 0;
        survival_time = 0;
    }


    void Update ()
    {
        // Update score text based on game mode.
        if (GameModeManager.gameMode == GameMode.Zen)
        {
            // Update and display survival time per second.
            if (Time.timeSinceLevelLoad > survival_time + interval && playerHealth.currentHealth > 0)
            {
                survival_time = Time.timeSinceLevelLoad;
            }
            // Format the displayed survival time to non zero precision (int).

            text.text = "Time: " + Math.Floor((survival_time)) + " s";
        }
        else if (GameModeManager.gameMode == GameMode.Wave)
        {
            text.text = "Score: " + score;
        }
    }
}
