using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIGameDataManager
{

    [Serializable]
    public struct CurrencyIcon
    {
        public Sprite icon;
        public CurrencyType currencyType;
    }

    [Serializable]
    public struct ShopItemTypeIcon
    {
        public Sprite icon;
        public ShopItemType shopItemType;
    }
    [Serializable]
    public struct RarityIcon
    {
        public Sprite icon;
        public RarityCard rarity;
    }

    [Serializable]
    public struct AttackTypeIcon
    {
        public Sprite icon;
        public AttackType attackType;
    }

    //[Serializable]
    //public struct FrameIcon
    //{
    //    public Sprite icon;
    //    public FrameType frameType;
    //}
    [Serializable]
    public struct StarIcon
    {
        public Sprite icon;
        public RarityCard rarityType;
    }

    // returns an icon matching 
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Icons", menuName = "UIToolkitDemo/Icons", order = 10)]
    public class GameIconsSO : ScriptableObject
    {
        public List<CurrencyIcon> currencyIcons;
        public List<ShopItemTypeIcon> shopItemTypeIcons;
        public List<RarityIcon> rarityIcons;
        public List<AttackTypeIcon> attackTypeIcons;
        //public List<FrameIcon> frameIcons;
        public List<StarIcon> starIcons;
        //public Sprite GetFrameIcon(FrameType currencyType)
        //{
        //    if (frameIcons == null || frameIcons.Count == 0)
        //        return null;

        //    FrameIcon match = frameIcons.Find(x => x.frameType == currencyType);
        //    return match.icon;
        //}
        public Sprite GetStarIcon(RarityCard currencyType)
        {
            if (starIcons == null || starIcons.Count == 0)
                return null;

            StarIcon match = starIcons.Find(x => x.rarityType == currencyType);
            return match.icon;
        }

        public Sprite GetCurrencyIcon(CurrencyType currencyType)
        {
            if (currencyIcons == null || currencyIcons.Count == 0)
                return null;

            CurrencyIcon match = currencyIcons.Find(x => x.currencyType == currencyType);
            return match.icon;
        }

        public Sprite GetShopTypeIcon(ShopItemType shopItemType)
        {
            if (shopItemTypeIcons == null || shopItemTypeIcons.Count == 0)
                return null;

            ShopItemTypeIcon match = shopItemTypeIcons.Find(x => x.shopItemType == shopItemType);
            return match.icon;
        }
        // get Character icon
        public Sprite GetRarityIcon(RarityCard rarity)
        {
            if (rarityIcons == null || rarityIcons.Count == 0)
                return null;

            RarityIcon match = rarityIcons.Find(x => x.rarity == rarity);
            return match.icon;
        }
        // get attackTypeIcon
        public Sprite GetAttackTypeIcon(AttackType attackType)
        {
            if (attackTypeIcons == null || attackTypeIcons.Count == 0)
                return null;

            AttackTypeIcon match = attackTypeIcons.Find(x => x.attackType == attackType);
            return match.icon;
        }

    }
    #region Comment
    //[Serializable]
    //public struct CharacterClassIcon
    //{
    //    public Sprite icon;
    //    public CharacterClass characterClass;
    //}
    //public List<CharacterClassIcon> characterClassIcons;
    // get rarity icon
    //public Sprite GetCharacterClassIcon(CharacterClass charClass)
    //{
    //    if (characterClassIcons == null || characterClassIcons.Count == 0)
    //        return null;

    //    CharacterClassIcon match = characterClassIcons.Find(x => x.characterClass == charClass);
    //    return match.icon;
    //}
    #endregion
}