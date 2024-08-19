using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable , CreateAssetMenu]
public class ItemReward : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite image;


    public string ItemName { get { return itemName; } }
    public string ItemDescription { get { return itemDescription; } }

    public ItemType Type { get { return type; } }
    public Sprite Image { get { return image; } }
}
public enum ItemType
{
    Medicine, Gem, Item, blabla
}