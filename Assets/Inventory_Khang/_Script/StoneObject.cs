using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stone Object", menuName = "Inventory System/Items/Stone")]
public class StoneObject : ItemObject
{
    public Medicine poisonType;
    public int countGame;
    public float percent;


    public void Awake()
    {
        type = InventoryType.Stone;
    }
}
