using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SpinItem", order = 5)]
public class SpinLevelItemSO : ScriptableObject
{
    public int Level;
    public List<UIGameDataMap.Resources> ResourceItems;
    public List<InventoryItem> InventoryItems;
}