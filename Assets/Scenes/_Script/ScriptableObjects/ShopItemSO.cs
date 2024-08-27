using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// what player is buying
[System.Serializable]
public enum ShopItemType
{
    // soft currency (in-game)
    CardCharacter,
    CardMachine,
    CardGuard,
    Gold,
    // hard currency (buy with real money)
    Ruby,

    // used in gameplay
    XpLv1,

    XpLv2,

    XpLv3,

    XpLv4,

    CastleGold,
    CastleRuby,

}

// type of currency used to purchase
[System.Serializable]
public enum CurrencyType
{
    Gold,

    USD,
    EnemyStone,
    EnemyBoss

}

[CreateAssetMenu(fileName = "Assets/Resources/GameData/ShopItems/ShopItemGameData", menuName = "UIToolkitDemo/ShopItem", order = 4)]
public class ShopItemSO : ScriptableObject
{
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
    public ShopItemType contentType;

    // SC (gold) costs HC (gems); HC (gems) costs real USD; HealthPotion costs SC (gold); LevelUpPotion costs HC (gems)
    public CurrencyType CostInCurrencyType
    {
        get
        {
            switch (contentType)
            {
                case (ShopItemType.XpLv1):
                    return CurrencyType.Gold;

                case (ShopItemType.XpLv2):
                    return CurrencyType.Gold;
                    
                case (ShopItemType.XpLv3):
                    return CurrencyType.Gold;

                case (ShopItemType.XpLv4):
                    return CurrencyType.Gold;

                case (ShopItemType.Gold):
                    return CurrencyType.EnemyStone;
                case (ShopItemType.Ruby):
                    return CurrencyType.USD;
                default:
                    return CurrencyType.Gold;
            }
        }
    }
}
