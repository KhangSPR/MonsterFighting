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
    public struct AttackTypeIcon
    {
        public Sprite icon;
        public AttackCategory attackType;
    }
    // returns an icon matching 
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Icons", menuName = "GameData/Icons", order = 0)]
    public class GameIconsSO : ScriptableObject
    {
        public List<CurrencyIcon> currencyIcons;
        public List<AttackTypeIcon> attackTypeIcons;
        public Sprite GetCurrencyIcon(CurrencyType currencyType)
        {
            if (currencyIcons == null || currencyIcons.Count == 0)
                return null;

            CurrencyIcon match = currencyIcons.Find(x => x.currencyType == currencyType);
            return match.icon;
        }
        // get attackTypeIcon
        public Sprite GetAttackTypeIcon(AttackCategory attackType)
        {
            if (attackTypeIcons == null || attackTypeIcons.Count == 0)
                return null;

            AttackTypeIcon match = attackTypeIcons.Find(x => x.attackType == attackType);
            return match.icon;
        }

    }
}