using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;


public class AbilitySumonHorn : AbilitySummon
{
    [SerializeField] protected float timeSpawnFisrt;
    public float TimeSpawnFisrt => timeSpawnFisrt;
    public EnemyType[] enemyTypes;
    protected override bool CheckTypeAbility()
    {
        return false;
    }

    protected override void Summoning()
    {
        if (minionCount >= minionLimit)
        {
            Debug.Log("Return Sumoning");

            return;
        }
        SetNameSpawn();
        Transform newPoint = null; // Khởi tạo newPoint

        if (this.abilities.AbilityCtrl.Abilities.AbilityCtrl is HornSpawnerCtrl hornSpawnerCtrl)
        {
            if (hornSpawnerCtrl.SpawnPoints is HornSpawnPoints spawnPoints)
            {
                newPoint = spawnPoints.GetPoints(hornSpawnerCtrl.ObjectCtrl.ObjLand.LandIndex);

                Debug.Log(newPoint + "Land: " + hornSpawnerCtrl.ObjectCtrl.ObjLand.LandIndex);

                if (newPoint != null)
                {
                    LandIndexScript landIndexScript = newPoint.GetComponentInChildren<LandIndexScript>();
                    if (landIndexScript != null)
                    {
                        Debug.Log("Set Land Index: " + landIndexScript.LandIndex);

                        landIndex = landIndexScript.LandIndex;
                    }

                    Summon(newPoint);
                    minionCount++;
                    this.Active();

                    spawnPoints.SwapPosition();
                }
                else
                {
                    Debug.LogWarning("newPoint is null, unable to summon.");
                }
            }
        }
    }
    public void EnableObject()
    {
        this.delay = timeSpawnFisrt;
        this.nameEnemyandCount = ListNameAndCountEnemy();
        this.minionLimit = SumEnemy();
        this.minionCount = 0;
        this.checkALLEnemyDead = false;
    }
    public void DisableObject()
    {
        this.Active();

    }
    private List<EnemyNameAndCount> ListNameAndCountEnemy()
    {
        List<EnemyNameAndCount> enemyNameAndCount = new List<EnemyNameAndCount>();

        foreach (EnemyType enemyType in enemyTypes)
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
    private int SumEnemy()
    {
        int totalEnemyCount = 0;

        foreach (EnemyType enemyType in enemyTypes)
        {
            totalEnemyCount += enemyType.countEnemy;
        }

        return totalEnemyCount;
    }
}
