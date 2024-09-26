using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDifficultMap : MonoBehaviour
{
    [SerializeField] Difficult difficult;
    public Difficult Difficult => Difficult;
    [SerializeField] MapSO mapSO;

    MapDifficulty mapDifficulty;

    [SerializeField]
    Button btn;
    public Button Button { get { return btn; } set { btn = value; } }



    ChangeDifficultMapInfos changeDifficultMapInfos;
    public void SetMapSO(MapSO mapSO)
    {
        this.mapSO = mapSO;
    }
    private void Start()
    {
        changeDifficultMapInfos = transform.parent.GetComponent<ChangeDifficultMapInfos>();
        //mapSO = MapManager.Instance.MapSOCurrent;
        //btn = transform.GetComponent<Button>();

        btn.onClick.AddListener(DoChange);
    }
    private void DoChange()
    {
        if (mapSO != null)
        {
            changeDifficultMapInfos.SetHolderPVP(difficult);
            mapDifficulty = mapSO.GetMapDifficult(difficult);
            LevelUIManager.Instance.LevelInfo.SetLevelDataDifficulty(mapSO, mapDifficulty);

            Debug.Log("Set Difficult");
            MapManager.Instance.Difficult = mapDifficulty.difficult;
        }
    }
}
