using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable , CreateAssetMenu]
public class Item :ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite image;
    [SerializeField] private Sprite imageBnW; // black and white


    public string ItemName { get { return itemName; } }
    public string ItemDescription { get { return itemDescription; } }

    public ItemType Type { get { return type; } }
    public Sprite Image { get { return image; } }
    public Sprite ImageBnW { get { return imageBnW; } }
}
public enum ItemType
{
    LevelUp , Gem , blabla
}