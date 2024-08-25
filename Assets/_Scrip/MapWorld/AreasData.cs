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
    public Difficult difficult;
    [Range(0, 3)] public int starCount;
    public bool isCompleted;

}
[System.Serializable]
public class LevelDifficultInformation
{
    public LevelInfomation[] levelInfomations = new LevelInfomation[3];
    private void Awake()
    {
        InitializeDifficultyMap();
    }

    private void InitializeDifficultyMap()
    {
        if (levelInfomations == null || levelInfomations.Length != 3)
        {
            levelInfomations = new LevelInfomation[3];
        }

        for (int i = 0; i < 3; i++)
        {
            if (levelInfomations[i] == null)
            {
                levelInfomations[i] = new LevelInfomation
                {
                    difficult = (Difficult)i,
                };
            }
        }
    }
}

public enum Difficult
{
    Easy , Normal , Hard
}

