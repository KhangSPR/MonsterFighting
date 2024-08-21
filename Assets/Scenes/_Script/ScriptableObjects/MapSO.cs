using System;
using System.Collections.Generic;
using UnityEngine;


namespace UIGameDataMap
{
    [Serializable]



    public class Resources
    {
        public ItemReward item;
        public int Count;

    }
    [Serializable]
    public class Portals
    {
        public RarityPortal rarityPortal;
        public int Count;
        public float DelaySpawnFirtEnemy;
        public float[] spawnTimeInSeconds;

        //On Mouse
        [Header("On Mouse")]
        public bool hasBoss;
        public EnemyType[] enemyTypes;
    }
    [Serializable]
    public class EnemyType
    {
        public Rarity rarity;
        public string name;
        public int countEnemy;

        [Header("Timer")]
        public float timerMin;
        public float timerMax;
        public SkillType[] skillType;
        public Sprite Sprite;
    }
    [Serializable]
    public class SkillType
    {
        public int id;
        public string skill;
    }
    public enum RarityPortal
    {
        Common,   // Phổ biến
        Rare,     // Hiếm
        Epic,     // Kỳ diệu
        Legendary // Huyền bí
    }
    public enum Rarity
    {
        Common,   // Phổ biến
        Rare,     // Hiếm
        Epic,     // Kỳ diệu
        Legendary // Huyền bí
    }
    public enum ResourceType
    {
        LeveUp,
        Gem,
        Equipment,
    }
    public enum MapType
    {
        Desert,
        Lava,
        Snow,
        Ice,
        Grass,
        Stone
    }
    [CreateAssetMenu(fileName = "NewLevelMapSO", menuName = "Map/LevelMapSO")]
    public class MapSO : ScriptableObject
    {
        public int id;
        public bool boss;
        public string mapZone;
        public MapType mapType;
        [Space]
        public Difficult difficult;
        [Space]
        [Space]
        [Space]
        [Space]
        [Header("EasyMap")]
        [Range(0, 3)] public int starsEasy;
        public StarsCondition starsConditionEasy;
        public Resources[] RewardEasy;
        public bool isOneTimeRewardGotEasy = false;
        public Resources[] OneTimeRewardEasy;
        public Portals[] portalsEasy;
        [Space]
        [Space]
        [Space]
        [Space]
        [Header("NormalMap")]
        [Range(0, 3)] public int starsNormal;
        public StarsCondition starsConditionNormal;
        public Resources[] RewardNormal;
        public bool isOneTimeRewardGotNormal = false;
        public Resources[] OneTimeRewardNormal;
        public Portals[] portalsNormal;
        [Space]
        [Space]
        [Space]
        [Space]
        [Header("HardMap")]
        [Range(0, 3)] public int starsHard;
        public StarsCondition starsConditionHard;
        public Resources[] RewardHard;
        public bool isOneTimeRewardGotHard = false;
        public Resources[] OneTimeRewardHard;
        public Portals[] portalsHard;
        //public StarsConditionSO starsCondition;
        [HideInInspector] public int starsDifficult;
        public int GetStarsCount(Difficult difficult)
        {

            switch (difficult)
            {
                case Difficult.Easy: return starsEasy;
                case Difficult.Normal: return starsNormal;
                case Difficult.Hard: return starsHard;
                default: return starsEasy;
            }
        }
        public void SetStarsCount(Difficult difficult, int starsCount)
        {

            switch (difficult)
            {
                case Difficult.Easy: starsEasy = starsCount; break;
                case Difficult.Normal: starsNormal = starsCount; break;
                case Difficult.Hard: starsHard = starsCount; break;
                default: starsEasy = starsCount; break;
            }
        }
        public StarsCondition GetStarsCondition(Difficult difficult)
        {
            //StarsCondition starsCondition = new StarsCondition();
            switch (difficult)
            {
                case Difficult.Easy: return starsConditionEasy;
                case Difficult.Normal: return starsConditionNormal;
                case Difficult.Hard: return starsConditionHard;
                default: return starsConditionEasy;
            }
        }
        public Color GetColorForRarity(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return Color.white; // Màu trắng cho Common
                case Rarity.Rare:
                    return new Color(0.164f, 1f, 0.898f); // Color Code "2AFFE5"
                case Rarity.Epic:
                    return new Color(255f / 255f, 18f / 255f, 128f / 255f);
                case Rarity.Legendary:
                    return Color.yellow; // Màu vàng cho Legendary
                default:
                    return Color.gray; // Mặc định màu xám
            }
        }
        public int GetIndexRarity(RarityPortal rarity)
        {
            switch (rarity)
            {
                case RarityPortal.Common:
                    return 0;
                case RarityPortal.Rare:
                    return 1;
                case RarityPortal.Epic:
                    return 2;
                case RarityPortal.Legendary:
                    return 3;
                default:
                    return 0;
            }
        }
        public Color GetColorForRarityPortal(RarityPortal rarity)
        {
            switch (rarity)
            {
                case RarityPortal.Common:
                    return Color.white; // Màu trắng cho Common
                case RarityPortal.Rare:
                    return new Color(0.164f, 1f, 0.898f); // Color Code "2AFFE5"
                case RarityPortal.Epic:
                    return new Color(255f / 255f, 18f / 255f, 128f / 255f);
                case RarityPortal.Legendary:
                    return Color.yellow; // Màu vàng cho Legendary
                default:
                    return Color.gray; // Mặc định màu xám
            }
        }
        public Portals[] GetPortals(Difficult difficult)
        {
            Portals[] portals = null;
            switch (difficult)
            {
                case Difficult.Easy: { portals = portalsEasy; } break;
                case Difficult.Normal: { portals = portalsNormal; } break;
                case Difficult.Hard: { portals = portalsHard; } break;
            }
            return portals;
        }

        public float[] TimeSpawnPortal(MapSO mapSO)
        {
            Portals[] portals = GetPortals(difficult);
            if (mapSO == null || portals.Length == 0) return null;

            List<float> spawnTimes = new List<float>();

            foreach (var portal in portals)
            {
                spawnTimes.AddRange(portal.spawnTimeInSeconds);
            }

            return spawnTimes.ToArray();
        }
        public Portals[] PortalsSpawn(MapSO mapSO)
        {
            Portals[] portals = GetPortals(difficult);
            if (mapSO == null || portals == null) return null;

            List<Portals> PortalsSpawn = new List<Portals>();

            foreach (var portal in portals)
            {
                for (int i = 0; i < portal.Count; i++)
                {
                    PortalsSpawn.Add(portal);
                }
            }

            return PortalsSpawn.ToArray();
        }

        public int SumEnemySpawnPortal(MapSO mapSO)
        {
            UIGameDataMap.Portals[] portals = GetPortals(difficult);
            if (mapSO == null || portals == null) return 0;

            int SumEnemy = 0;

            for (int i = 0; i < portals.Length; i++)
            {
                for (int j = 0; j < portals[i].enemyTypes.Length; j++)
                {
                    SumEnemy += portals[i].enemyTypes[j].countEnemy * portals[i].Count;
                }
            }

            return SumEnemy;
        }
        //public Portals[] Portals(Map)

    }


}