using System;
using System.Collections.Generic;

[Serializable]
public class ZenScoreData
{
    public List<ZenScore> scores;

    public ZenScoreData()
    {
        scores = new List<ZenScore>();
    }
}
