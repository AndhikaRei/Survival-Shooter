using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public WaveRowUI waveRowUI;
    public ZenRowUI zenRowUI;
    public ScoreboardManager scoreboardManager;

    public GameObject waveScoreboard;
    public GameObject zenScoreboard;

    // Start is called before the first frame update
    void Start()
    {
        //scoreboardManager.AddZenScore(new ZenScore("Test2", 4));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));
        //scoreboardManager.AddZenScore(new ZenScore("Test", 10));

        //scoreboardManager.AddWaveScore(new WaveScore("Test2", 4, 100));
        //scoreboardManager.AddWaveScore(new WaveScore("Test", 2, 35));

        // Initiate wave scoreboard
        var waveScores = scoreboardManager.GetWaveScores().ToList();

        for (int i = 0; i < waveScores.Count(); i++)
        {
            var row = Instantiate(waveRowUI, waveScoreboard.transform).GetComponent<WaveRowUI>();
            row.rank.text = (i + 1).ToString();
            row.playerName.text = waveScores[i].name;
            row.wave.text = waveScores[i].wave.ToString();
            row.score.text = waveScores[i].score.ToString();
        }

        // Initiate zen scoreboard
        var zenScores = scoreboardManager.GetZenScores().ToList();

        for (int i = 0; i < zenScores.Count(); i++)
        {
            var row = Instantiate(zenRowUI, zenScoreboard.transform).GetComponent<ZenRowUI>();
            row.rank.text = (i + 1).ToString();
            row.playerName.text = zenScores[i].name;
            row.time.text = zenScores[i].time.ToString() + " s";
        }
    }
}
