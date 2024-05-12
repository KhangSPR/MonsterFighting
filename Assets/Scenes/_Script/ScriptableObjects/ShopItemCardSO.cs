using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;


[CreateAssetMenu(fileName = "Assets/Resources/GameData/ShopItemCards/ShopItemGameData", menuName = "UIToolkitDemo/ShopItemCard", order = 5)]
public class ShopItemCardSO : ScriptableObject
{
    public string itemName;
    // FREE if equal to 0; cost amount in CostInCurrencyType below
    public float cost;

    // UI shows tag if value larger than 0 (percentage off)
    public uint discount;

    public ShopItemType contentType;

    public bool hasBuy;

    public CardComponent cardComponent;

    // SC (gold) costs HC (gems); HC (gems) costs real USD; HealthPotion costs SC (gold); LevelUpPotion costs HC (gems)
    public CurrencyType CostInCurrencyType
    {
        get
        {
            switch (contentType)
            {
                case (ShopItemType.CardCharacter):
                    return CurrencyType.Gold;

                case (ShopItemType.CardMachine):
                    return CurrencyType.Gold;

                case (ShopItemType.CardGuard):
                    return CurrencyType.Gold;
                default:
                    return CurrencyType.Gold;
            }
        }
    }
    public ShopItemType GetShopItemType()
    {
        return contentType;
    }
}
