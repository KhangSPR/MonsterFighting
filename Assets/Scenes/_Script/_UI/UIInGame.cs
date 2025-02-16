using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] UILoseGameCtrl uiloseGameController;
    public UILoseGameCtrl UILoseGameController => uiloseGameController;

    [SerializeField] UIWinGameCtrl uIWinGameController;
    public UIWinGameCtrl UIWinGameController => uIWinGameController;

    [SerializeField] UILevelStarConditionCtrl uILevelStarConditionCtrl;
    public UILevelStarConditionCtrl UILevelStarConditionCtrl => uILevelStarConditionCtrl;

    [Space]
    [Space]
    [Space]
    [Header("Settings")]
    [SerializeField] SettingUI settingUI;
    public SettingUI SettingUI => settingUI;



    [Header("List Card Data")]
    [SerializeField] ListCardCharacterData listcardTowerData;
    public ListCardCharacterData ListCardTowerData => listcardTowerData;
}
