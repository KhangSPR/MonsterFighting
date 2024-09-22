using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    public class Wave
    {
        public WaveRandom waveRandom;
        public WaveFinal waveFinal;

        public List<EnemyRandom> GetEnemiesRandom(Wave wave)
        {
            return wave.waveRandom.enemyRanDoms.ToList();
        }
        public List<EnemyRandom> GetEnemiesWave(Wave wave)
        {
            return wave.waveFinal.enemyRanDoms.ToList();
        }
        public Portals[] GetPortalsWave(Wave wave)
        {
            return wave.waveFinal.Portals;
        }
        public Portals[] GetPortalsSpawning(Wave wave)
        {
            return wave.waveRandom.Portals;
        }
        public int SumEnemyWave(Wave wave)
        {
            int sumEnemy = 0;
            sumEnemy += SumEnemyRanDom(wave);
            sumEnemy += SumEnemyWaveFinal(wave);

            return sumEnemy;
        }
        public int SumEnemyRanDom(Wave wave)
        {
            int sumEnemy = 0;
            var portals = GetPortalsSpawning(wave);
            foreach (var portal in portals)
            {
                if (portal == null) return 0;

                sumEnemy += portal.SumEnemy(portal);
            }
            var enemys = GetEnemiesRandom(wave);
            foreach (var enemyRandom in enemys)
            {
                if (enemyRandom == null) return 0;

                sumEnemy += enemyRandom.SumEnemy(enemyRandom);
            }
            return sumEnemy;
        }
        public int SumEnemyWaveFinal(Wave wave)
        {
            int sumEnemy = 0;
            var portals = GetPortalsWave(wave);
            foreach (var portal in portals)
            {
                if (portal == null) return 0;

                sumEnemy += portal.SumEnemy(portal);
            }
            var enemys = GetEnemiesWave(wave);
            foreach (var enemyRandom in enemys)
            {
                if (enemyRandom == null) return 0;

                sumEnemy += enemyRandom.SumEnemy(enemyRandom);
            }
            return sumEnemy;
        }
    }
    [Serializable]
    public class WaveRandom
    {
        public EnemyRandom[] enemyRanDoms;
        public Portals[] Portals;
    }
    [Serializable]
    public class WaveFinal
    {
        public EnemyRandom[] enemyRanDoms;
        public Portals[] Portals;
    }
    [Serializable]
    public class Portals
    {
        public RarityPortal rarityPortal;
        public int countPortal;
        public float DelaySpawnFirstEnemy;
        public float[] spawnTimeInSeconds;

        [Header("On Mouse")]
        public bool hasBoss;
        public EnemyType[] enemyTypes;

        public int SumEnemy(Portals portals)
        {
            int totalEnemyCount = 0;

            foreach (EnemyType enemyType in portals.enemyTypes)
            {
                totalEnemyCount += enemyType.countEnemy;
            }

            return totalEnemyCount;
        }
        public List<EnemyNameAndCount> ListNameAndCountEnemy(Portals portals)
        {
            List<EnemyNameAndCount> enemyNameAndCount = new List<EnemyNameAndCount>();

            foreach (EnemyType enemyType in portals.enemyTypes)
            {
                EnemyNameAndCount enemy = new EnemyNameAndCount();
                enemy.name = enemyType.name;
                enemy.max = enemyType.countEnemy;
                enemy.radomMin = enemyType.timerMin;
                enemy.radomMax = enemyType.timerMax;
                enemyNameAndCount.Add(enemy);
            }

            return enemyNameAndCount;
        }
    }
    [Serializable]
    public class EnemyRandom
    {
        public float TimeFirstSpawn;
        public EnemyType[] EnemyType;
        public int SumEnemy(EnemyRandom enemyRandom)
        {
            int totalEnemyCount = 0;

            foreach (EnemyType enemyType in enemyRandom.EnemyType)
            {
                totalEnemyCount += enemyType.countEnemy;
            }

            return totalEnemyCount;
        }
        public List<EnemyNameAndCount> ListNameAndCountEnemy(EnemyRandom enemyRandom)
        {
            List<EnemyNameAndCount> enemyNameAndCount = new List<EnemyNameAndCount>();

            foreach (EnemyType enemyType in enemyRandom.EnemyType)
            {
                EnemyNameAndCount enemy = new EnemyNameAndCount();
                enemy.name = enemyType.name;
                enemy.max = enemyType.countEnemy;
                enemy.radomMin = enemyType.timerMin;
                enemy.radomMax = enemyType.timerMax;
                enemyNameAndCount.Add(enemy);
            }

            return enemyNameAndCount;
        }
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
        public Wave[] Waves;
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
                        Waves = new Wave[0]
                    };
                }
            }
        }

        public MapDifficulty GetMapDifficult(Difficult difficult)
        {
            return DifficultyMap[(int)difficult];
        }
        public MapDifficulty GetMapDifficultUnlock(MapSO mapSO)
        {
            MapDifficulty[] mapDifficulties = mapSO.DifficultyMap;

            foreach (MapDifficulty difficulty in mapDifficulties)
            {
                if (!difficulty.isReceivedReWard)
                {
                    return difficulty;
                }
            }
            return null;
        }
        public void SetStarsCount(Difficult difficult, int starsCount)
        {
            DifficultyMap[(int)difficult].stars = starsCount;
        }
        public int SumStarsMapDifficult(MapDifficulty[] DifficultyMap)
        {
            int sum = 0;
            foreach (MapDifficulty difficulty in DifficultyMap)
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
        #region Wave-Portal-Enemy
        //Get Sum Wave
        public Wave[] GetWaves(Difficult difficult)
        {
            return DifficultyMap[(int)difficult]?.Waves;
        }
        public Portals[] GetPortalsWave(Wave wave)
        {
            return wave.waveFinal.Portals;
        }
        public Portals[] GetPortalsSpawning(Wave wave)
        {
            return wave.waveRandom.Portals;
        }
        public List<EnemyRandom> GetEnemiesRandom(Wave wave)
        {
            return wave.waveRandom.enemyRanDoms.ToList();
        }
        public List<EnemyRandom> GetEnemiesWave(Wave wave)
        {
            return wave.waveFinal.enemyRanDoms.ToList();
        }
        public List<float> TimeSpawnWavePotal(Wave wave)
        {
            var spawnTimes = new List<float>();

            var portals = GetPortalsWave(wave);
            if (portals == null || portals.Length == 0) return null;

            foreach (var portal in portals)
            {
                spawnTimes.AddRange(portal.spawnTimeInSeconds);
            }

            Debug.Log("Da lay");

            return spawnTimes;
        }
        public List<float> TimeSpawningPortal(Wave wave)
        {
            var spawnTimes = new List<float>();

            var portals = GetPortalsSpawning(wave);
            if (portals == null || portals.Length == 0) return null;

            foreach (var portal in portals)
            {
                spawnTimes.AddRange(portal.spawnTimeInSeconds);
            }

            Debug.Log("Da lay");

            return spawnTimes;
        }
        public int SumEnemyAll(Difficult difficult)
        {
            var waves = GetWaves(difficult);

            int sumEnemy = 0;

            foreach (var wave in waves)
            {
                sumEnemy+= wave.SumEnemyRanDom(wave);
                sumEnemy += wave.SumEnemyWaveFinal(wave);
            }
            return sumEnemy;
        }
        public int SumWaveSpawn(Difficult difficult)
        {
            var waves = GetWaves(difficult);
            if (waves == null) return 0;

            int sumWave = 0;
            foreach (var wave in waves)
            {
                sumWave++;
            }

            return sumWave;
        }
        #endregion
    }
}

