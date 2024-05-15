using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory
{
    public static Inventory instance;
    public static Inventory Instance => instance;
    public static Dictionary<Item,int> id_count_item = new Dictionary<Item, int>();
    public void AddOrChangeQuantity(Item item , int quantity)
    {
        if (id_count_item.ContainsKey(item))
        {
            id_count_item[item] += quantity;
        }else
        id_count_item.Add(item, quantity);
    }

    public void RemoveOrChangeQuantity(Item item, int quantity)
    {
        if (id_count_item.ContainsKey(item))
        {
            id_count_item[item] -= quantity;
            if (id_count_item[item] <= 0) id_count_item[item] = 0;
        }
        else
            id_count_item.Add(item, quantity);
    }
}
