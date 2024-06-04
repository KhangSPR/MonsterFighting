using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;

public class ChangeDifficultMapInfo : MonoBehaviour
{
    [SerializeField] Difficult difficult;
    [SerializeField] MapSO mapSO;

    private void Awake()
    {
        
    }
    public void DoChange()
    {
        mapSO = GameDataManager.Instance.currentMapSO;
        if (mapSO != null)
        {
            GameObject infoMap = GameObject.Find("MapWorld");
            Debug.Log(infoMap);
            mapSO.difficult = difficult;
            infoMap.transform.Find("Maps").Find("Map_Scroll").Find("InforMap").GetComponent<LevelInfo>().SetLevelData(mapSO);

        }
    }
}
