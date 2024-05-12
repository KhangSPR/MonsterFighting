using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UIGameDataMap;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class AreasData
{
    [Min(0)]
    public int areaIndex;
    public string areaName;

    public List<LevelData> levelsData;
}

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public string levelName { get { return levelIndex.ToString(); } set { } }
    public bool isUnlocked;
    [Range(0,3)]public int starCount;
    public LevelDifficultInformation DifficultInformation;
    //public string RomanLevelIndex(int index)
    //{
    //    string[] romanSymbols = {"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X",
    //                             "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX"};
    //    Debug.Log(romanSymbols[index]);
    //    return romanSymbols[index];
    //}
}

[System.Serializable]
public class LevelInfomation
{
    [Header("Level Infomation Here")]
    public bool LevelInformation_Temp; // biến tạm
                                       // Thông số quái , chỉ số máu , tướng ... code ở đây
}
[System.Serializable]
public class LevelDifficultInformation
{
    [ReadOnlyInspector]
    public Difficult Easy = Difficult.Easy;
    public LevelInfomation levelInfomation_easy;
    [ReadOnlyInspector]
    public Difficult Normal = Difficult.Normal;
    public LevelInfomation levelInfomation_normal;
    [ReadOnlyInspector]
    public Difficult Hard = Difficult.Hard;
    public LevelInfomation levelInfomation_hard;

}
public enum Difficult
{
    Easy , Normal , Hard
}

