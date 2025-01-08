using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stone Object", menuName = "Inventory System/Items/Craft")]
public class ItemCraftObject : ItemObject
{
    public void Awake()
    {
        type = InventoryType.ItemCraft;
    }
}
