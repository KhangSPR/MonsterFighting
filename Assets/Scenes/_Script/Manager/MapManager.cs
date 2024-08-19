using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGameDataMap
{
    public class MapManager : SaiMonoBehaviour
    {
        private static MapManager instance;
        public static MapManager Instance => instance;

        [Header("PrefabMap")]
        public GameObject gameMapPrefab;

        [Space]
        [Space]
        [Space]
        [Space]
        [Header("MapSOArray")]
        private GameObject currentMap = null;
        public GameObject CurrentMap => currentMap;

        [SerializeField] MapSO[] mapSOArray;
        public MapSO[] MapSOArray => mapSOArray;

        // Here, we added a private setter to enable setting this property within this class
        private MapSO _mapSOCurrent;
        public MapSO MapSOCurrent
        {
            get => mapSOArray[m_CurrentIndex];
            private set => _mapSOCurrent = value; // Only this class can set this property
        }

        [SerializeField]
        int m_CurrentIndex;
        public int CurrentIndex => m_CurrentIndex;
        const string mapSOFolder = "Map/";

        protected override void Start()
        {
            base.Start();
            mapSOArray = loadMapSO(mapSOFolder);
            m_CurrentIndex = GetLevelLastUnLockArea();
        }

        protected override void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Only 1 MapManager Warning");
            }
            instance = this;
        }

        MapSO[] loadMapSO(string path)
        {
            MapSO[] mapSOs = UnityEngine.Resources.LoadAll<MapSO>(path);
            Array.Sort(mapSOs, (x, y) => x.id.CompareTo(y.id)); //Sort
            return mapSOs;
        }

        public void LoadNextMap()
        {
            if (m_CurrentIndex != -1)
            {
                int nextIndex = m_CurrentIndex + 1;
                nextIndex = Math.Clamp(nextIndex, 0, mapSOArray.Length);
                _mapSOCurrent = mapSOArray[nextIndex]; // Set the current MapSO

                if(_mapSOCurrent != null)
                    m_CurrentIndex++;

                Debug.Log("MapSO: " + _mapSOCurrent+" Index: "+ m_CurrentIndex);

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
            var newCurrentMap = currentMap;
            Destroy(currentMap.gameObject);
            LoadMap();
        }
        public void SetCurrentMapSO(MapSO mapSO)
        {
            if (mapSO != null && Array.IndexOf(mapSOArray, mapSO) != -1)
            {
                _mapSOCurrent = mapSO;
                m_CurrentIndex = Array.IndexOf(mapSOArray, mapSO);
            }
        }

        #region Get Level Last UnLock Area
        int GetLevelLastUnLockArea()
        {
            List<AreasData> areas = LevelSystemManager.Instance.aiso.areasData;
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
            return 0; // Default to zero if all levels are unlocked
        }
        #endregion
    }
}
