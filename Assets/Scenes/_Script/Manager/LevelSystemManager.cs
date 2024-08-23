using UnityEngine;

/// <summary>
/// This script hold the level data scriptable object and its Singleton and dont get deleted on scene change
/// </summary>
public class LevelSystemManager : MonoBehaviour
{
    [SerializeField] Transform FullMap;
    [Header("Sử dụng phần này")]
    public AreaInfomationSO DatabaseAreaSO;
    private static LevelSystemManager instance;                             //instance variable
    public static LevelSystemManager Instance { get => instance; }          //instance getter
    public void Awake()
    {
        if (LevelSystemManager.instance != null)
        {
            Debug.LogError("Only 1 LevelSystemManager Warning");
        }
        LevelSystemManager.instance = this;
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
