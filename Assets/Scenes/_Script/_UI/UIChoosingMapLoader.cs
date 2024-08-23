using UnityEngine;

namespace UIGameDataMap
{
    public class UIChoosingMapLoader : SaiMonoBehaviour
    {
        private MapSO mapSO;
        public void onClickExit()
        {
            Application.Quit();
        }

        public void LoadNextMap() // Apply only for Grass area
        {
            MapManager.Instance.LoadNextMap();
            //MapManager.Instance.ReloadMap();

            mapSO = MapManager.Instance.MapSOCurrent;

            Debug.Log(mapSO);

            LoadGame(mapSO);
        }

        public void ReloadMap()
        {
            MapManager.Instance.ReloadMap();
            //UIManager.Instance.DetiveGameUI();

            mapSO = MapManager.Instance.MapSOCurrent;


            LoadGame(mapSO);
        }

        public void LoadMap()
        {
            UIManager.Instance.DetiveGameUI();


            MapManager.Instance.SetCurrentMapSO(mapSO); // Updated to use new method
            MapManager.Instance.LoadMap();


            LoadGame(mapSO);
        }
        private void LoadGame(MapSO mapSO)
        {

            MapCtrl mapCtrl = MapManager.Instance.CurrentMap.GetComponent<MapCtrl>();

            if (mapSO == null)
            {
                Debug.Log("No Haven't MapSO");
                return;
            }
            Difficult difficult = MapManager.Instance.Difficult;
            PortalSpawnManager.Instance.Difficult = difficult;
            PortalSpawnManager.Instance.MapSO = mapSO;

            mapCtrl.UIInGame.ListCardTowerData.InstantiateObjectsFromData();
            mapCtrl.UIInGame.UIWinGameController.MapDifficulty = mapSO.GetMapDifficult(difficult);

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
