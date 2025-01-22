using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/GameData/ItemSO/ItemSOData", menuName = "GameData/ItemReward", order = 3)]
public class ItemReward : ScriptableObject
{
    public string ID;
    [SerializeField] private string itemName;
    [TextArea(15, 20)]
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite image;
    [SerializeField] private CurrencyType currencyType;
    public ItemRarity itemRarity;


    public string ItemName { get { return itemName; } }
    public string ItemDescription { get { return itemDescription; } }

    public Sprite Image { get { return image; } }
    public CurrencyType CurrencyType { get {  return currencyType; } }
}