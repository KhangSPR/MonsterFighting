using System;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class FullMapController : MonoBehaviour
{
    public bool isMapOpening = false;
    public List<GameObject> maps; 
    public int mapOpeningIndex;

    public void ToggleOpen()
    {
        isMapOpening = !isMapOpening;
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

    public void HideArea(Image image)
    {
        image.color = Color.white * 0;
    }
    //public void NextMap()
    //{

    //}

    //public void PreviousMap()
    //{

    //}
}
