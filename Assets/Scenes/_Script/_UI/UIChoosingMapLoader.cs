using System;
using UnityEngine;

namespace UIGameDataMap
{
    public class UIChoosingMapLoader : SaiMonoBehaviour
    {
        private MapSO mapSO;

        public static event Action<LevelSettings> LevelSettingsChanged;

        protected override void OnEnable()
        {
            base.OnEnable();
            EnergyUI.OnClickEnergy += LoadMap;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EnergyUI.OnClickEnergy -= LoadMap;

        }
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
            // Kiểm tra null sớm và thoát hàm nếu không có mapSO
            if (mapSO == null)
            {
                Debug.LogWarning("MapSO is null. Unable to load the game.");
                return;
            }

            // Kích hoạt sự kiện thay đổi cài đặt màn chơi
            LevelSettingsChanged?.Invoke(MapManager.Instance?.LoadCurrentLevelSettings());

            // Thiết lập thông tin cho PortalSpawnManager
            SetUpPortalSpawnManager(mapSO, difficult);

            // Thiết lập và cập nhật UI trong game
            UpdateInGameUI(mapSO, difficult);

            // Clear các button trong LevelUIManager
            LevelUIManager.Instance?.mapbtnGameObjects?.Clear();

            CoroutineManager.Instance.StopAllManagedCoroutines();
        }

        private void SetUpPortalSpawnManager(MapSO mapSO, Difficult difficult)
        {
            WaveSpawnManager.Instance.Difficult = difficult;
            WaveSpawnManager.Instance.MapSO = mapSO;
            WaveSpawnManager.Instance.Wave = mapSO.GetWaves(difficult);
        }

        private void UpdateInGameUI(MapSO mapSO, Difficult difficult)
        {
            var mapCtrl = MapManager.Instance?.CurrentMap?.GetComponent<MapCtrl>();
            if (mapCtrl == null)
            {
                Debug.LogError("Failed to get MapCtrl. UI will not be updated.");
                return;
            }

            // Khởi tạo các đối tượng UI liên quan
            mapCtrl.UIInGame?.ListCardTowerData?.InstantiateObjectsFromData();
            mapCtrl.UIInGame.UIWinGameController.MapDifficulty = mapSO.GetMapDifficult(difficult);
            mapCtrl.UIInGame?.UILevelStarConditionCtrl?.ActiveLevelConditionUI();
            mapCtrl.UIInGame?.UILevelStarConditionCtrl?.UpdateUIWithLevelSettings(GameManager.Instance?.CurrentLevelSettings);
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
