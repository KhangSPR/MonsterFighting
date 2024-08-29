using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDropDisplay : ItemDropCtrl
{
    private ItemDropType itemDropType;
    public ItemDropType ItemDropType { get { return itemDropType; } set { itemDropType = value; } }
    protected int countItem;
    public int CountItem { get { return countItem; } set { countItem = value; } }
    protected override void OnReceiverItem()
    {
        CostManager.Instance.ReceiverItemDisplay(ItemDropType, countItem);
    }
}
