using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;       
    public GameObject gameOverScreen;
    public EnemyManager enemyManager;
    public ScoreboardManager scoreboardManager;
    public TMP_Text waveTimeText;
    public TMP_Text waveTimeval;
    public TMP_Text scoreText;
    public TMP_Text scoreVal;

    Animator anim;                                      
    public Text warningText;
    bool isDead;

    void Awake()
    {
        if (GameModeManager.gameMode == GameMode.Zen)
        {
            waveTimeText.text = "Time:";
            scoreText.text = "";
            scoreVal.text = "";
        }
        else if (GameModeManager.gameMode == GameMode.Wave)
        {
            waveTimeText.text = "Max Wave:";
            scoreText.text = "Score:";
        }
        anim = GetComponent<Animator>();
        isDead = false;
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0 && !isDead)
        {
            isDead = true;
            if (GameModeManager.gameMode == GameMode.Zen)
            {
                waveTimeval.text = Math.Floor((ScoreManager.survival_time)) + " s";
                scoreboardManager.AddZenScore(new ZenScore(PlayerHealth.playerName, Convert.ToInt32(Math.Floor(ScoreManager.survival_time))));
            }
            else if (GameModeManager.gameMode == GameMode.Wave)
            {
                waveTimeval.text = enemyManager.currentWave.ToString();
                scoreVal.text = ScoreManager.score.ToString();
                scoreboardManager.AddWaveScore(new WaveScore(PlayerHealth.playerName, enemyManager.currentWave, ScoreManager.score));
            }

            gameOverScreen.SetActive(true);
            anim.SetTrigger("GameOver");
        }
    }

    public void ShowWarning(float enemyDistance)
    {
        warningText.text = string.Format("! {0} m",Mathf.RoundToInt(enemyDistance));
        anim.SetTrigger("Warning");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NewGame()
    {
        WeaponUpgradeManager.maxUpgrade = 0;
        WeaponUpgradeManager.aspdUpgrade = 0;
        WeaponUpgradeManager.diagonalArrowUpgrade = 0;

        gameOverScreen.SetActive(false);
        SceneManager.LoadScene(1);
    }
}