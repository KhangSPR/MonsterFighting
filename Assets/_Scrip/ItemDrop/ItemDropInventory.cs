using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropInventory : ItemDropCtrl
{
    [SerializeField] InventoryItem inventoryItem;
    public InventoryItem InventoryItem { get { return inventoryItem; } set { inventoryItem = value; } }

    protected override void OnReceiverItem()
    {
        CostManager.Instance.ReceiverItemInventory(InventoryItem);
    }
}
