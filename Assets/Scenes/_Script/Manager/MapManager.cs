using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIGameDataMap
{
    [System.Serializable]
    public class MapArrayData
    {
        [SerializeField] private MapSO[] mapSOArray;
        public MapSO[] MapSOArray => mapSOArray;

        public void SetMapSOArray(MapSO[] value)
        {
            mapSOArray = value;
        }
    }

    public class MapManager : SaiMonoBehaviour
    {
        private static MapManager instance;
        public static MapManager Instance => instance;

        [Header("PrefabMap")]
        public GameObject gameMapPrefab;

        private GameObject currentMap = null;
        public GameObject CurrentMap => currentMap;

        [SerializeField] private MapArrayData[] mapArrayData;
        public MapArrayData[] MapArrayData => mapArrayData;

        [SerializeField] private int m_ArrayCurrentIndex;
        public int ArrayCurrentIndex => m_ArrayCurrentIndex;
        [SerializeField] private int m_MapSOCurrentIndex;
        public int MapSOCurrentIndex => m_MapSOCurrentIndex;

        private MapArrayData _mapArrayCurrent;
        public MapArrayData MapArrayCurrent
        {
            get => mapArrayData[m_ArrayCurrentIndex];
            //set => _mapArrayCurrent = value;
        }

        [SerializeField] private MapSO _mapSOCurrent;
        public MapSO MapSOCurrent => _mapSOCurrent;
        public void SetMapSOCurrent()
        {
            if (m_MapSOCurrentIndex == -1) m_MapSOCurrentIndex = 0;

            MapSO mapSO = mapArrayData[m_ArrayCurrentIndex].MapSOArray[m_MapSOCurrentIndex];
            if (mapSO == null) return;
            _mapSOCurrent = mapSO;

            if (_mapSOCurrent.id == 0)
            {
                Debug.Log("True");
                _mapSOCurrent.Unlocked = true;
            }
        }
        [SerializeField]
        private Difficult difficult;
        public Difficult Difficult
        {
            get => difficult;
            set => difficult = value; // Only this class can set this property
        }

        private static readonly string[] mapSOFolders = { "Map/GrassChoosen", "Map/LavaChoosen" };

        protected override void Start()
        {
            base.Start();


            InitializeMapSOArray();
            m_MapSOCurrentIndex = GetCurrentSOLevelLastUnLockArea();
            m_ArrayCurrentIndex = GetCurrentArrayLastUnlockArea();
            //difficult = GetCurrentDifficultUnlockMap();

            SetMapSOCurrent();


            Debug.Log("Map current: " + _mapSOCurrent);
        }
        protected override void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Only 1 MapManager Warning");
            }
            instance = this;
        }

        private void InitializeMapSOArray()
        {
            mapArrayData = new MapArrayData[mapSOFolders.Length];

            for (int i = 0; i < mapSOFolders.Length; i++)
            {
                // Khởi tạo từng phần tử trong mảng
                mapArrayData[i] = new MapArrayData();

                MapSO[] mapSOs = LoadMapSO(mapSOFolders[i]);
                mapArrayData[i].SetMapSOArray(mapSOs);
            }
        }

        private MapSO[] LoadMapSO(string path)
        {
            MapSO[] mapSOs = UnityEngine.Resources.LoadAll<MapSO>(path);
            Array.Sort(mapSOs, (x, y) => x.id.CompareTo(y.id));
            return mapSOs;
        }

        public void LoadNextMap()
        {
            if (m_MapSOCurrentIndex < _mapArrayCurrent.MapSOArray.Length - 1)
            {
                m_MapSOCurrentIndex++;
                _mapSOCurrent = _mapArrayCurrent.MapSOArray[m_MapSOCurrentIndex];

                Debug.Log("MapSO: " + _mapSOCurrent + " Index: " + m_MapSOCurrentIndex);

                ReloadMap();
            }
            else if (m_ArrayCurrentIndex < mapArrayData.Length - 1)
            {
                m_ArrayCurrentIndex++;
                _mapArrayCurrent = mapArrayData[m_ArrayCurrentIndex];
                m_MapSOCurrentIndex = 0;
                _mapSOCurrent = _mapArrayCurrent.MapSOArray[m_MapSOCurrentIndex];

                Debug.Log("Next MapArray loaded. MapSO: " + _mapSOCurrent + " Index: " + m_MapSOCurrentIndex);

                ReloadMap();
            }
        }

        public void LoadMap()
        {
            currentMap = Instantiate(gameMapPrefab);
            currentMap.SetActive(true);
        }

        public void ReloadMap()
        {
            Destroy(currentMap);
            LoadMap();
        }

        public void SetCurrentMapSO(MapSO mapSO)
        {
            for (int i = 0; i < mapArrayData.Length; i++)
            {
                int index = Array.FindIndex(mapArrayData[i].MapSOArray, element => element == mapSO);
                if (index != -1)
                {
                    m_ArrayCurrentIndex = i;
                    m_MapSOCurrentIndex = index;
                    _mapArrayCurrent = mapArrayData[i];
                    _mapSOCurrent = mapSO;
                    break;
                }
            }
        }

        private int GetCurrentSOLevelLastUnLockArea()
        {
            List<AreasData> areas = LevelSystemDataManager.Instance.DatabaseAreaSO.areasData;
            int firstLockedLevelIndex = -1;
            foreach (AreasData area in areas)
            {
                List<LevelData> levels = area.levelsData;
                foreach (LevelData level in levels)
                {
                    if (!level.isUnlocked)
                    {
                        firstLockedLevelIndex = level.levelIndex;
                        return firstLockedLevelIndex - 1;
                    }
                }
            }
            return 0;
        }
        private int GetCurrentArrayLastUnlockArea()
        {
            List<AreasData> areas = LevelSystemDataManager.Instance.DatabaseAreaSO.areasData;
            int firstLockedLevelIndex = -1;
            foreach (AreasData area in areas)
            {
                List<LevelData> levels = area.levelsData;
                foreach (LevelData level in levels)
                {
                    if (!level.isUnlocked)
                    {
                        firstLockedLevelIndex = areas.IndexOf(area);
                        return firstLockedLevelIndex;
                    }
                }

            }
            return 0;
        }
        //private Difficult GetCurrentDifficultUnlockMap()
        //{
        //    List<AreasData> areas = LevelSystemDataManager.Instance.DatabaseAreaSO.areasData;

        //    Difficult difficult = Difficult.Easy; // Default Easy

        //    if (areas[ArrayCurrentIndex].levelsData[m_MapSOCurrentIndex].isUnlocked)
        //    {
        //        LevelInfomation[] levelInfomations = areas[ArrayCurrentIndex].levelsData[m_MapSOCurrentIndex].DifficultInformation.levelInfomations;
        //        foreach (LevelInfomation levelInfomation in levelInfomations)
        //        {
        //            if(!levelInfomation.isCompleted)
        //            {
        //                difficult = levelInfomation.difficult;
        //                return difficult; // return Difficult
        //            }
        //        }
        //    }

        //    // return Difficult
        //    return difficult;
        //}
        public LevelSettings LoadCurrentLevelSettings()
        {
            LevelSettings levelSettings = LevelSystemDataManager.Instance.LoadLevelSettings(m_ArrayCurrentIndex, m_MapSOCurrentIndex, Difficult.ToString());

            if (levelSettings == null)
            {
                Debug.Log("Null");
                return null;
            }

            Debug.Log("Not Null");

            return levelSettings;
        }
        public void SetStarDifficult(int star)
        {
            _mapSOCurrent.SetStarsCount(difficult, star);
        }
        public void SetReward()
        {
            MapDifficulty difficulty =
            _mapSOCurrent.GetMapDifficult(difficult);
            difficulty.isReceivedReWard = true;
        }
        public void UnLockNextMap()
        {
            foreach (var mapArray in mapArrayData)
            {
                for (int i = 0; i < mapArray.MapSOArray.Length; i++)
                {
                    MapSO so = mapArray.MapSOArray[i];

                    if (so == _mapSOCurrent) 
                    {
                        if (i + 1 < mapArray.MapSOArray.Length)
                        {
                            MapSO nextMapSO = mapArray.MapSOArray[i + 1];

                            if (CheckUnlocked(nextMapSO)) 
                            {
                                nextMapSO.Unlocked = true;
                                m_MapSOCurrentIndex++;
                                return;
                            }
                        }
                        else
                        {
                            Debug.Log("Không còn bản đồ tiếp theo để mở khóa.");
                        }

                        return; // Ngừng vòng lặp sau khi đã xử lý
                    }
                }
            }
        }

        private bool CheckUnlocked(MapSO mapSO)
        {
            if (mapSO == null) return false;
            return !mapSO.Unlocked; // If not unlocked, return true
        }
    }
}
