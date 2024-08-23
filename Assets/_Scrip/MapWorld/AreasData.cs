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
    public LevelDifficultInformation DifficultInformation;

}

[System.Serializable]
public class LevelInfomation
{
    [Header("Level Complete")]
    [Range(0, 3)] public int starCount;
    public bool isCompleted;

}
[System.Serializable]
public class LevelDifficultInformation
{
    // Phương thức trả về khó khăn Easy và thông tin cấp độ tương ứng
    public (Difficult, LevelInfomation) GetEasyLevelInfo()
    {
        return (Difficult.Easy, levelInfomation_easy);
    }

    // Phương thức trả về khó khăn Normal và thông tin cấp độ tương ứng
    public (Difficult, LevelInfomation) GetNormalLevelInfo()
    {
        return (Difficult.Normal, levelInfomation_normal);
    }

    // Phương thức trả về khó khăn Hard và thông tin cấp độ tương ứng
    public (Difficult, LevelInfomation) GetHardLevelInfo()
    {
        return (Difficult.Hard, levelInfomation_hard);
    }

    public LevelInfomation levelInfomation_easy;
    public LevelInfomation levelInfomation_normal;
    public LevelInfomation levelInfomation_hard;
}

public enum Difficult
{
    Easy , Normal , Hard
}

