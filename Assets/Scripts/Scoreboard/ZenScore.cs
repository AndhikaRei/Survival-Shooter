using System;

[Serializable]
public class ZenScore
{
    public string name;
    public int time;

    public ZenScore(string name, int time)
    {
        this.name = name;
        this.time = time;
    }
}
