using System;
using System.Collections.Generic;

[Serializable]
public class WaveScoreData
{
    public List<WaveScore> scores;
    
    public WaveScoreData()
    {
        scores = new List<WaveScore>();
    }
}
