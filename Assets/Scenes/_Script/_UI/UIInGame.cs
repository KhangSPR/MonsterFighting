using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] UILoseGameController uiloseGameController;
    public UILoseGameController UILoseGameController => uiloseGameController;

    [SerializeField] UIWinGameController uIWinGameController;
    public UIWinGameController UIWinGameController => uIWinGameController;


    [Header("List Card Data")]
    [SerializeField] ListCardCharacterData listcardTowerData;
    public ListCardCharacterData ListCardTowerData => listcardTowerData;
}
