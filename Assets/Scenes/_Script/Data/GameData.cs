using UnityEngine;
using UnityEngine.UIElements;
using System;



// stores consumable data (resources)
[System.Serializable]
public class GameData
{
    public uint gold = 500;
    public uint enemyBoss = 50;
    public uint enemyStone = 50;
    public uint xpLv1 = 50;
    public uint xpLv2 = 25;
    public uint xpLv3 = 10;
    public uint xpLv4 = 1;
    //public uint healthPotions = 6;
    //public uint levelUpPotions = 80;

    //public string username;
    //public string theme;

    //public float musicVolume;
    //public float sfxVolume;

    //// non-functional, used for saving SettingsScreen values
    //public bool isSlideToggled;
    //public bool isToggled;
    //public string dropdownSelection;
    //public int buttonSelection;

    // constructor, starting values
    public GameData()
    {
        // player stats/data
        this.gold = 200;
        this.enemyBoss = 50;
        this.enemyStone = 50;
        this.xpLv1  = 50;
        this.xpLv2 = 25;
        this.xpLv3 = 10;
        this.xpLv4 = 1;
        //this.healthPotions = 6;
        //this.levelUpPotions = 80;

        //// settings
        //this.musicVolume = 80f;
        //this.sfxVolume = 80f;

        //this.username = "GUEST_123456";
        //this.dropdownSelection = "Item1";
        //this.theme = "Default";
        //this.buttonSelection = 2;

        //this.isSlideToggled = true;
        //this.isToggled = true;
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
