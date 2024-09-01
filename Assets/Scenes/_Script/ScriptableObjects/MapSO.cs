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
        public float DelaySpawnFirstEnemy;
        public float[] spawnTimeInSeconds;

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
        LevelUp,
        Gem,
        Equipment,
    }

    public enum MapType
    {
        Grass,
        Lava,
        Desert,
        Snow,
        Ice,
        Stone
    }

    [Serializable]
    public class MapDifficulty
    {
        [Range(0, 3)] public int stars;
        [Space]
        [ReadOnlyInspector]
        public Difficult difficult;
        public StarsCondition starsCondition;
        public bool isReceivedReWard = false;
        public Portals[] portals;
        public Resources[] Reward;

    }

    [CreateAssetMenu(fileName = "NewLevelMapSO", menuName = "Map/LevelMapSO")]
    public class MapSO : ScriptableObject
    {
        public int id;
        public bool boss;
        public string mapZone;
        public MapType mapType;
        public bool Unlocked;

        [Header("MapDifficulty")]
        public MapDifficulty[] DifficultyMap = new MapDifficulty[3];

        private void Awake()
        {
            InitializeDifficultyMap();
        }

        private void InitializeDifficultyMap()
        {
            if (DifficultyMap == null || DifficultyMap.Length != 3)
            {
                DifficultyMap = new MapDifficulty[3];
            }

            for (int i = 0; i < 3; i++)
            {
                if (DifficultyMap[i] == null)
                {
                    DifficultyMap[i] = new MapDifficulty
                    {
                        difficult = (Difficult)i,
                        stars = 0,
                        Reward = new Resources[0],
                        isReceivedReWard = false,
                        portals = new Portals[0]
                    };
                }
            }
        }

        public MapDifficulty GetMapDifficult(Difficult difficult)
        {
            return DifficultyMap[(int)difficult];
        }

        public void SetStarsCount(Difficult difficult, int starsCount)
        {
            DifficultyMap[(int)difficult].stars = starsCount;
        }
        public int SumStarsMapDifficult(MapDifficulty[] DifficultyMap)
        {
            int sum = 0;
            foreach(MapDifficulty difficulty in DifficultyMap)
            {
                sum += difficulty.stars;
            }
            return sum;
        }
        public StarsCondition GetStarsCondition(Difficult difficult)
        {
            return DifficultyMap[(int)difficult].starsCondition;
        }

        public Color GetColorForRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => Color.white,
                Rarity.Rare => new Color(0.164f, 1f, 0.898f),
                Rarity.Epic => new Color(255f / 255f, 18f / 255f, 128f / 255f),
                Rarity.Legendary => Color.yellow,
                _ => Color.gray
            };
        }

        public int GetIndexRarity(RarityPortal rarity)
        {
            return (int)rarity;
        }

        public Color GetColorForRarityPortal(RarityPortal rarity)
        {
            return rarity switch
            {
                RarityPortal.Common => Color.white,
                RarityPortal.Rare => new Color(0.164f, 1f, 0.898f),
                RarityPortal.Epic => new Color(255f / 255f, 18f / 255f, 128f / 255f),
                RarityPortal.Legendary => Color.yellow,
                _ => Color.gray
            };
        }

        public Portals[] GetPortals(Difficult difficult)
        {
            return DifficultyMap[(int)difficult]?.portals;
        }

        public float[] TimeSpawnPortal(Difficult difficult)
        {
            var portals = GetPortals(difficult);
            if (portals == null || portals.Length == 0) return null;

            var spawnTimes = new List<float>();
            foreach (var portal in portals)
            {
                spawnTimes.AddRange(portal.spawnTimeInSeconds);
            }

            return spawnTimes.ToArray();
        }

        public Portals[] PortalsSpawn(Difficult difficult)
        {
            var portals = GetPortals(difficult);
            if (portals == null) return null;

            var portalsSpawn = new List<Portals>();
            foreach (var portal in portals)
            {
                for (int i = 0; i < portal.Count; i++)
                {
                    portalsSpawn.Add(portal);
                }
            }

            return portalsSpawn.ToArray();
        }

        public int SumEnemySpawnPortal(Difficult difficult)
        {
            var portals = GetPortals(difficult);
            if (portals == null) return 0;

            int sumEnemy = 0;
            foreach (var portal in portals)
            {
                foreach (var enemyType in portal.enemyTypes)
                {
                    sumEnemy += enemyType.countEnemy * portal.Count;
                }
            }

            return sumEnemy;
        }
    }
}
