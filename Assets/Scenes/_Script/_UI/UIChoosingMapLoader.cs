using System;
using UnityEngine;

namespace UIGameDataMap
{
    public class UIChoosingMapLoader : SaiMonoBehaviour
    {
        private MapSO mapSO;

        public static event Action<LevelSettings> LevelSettingsChanged;

        public void onClickExit()
        {
            Application.Quit();
        }

        public void LoadNextLevel() // Apply only for Grass area
        {
            MapManager.Instance.LoadNextMap();
            //MapManager.Instance.ReloadMap();

            mapSO = MapManager.Instance.MapSOCurrent;

            Debug.Log(mapSO);

            MapManager.Instance.Difficult = Difficult.Easy;

            LoadGame(mapSO, MapManager.Instance.Difficult);
        }

        public void ReloadMap()
        {
            MapManager.Instance.ReloadMap();
            //UIManager.Instance.DetiveGameUI();

            mapSO = MapManager.Instance.MapSOCurrent;


            LoadGame(mapSO, MapManager.Instance.Difficult);
        }
        public void LoadNextDifficult()
        {
            MapManager.Instance.ReloadMap();

            Difficult currentDifficult = (Difficult)((int)MapManager.Instance.Difficult + 1);

            mapSO = MapManager.Instance.MapSOCurrent;

            LoadGame(mapSO, currentDifficult);

            MapManager.Instance.Difficult = currentDifficult;

            Debug.Log("Map: " + mapSO + "-- Difficult: "+ currentDifficult);
        }

        public void LoadMap()
        {
            UIManager.Instance.DetiveGameUI();


            MapManager.Instance.SetCurrentMapSO(mapSO); // Updated to use new method
            MapManager.Instance.LoadMap();


            LoadGame(mapSO, MapManager.Instance.Difficult);
        }
        private void LoadGame(MapSO mapSO, Difficult difficult)
        {
            LevelSettingsChanged?.Invoke(MapManager.Instance.LoadCurrentLevelSettings());

            MapCtrl mapCtrl = MapManager.Instance.CurrentMap.GetComponent<MapCtrl>();

            if (mapSO == null)
            {
                Debug.Log("No Haven't MapSO");
                return;
            }
            //Difficult difficult = MapManager.Instance.Difficult;
            PortalSpawnManager.Instance.Difficult = difficult;
            PortalSpawnManager.Instance.MapSO = mapSO;

            mapCtrl.UIInGame.ListCardTowerData.InstantiateObjectsFromData();
            mapCtrl.UIInGame.UIWinGameController.MapDifficulty = mapSO.GetMapDifficult(difficult);
            mapCtrl.UIInGame.UILevelStarConditionCtrl.ActiveLevelConditionUI(true);
            mapCtrl.UIInGame.UILevelStarConditionCtrl.UpdateUIWithLevelSettings(GameManager.Instance.CurrentLevelSettings);

            //Event 
            LevelUIManager.Instance.mapbtnGameObjects.Clear();
        }
        public void SetMapSOFromLevelInfo(MapSO mapSO)
        {
            this.mapSO = mapSO;
        }

        public void GetCurrentMapSO()
        {
            // This method can be implemented to return or do something with the current MapSO.
        }
    }
}
