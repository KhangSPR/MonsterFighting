using System;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class FullMapController : MonoBehaviour
{
    public bool isMapOpening = false;
    public List<GameObject> map_icons;
    public List<GameObject> maps; 
    public int mapOpeningIndex;

    public void ToggleOpen()
    {
        isMapOpening = !isMapOpening;
    }
    //Nguyen

    private void OnEnable()
    {
        //FirstTimeFullStarsUI();
        //DifficultHolderController();
    }
    //public void FirstTimeFullStarsUI()
    //{
    //    if (LevelUIManager.Instance.mapbtnGameObjects == null || LevelUIManager.Instance.mapbtnGameObjects.Count == 0) { Debug.Log("map btn object null"); Invoke(nameof(FirstTimeFullStarsUI), 0.1f); return; }
    //    var listBtn = LevelUIManager.Instance.mapbtnGameObjects;
    //    print("list btn count:" + listBtn.Count);
    //    foreach (var btn in listBtn)
    //    {
    //        var MapDataSO = btn.GetComponent<LevelButton>().GetMapDataSO();
    //        if (MapDataSO != null)
    //        {
    //            Debug.Log("Map Data SO is not null");
    //            if (MapDataSO.starsEasy == 3) GameDataManager.Instance.FirstTimeGetFullStars = true;
    //            DifficultHolder.SetActive(GameDataManager.Instance.FirstTimeGetFullStars);
    //        }
    //    }
    //}
    //public void DifficultHolderController()
    //{
    //    DifficultHolder.transform.GetChild(1).gameObject.SetActive(GameDataManager.Instance.currentMapSO.starsEasy == 3);
    //    DifficultHolder.transform.GetChild(2).gameObject.SetActive(GameDataManager.Instance.currentMapSO.starsNormal == 3);
    //}

    // Nguyen


    public void Awake()
    {
        // Show All Area
        foreach(GameObject map in map_icons)
        {
            Image image = map.GetComponent<Image>();
            if (image != null)
            {
                HideArea(image);
            }
        }
        
    }

    public void SetMapOpeningIndex(GameObject mapInput)
    {
        foreach(GameObject map in maps)
        {
            if (mapInput == map)
            {
                mapOpeningIndex = maps.IndexOf(map);
                Debug.Log("map Opening Index = " + mapOpeningIndex);
            }
        }
    }

    public void NextMap()
    {
        
    }

    public void PreviousMap()
    {
        
    }

    public void OpenMap(int index)
    {
        ToggleOpen();
        Transform maps = transform.Find("Maps");
        maps.gameObject.SetActive(true);
        Transform map_scroll = maps.Find("Map_Scroll");
        map_scroll.gameObject.SetActive(true);

        SwipeController S_controller = map_scroll.GetComponent<SwipeController>();
        Debug.Log(S_controller);
        S_controller.currentPage = index;
        S_controller.MovePage();
    }

    public void ShowArea(Image image)
    {
        //Debug.Log(image);
        foreach(GameObject o in map_icons)
        {
            Image imageO = o.GetComponent<Image>();
            if(image == imageO)
            {
                //Debug.Log("image equal");
            }
        }
        image.color = Color.white;
    }

    public void HideArea(Image image)
    {
        image.color = Color.white * 0;
    }

    public int GetIndex(Transform transform)
    {
        foreach (GameObject child in maps)
        {
            if(transform.name == child.name)
            {
                return maps.IndexOf(child);

            }
        }

        throw new IndexOutOfRangeException("Không tồn tại map , hoặc map chưa được tạo ra");
    }
}
