using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


// Declare enumerable for game mode. The available game mode is zen/endless and wave mode.
public enum GameMode
{
    Zen,
    Wave
}

public class GameModeManager : MonoBehaviour
{
    // Attribute initialization.
    public static GameMode gameMode = GameMode.Zen;
    // Enemy manager.
    public EnemyManager enemyManager;

    // Maximum Wave.
    public const int maxWave = EnemyManager.maxWave;

    Text gameModeText;
    Text waveText;

    // Start is called before the first frame update
    void Awake()
    {
        // TODO: Get the game mode from main menu.
        //gameMode = GameMode.Wave;

        // Get the gameModeText from hudcanvas.
        gameModeText = GameObject.Find("GameMode").GetComponent<Text>();
        waveText = GameObject.Find("Wave").GetComponent<Text>();

        // Get other object.
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        Debug.Log(gameMode);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the game mode text based on game mode.
        if (gameMode == GameMode.Zen)
        {
            gameModeText.text = "Zen Mode";
            waveText.text = "";
        }
        else if (gameMode == GameMode.Wave)
        {
            // Update the wave text.
            gameModeText.text = "Wave Mode";
            waveText.text = "Wave " + enemyManager.currentWave + " / " + maxWave;
        }
    }

    // Function to change into Zen Mode
    void startZenMode()
    {
        // Set game mode to Zen mode.
        gameMode = GameMode.Zen;
    }

    // Function to change into Wave Mode
    void startWaveMode()
    {
        // Set game mode to Wave mode.
        gameMode = GameMode.Wave;
    }
}
