using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    ItemObject itemObject;
    public ItemObject ItemObject { get { return itemObject; } set { itemObject = value; } }
    void Start()
    {
        if(itemObject!=null)
        {
            Tooltip_Item.AddTooltip(transform, itemObject);
        }
    }
}
