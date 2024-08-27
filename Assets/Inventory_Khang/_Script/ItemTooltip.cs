using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    ItemObject itemObject;
    public ItemObject ItemObject { get { return itemObject; } set { itemObject = value; } }

    ItemReward itemReward;
    public ItemReward ItemReward { get { return itemReward; } set { itemReward = value; } }
    void Start()
    {
        if(itemObject!=null)
        {
            Tooltip_Item.AddTooltip(transform, itemObject, null);
        }
        if(itemReward!=null)
        {
            Tooltip_Item.AddTooltip(transform, null, itemReward);
        }
    }
}
