using System;
using TMPro;
using UnityEngine;


// what player is buying
[System.Serializable]
public enum ShopItemType
{  
    Craft,
    Medicine,
    Skill,
    Item,
    Ruby,
    Watch
}

// type of currency used to purchase
[System.Serializable]
public enum CurrencyType
{
    EnemyStone,
    EnemyBoss,
    Badge,
    Energy,
    Ruby,
    Watch,
    USD,
    XP
}

[CreateAssetMenu(fileName = "Assets/Resources/GameData/ShopItems/ShopItemGameData", menuName = "GameData/ShopItem", order = 1)]
public class ShopItemSO : ScriptableObject
{
    public string ID;
    public string itemName;

    public Sprite sprite;

    // FREE if equal to 0; cost amount in CostInCurrencyType below
    public float cost;

    // UI shows tag if value larger than 0 (percentage off)
    public uint discount;

    // if not empty, UI shows a banner with this text
    //public string promoBannerText;

    // how many potions/coins this item gives the player upon purchase
    public uint contentValue;
    public uint maxValue;
    public uint quantityContent;
    public ShopItemType contentType;

    //Watch
    public float RestoreInterval;


    // SC (gold) costs HC (gems); HC (gems) costs real USD; HealthPotion costs SC (gold); LevelUpPotion costs HC (gems)
    public CurrencyType CostInCurrencyType
    {
        get
        {
            switch (contentType)
            {
                case ShopItemType.Craft:
                case ShopItemType.Medicine:
                case ShopItemType.Skill:
                    return CurrencyType.EnemyStone;
                case ShopItemType.Item:
                    return CurrencyType.Ruby;
                case ShopItemType.Watch:
                    return CurrencyType.Watch;
                case ShopItemType.Ruby:
                    return CurrencyType.USD;
                default:
                    return CurrencyType.EnemyStone;
            }
        }
    }

}
