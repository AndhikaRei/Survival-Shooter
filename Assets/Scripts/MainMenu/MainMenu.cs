using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectGameMode(int val)
    {
        switch (val) {
            case 0:
                GameModeManager.gameMode = GameMode.Zen;
                break;
            case 1:
                GameModeManager.gameMode = GameMode.Wave;
                break;
        }
    }

    public void SetPlayerName(string name) 
    {
        Debug.Log(name);
        PlayerHealth.playerName = name;
    }
}
