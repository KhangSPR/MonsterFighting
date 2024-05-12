using System;
namespace UIGameDataManager
{
    public enum StatKeys
    {
        Attack,
        CriticalHit,
        Life,
        Defense,
        AttackSpeed,
        SpecialAttack
    }
    [Serializable]
    public class Stat
    {
        public StatKeys statKey;
        public float statValue;

        public Stat(StatKeys statKey, float statValue)
        {
            this.statKey = statKey;
            this.statValue = statValue;
        }
    }
}