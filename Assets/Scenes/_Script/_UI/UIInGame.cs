using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    [SerializeField] UIMap uIMap;
    public UIMap UIMap => uIMap;


    [Header("List Card Data")]
    [SerializeField] ListCardCharacterData listcardTowerData;
    public ListCardCharacterData ListCardTowerData => listcardTowerData;
}
