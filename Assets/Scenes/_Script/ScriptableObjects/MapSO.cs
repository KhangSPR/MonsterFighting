using System;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;


namespace UIGameDataMap
{
    [Serializable]
    public class Resources
    {
        public Item item;
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
        public Difficult difficult;

        public Portals[] portals;
        public Resources[] Reward;

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
        public float[] TimeSpawnPortal(MapSO mapSO)
        {
            if (mapSO == null || mapSO.portals == null) return null;

            List<float> spawnTimes = new List<float>();

            foreach (var portal in mapSO.portals)
            {
                spawnTimes.AddRange(portal.spawnTimeInSeconds);
            }

            return spawnTimes.ToArray();
        }
        public Portals[] PortalsSpawn(MapSO mapSO)
        {
            if (mapSO == null || mapSO.portals == null) return null;

            List<Portals> PortalsSpawn = new List<Portals>();

            foreach (var portal in mapSO.portals)
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
            if (mapSO == null || mapSO.portals == null) return 0;

            int SumEnemy = 0;

            for (int i = 0; i < mapSO.portals.Length; i++)
            {
                for (int j = 0; j < mapSO.portals[i].enemyTypes.Length; j++)
                {
                    SumEnemy += mapSO.portals[i].enemyTypes[j].countEnemy * mapSO.portals[i].Count;
                }
            }

            return SumEnemy;
        }
        //public Portals[] Portals(Map)
    }
}