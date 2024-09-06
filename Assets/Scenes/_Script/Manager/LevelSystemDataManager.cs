using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UIGameDataMap;
using UnityEngine;

/// <summary>
/// This script hold the level data scriptable object and its Singleton and dont get deleted on scene change
/// </summary>
public class LevelSystemDataManager : MonoBehaviour
{
    [Header("Area Database")]
    public AreaInfomationSO DatabaseAreaSO;
    [SerializeField]
    TextAsset csvFile;

    private static LevelSystemDataManager instance;                             //instance variable
    public static LevelSystemDataManager Instance { get => instance; }          //instance getter

    private void Start()
    {
        LoadAreasData();
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

        if (mapArrayDatas.Length < 0)
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
    public LevelSettings LoadLevelSettings(int areaId, int lvId, string difficult)
    {
        LevelSettings levelSettings = new LevelSettings();

        // Tách dữ liệu từ CSV
        string[] lines = csvFile.text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Bỏ qua dòng tiêu đề (header)
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            // Kiểm tra số lượng cột
            if (fields.Length < 9)
            {
                Debug.LogWarning($"Dòng {i + 1} không đủ số cột.");
                continue;
            }

            // Lấy giá trị từ các cột
            if (!int.TryParse(fields[0], out int area))
            {
                Debug.LogWarning($"Không thể chuyển đổi AreaId: {fields[0]}");
                continue;
            }
            if (!int.TryParse(fields[1], out int lv))
            {
                Debug.LogWarning($"Không thể chuyển đổi LvID: {fields[1]}");
                continue;
            }
            string diff = fields[2];

            string des = fields[9];

            Debug.Log(fields[0] + fields[1] + fields[2] + fields[3] + fields[4] + fields[5] + fields[6] + fields[7] + fields[8]);

            // Kiểm tra điều kiện khớp
            if (area != areaId || lv != lvId || !diff.Equals(difficult, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Nếu tất cả các điều kiện đều đúng, tiếp tục xử lý các cột tiếp theo
            string[] conditionTypes = { fields[3], fields[4], fields[5] };
            string[] conditionValues = { fields[6], fields[7], fields[8] };

            for (int j = 0; j < conditionTypes.Length; j++)
            {
                ILevelCondition condition = null;
                var values = conditionValues[j].Split('-');

                // Xử lý các điều kiện dựa trên conditionTypes[j]
                switch (conditionTypes[j])
                {
                    case "Hp":
                        condition = new HpPercentageCondition();
                        if (values.Length > 0 && float.TryParse(values[0], out float hpPercentage))
                        {
                            ((HpPercentageCondition)condition).RequiredHpPercentage = hpPercentage;
                        }
                        break;
                    case "Hp-Time":
                        condition = new HpAndTimeCondition();
                        if (values.Length > 0 && float.TryParse(values[0], out float hpTimePercentage))
                        {
                            ((HpAndTimeCondition)condition).RequiredHpPercentage = hpTimePercentage;
                        }
                        if (values.Length > 1 && float.TryParse(values[1], out float completionTime))
                        {
                            ((HpAndTimeCondition)condition).RequiredCompletionTime = completionTime;
                        }
                        break;
                    case "Time":
                        condition = new TimeCondition();
                        if (values.Length > 0 && float.TryParse(values[0], out float timeRequired))
                        {
                            ((TimeCondition)condition).RequiredTime = timeRequired;
                        }
                        break;
                        // Thêm các trường hợp khác nếu cần
                }

                if (condition != null)
                {
                    levelSettings.starConditions.Add(condition);

                }

                Debug.Log("starConditions count: " + (levelSettings?.starConditions.Count ?? 0));
            }
            levelSettings.levelName = des;


            return levelSettings; // Trả về kết quả khi đã tìm thấy
        }

        // Nếu không tìm thấy kết quả
        Debug.LogWarning("Không thể tải LevelSettings cho các tham số đã cho.");
        return null;
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
