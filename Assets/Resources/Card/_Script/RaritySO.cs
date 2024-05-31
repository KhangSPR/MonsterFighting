using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIGameDataManager
{
    [Serializable]
    public class BaseRarity
    {
        [SerializeField]
        public float baseStatValue;

        [SerializeField]
        public AnimationCurve baseStatModifier;
    }

    [CreateAssetMenu(fileName = "Rarity New", menuName = "Card/Rarity New")]
    public class RaritySO : ScriptableObject
    {
        public BaseRarity Attack;
        public BaseRarity Life;
        public BaseRarity AttackSpeed;
        public BaseRarity SpecialAttack;
    }
}