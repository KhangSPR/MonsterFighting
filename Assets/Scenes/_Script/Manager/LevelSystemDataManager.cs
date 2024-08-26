using System;
using System.IO;
using UIGameDataMap;
using UnityEngine;

/// <summary>
/// This script hold the level data scriptable object and its Singleton and dont get deleted on scene change
/// </summary>
public class LevelSystemDataManager : MonoBehaviour
{
    [Header("Sử dụng phần này")]
    public AreaInfomationSO DatabaseAreaSO;
    private static LevelSystemDataManager instance;                             //instance variable
    public static LevelSystemDataManager Instance { get => instance; }          //instance getter

    private void OnEnable()
    {

    }
    private void OnApplicationQuit()
    {
        SaveAreasData();
    }
    #region Save LevelDataManager
    private string savePath => Path.Combine(Application.persistentDataPath, "areasData.json");
    public void SaveAreasData()
    {
        SaveAreaDataFromMapSO();

        string jsonData = JsonUtility.ToJson(DatabaseAreaSO);
        File.WriteAllText(savePath, jsonData);
        Debug.Log("AreasData Saved");
    }
    public void LoadAreasData()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(jsonData, DatabaseAreaSO);
            Debug.Log("AreasData Loaded");

            LoadMapSOFromAreaData();
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveAreasData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadAreasData();
        }
    }

    public void Awake()
    {
        if (LevelSystemDataManager.instance != null)
        {
            Debug.LogError("Only 1 LevelSystemManager Warning");
        }
        LevelSystemDataManager.instance = this;
        //LoadAreas();
    }

    [ContextMenu("Reset Area Data")]
    public void ResetAreaData()
    {
        DatabaseAreaSO.Reset();
    }
    public void LoadAreas(Transform FullMap)
    {
        //var FullMap = GameObject.Find("FullMap").transform;
        if (FullMap != null)
        {
            Debug.Log("Tìm thấy FullMap");
            var Maps_Icon = FullMap.Find("Maps_Icon");
            for (int i = 0; i < Maps_Icon.childCount; i++)
            {
                var icon = Maps_Icon.GetChild(i);
                AreasData data = new AreasData();
                data.areaIndex = i;
                data.areaName = icon.name;


                int levelCount = FullMap.Find("Maps").Find(data.areaName).Find("LvBtnHolder").Find("LvPostion").childCount;
                for (int j = 0; j < levelCount; j++)
                {
                    global::LevelData levelData = new global::LevelData();
                    levelData.levelIndex = j;
                    levelData.levelName = j + "";
                    levelData.isUnlocked = false;
                    //levelData.starCount = 0;

                    data.levelsData.Add(levelData);

                }
                //areasDatas.Add(data);

                //global::LevelData levelData = new global::LevelData();

                data.levelsData[0].isUnlocked = true;
            }
        }
    }
    private void LoadMapSOFromAreaData()
    {
        foreach (var area in DatabaseAreaSO.areasData)
        {
            foreach (var level in area.levelsData)
            {
                // Tìm `MapSO` dựa trên `levelIndex` và `areaIndex`
                MapSO mapSO = FindMapSO(level.levelIndex, area.areaIndex);

                if (mapSO != null)
                {
                    mapSO.id = level.levelIndex;
                    mapSO.mapType = (MapType)area.areaIndex; // Đảm bảo rằng mapSO cũng lưu areaIndex

                    Debug.Log(mapSO.mapType);
                    mapSO.Unlocked = level.isUnlocked;

                    // Cập nhật thông tin khác từ levelData sang MapSO
                    for (int i = 0; i < level.DifficultInformation.levelInfomations.Length; i++)
                    {                   
                        mapSO.DifficultyMap[i].stars = level.DifficultInformation.levelInfomations[i].starCount;
                        mapSO.DifficultyMap[i].isReceivedReWard = level.DifficultInformation.levelInfomations[i].isCompleted;
                    }
                }
            }
        }
    }

    private void SaveAreaDataFromMapSO()
    {
        foreach (var area in DatabaseAreaSO.areasData)
        {
            foreach (var level in area.levelsData)
            {
                // Tìm `MapSO` dựa trên `levelIndex` và `areaIndex`
                MapSO mapSO = FindMapSO(level.levelIndex, area.areaIndex);

                if (mapSO != null)
                {
                    level.isUnlocked = mapSO.Unlocked;
                    for (int i = 0; i < mapSO.DifficultyMap.Length; i++)
                    {
                        level.DifficultInformation.levelInfomations[i].starCount = mapSO.DifficultyMap[i].stars;
                        level.DifficultInformation.levelInfomations[i].isCompleted = mapSO.DifficultyMap[i].isReceivedReWard;
                    }
                }
            }
        }
    }

    private MapSO FindMapSO(int levelIndex, int areaIndex)
    {
        // Tìm `MapSO` dựa trên `levelIndex` và `areaIndex` từ `MapManager`
        MapArrayData[] mapArrayDatas = MapManager.Instance.MapArrayData;

        if(mapArrayDatas.Length >0)
        {
            Debug.Log("MapArrayData = 0");
            return null;
        }

        for (int i = 0; i < mapArrayDatas.Length; i++)
        {
            MapSO[] mapSOs = mapArrayDatas[i].MapSOArray;
            foreach (MapSO mapSO in mapSOs)
            {
                if (mapSO.id == levelIndex && i == areaIndex)
                {
                    return mapSO;
                }
            }
        }
        return null; // Nếu không tìm thấy
    }

}
//[Header("Bỏ Qua Phần Bên Dưới")]
//private static LevelSystemManager instance;                             //instance variable
//public static LevelSystemManager Instance { get => instance; }          //instance getter

//[SerializeField] private LevelData levelData;

//public LevelData LevelData { get => levelData; }   //getter

//private int currentLevel;                                               //keep track of current level player is playing
//public int CurrentLevel { get => currentLevel; set => currentLevel = value; }   //getter and setter for currentLevel



//private void Update()
//{
//    if (Input.GetKeyDown(KeyCode.Space))
//    {
//        levelData.lastUnlockedLevel = 3;


//        SaveLoadData.Instance.SaveData();
//    }
//}

//private void OnEnable()
//{



//    SaveLoadData.Instance.Initialize();
//}

//public void LevelComplete(/*int starAchieved*/)                             //method called when player win the level
//{
//    //*            levelData.levelItemArray[currentLevel].starAchieved = starAchieved; *//*   //save the stars achieved by the player in level
//    if (levelData.lastUnlockedLevel < (currentLevel + 1))
//    {
//        levelData.lastUnlockedLevel = currentLevel + 1;           //change the lastUnlockedLevel to next level
//                                                                  //and make next level unlock true
//        levelData.levelItemArray[levelData.lastUnlockedLevel].unlocked = true;
//    }
//}
//}
