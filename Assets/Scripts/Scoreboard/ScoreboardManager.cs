using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    private WaveScoreData waveScores;
    private ZenScoreData zenScores;

    void Awake()
    {
        var waveJson = PlayerPrefs.GetString("wave_scores", "{}");
        var zenJson = PlayerPrefs.GetString("zen_scores", "{}");

        waveScores = JsonUtility.FromJson<WaveScoreData>(waveJson);
        zenScores = JsonUtility.FromJson<ZenScoreData>(zenJson);

        if (waveScores == null)
        {
            waveScores = new WaveScoreData();
        }

        if (zenScores == null)
        {
            zenScores = new ZenScoreData();
        }
    }

    public IEnumerable<WaveScore> GetWaveScores()
    {
        return waveScores.scores.OrderByDescending(x => x.score);
    }

    public IEnumerable<ZenScore> GetZenScores()
    {
        return zenScores.scores.OrderByDescending(x => x.time);
    }

    public void AddWaveScore(WaveScore score)
    {
        waveScores.scores.Add(score);
    }

    public void AddZenScore(ZenScore score)
    {
        zenScores.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScores();
    }

    public void SaveScores()
    {
        var waveJson = JsonUtility.ToJson(waveScores);
        var zenJson = JsonUtility.ToJson(zenScores);

        PlayerPrefs.SetString("wave_scores", waveJson);
        PlayerPrefs.SetString("zen_scores", zenJson);
    }

}
