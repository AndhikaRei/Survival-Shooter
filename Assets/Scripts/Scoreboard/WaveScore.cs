using System;

[Serializable]
public class WaveScore
{
    public string name;
    public int wave;
    public int score;

    public WaveScore(string name, int wave, int score)
    {
        this.name = name;
        this.wave = wave;
        this.score = score;
    }
}
