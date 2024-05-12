using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    private void OnEnable()
    {
        LoadGameMap();
        //LevelUIManager.Instance.AutoClickLv();
    }

    private void OnDisable()
    {
        DeloadGameMap();
        LevelUIManager.Instance.mapbtnGameObjects.Clear();
    }

    private void DeloadGameMap()
    {
        var o = transform.Find("LvBtnHolder");

        for (int i = o.childCount - 1; i > 0; i--)
        {
            Destroy(o.GetChild(i).gameObject);
        }

    }

    void LoadGameMap()
    {
        LevelUIManager.Instance.LoadAllLevelByArea(transform.name);

    }
}
