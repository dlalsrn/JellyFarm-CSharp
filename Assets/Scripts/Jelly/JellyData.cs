using System.Collections.Generic;

[System.Serializable]
public class JellyData
{
    public int Id;
    public int Level;
    public float Exp;

    public JellyData(int id, int level, float exp)
    {
        Id = id;
        Level = level;
        Exp = exp;
    }
}

[System.Serializable]
public class JellyDataList
{
    public List<JellyData> jellyDataList;
}