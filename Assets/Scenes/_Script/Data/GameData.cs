using UnityEngine;

[System.Serializable]
public class GameData
{
    public uint badGe = 5;
    public uint StoneBoss = 50;
    public uint StoneEnemy = 50;
    public uint ruby = 1499;

    public Settings settings;

    // constructor, starting values
    public GameData()
    {
        // player stats/data
        this.badGe = 5;
        this.StoneBoss = 50;
        this.StoneEnemy = 50;
        this.ruby = 1499;

    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadJson(string jsonFilepath)
    {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this);
    }
}
