using UnityEngine;

namespace UIGameDataMap
{
    public class UIChoosingMapLoader : SaiMonoBehaviour
    {
        public MapSO mapSO;

        public void onClickExit()
        {
            Application.Quit();
        }

        public void LoadNextMap() // Apply only for Grass area
        {
            MapManager.Instance.LoadNextMap();
            ReloadMap();
        }

        public void ReloadMap()
        {
            mapSO = MapManager.Instance.MapSOCurrent;
            MapManager.Instance.ReloadMap();
            UIManager.Instance.DetiveGameUI();

            MapCtrl mapCtrl = MapManager.Instance.CurrentMap.GetComponent<MapCtrl>();
            mapCtrl.PortalSpawnerCtrl.MapSO = mapSO;
            mapCtrl.UIInGame.ListCardTowerData.InstantiateObjectsFromData();

            LevelUIManager.Instance.mapbtnGameObjects.Clear();
        }

        public void loadMapButtonDetail()
        {
            MapManager.Instance.SetCurrentMapSO(mapSO); // Updated to use new method
            MapManager.Instance.LoadMap();

            UIManager.Instance.DetiveGameUI();

            MapCtrl mapCtrl = MapManager.Instance.CurrentMap.GetComponent<MapCtrl>();
            mapCtrl.PortalSpawnerCtrl.MapSO = mapSO;
            mapCtrl.UIInGame.ListCardTowerData.InstantiateObjectsFromData();

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
