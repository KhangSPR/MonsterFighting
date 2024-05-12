using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIGameDataManager
{
    [Serializable]
    public class BaseStat
    {
        [SerializeField]
        public float baseStatValue;

        [SerializeField]
        public AnimationCurve baseStatModifier;
    }

    [CreateAssetMenu(fileName = "CardCharcter New", menuName = "Card/Character New")]
    public class CardStat : ScriptableObject
    {
        public BaseStat Attack;
        public BaseStat Life;
        public BaseStat AttackSpeed;
        public BaseStat SpecialAttack;
    }
}