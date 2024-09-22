using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class AbilitySummonEnemy : AbilitySummon
{
    private List<EnemyRandom> enemies = new List<EnemyRandom>();
    public List<EnemyRandom> Enemies => enemies;
    [SerializeField] private int currentListEnemies = 0;
    public int CurrentListEnemies => currentListEnemies;


    protected override void OnEnable()
    {
        base.OnEnable();
        WaveSpawnManager.UpdateWave += OnWaveCurrent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        WaveSpawnManager.UpdateWave -= OnWaveCurrent;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemySpawner();
    }

    protected virtual void LoadEnemySpawner()
    {
        if (spawner != null) return;
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        spawner = enemySpawner.GetComponent<EnemySpawner>();
        Debug.LogWarning(transform.name + ": LoadAbilities", gameObject);
    }

    private void OnWaveCurrent(Wave wave, bool WaveType)
    {
        this.checkALLEnemyDead = false;
        this.UpdateClear();

        enemies.AddRange(WaveType ? wave.GetEnemiesWave(wave) : wave.GetEnemiesRandom(wave));
        this.AddEnemies();
    }

    private void AddEnemies()
    {
        while (currentListEnemies < enemies.Count)
        {
            AddEnimesCurrent();
            currentListEnemies++;
            return;
        }
        Debug.Log("ADD Ngoai");

        AddEnimesCurrent();
    }
    private void AddEnimesCurrent()
    {
        minionCount = 0;
        this.nameEnemyandCount.Clear();
        var enemy = enemies[currentListEnemies];
        delay = enemy.TimeFirstSpawn;
        this.minionLimit = enemy.SumEnemy(enemy);
        this.nameEnemyandCount = enemy.ListNameAndCountEnemy(enemy);
    }
    protected override void Summoning()
    {
        if (minionCount >= minionLimit)
        {
            return;
        }

        Debug.Log("Sumoning loi ");

        SetNameSpawn();
        Transform pos = this.abilities.AbilityCtrl.SpawnEnemyPoints.GetRandom();
        Summon(pos);
        WaveSpawnManager.Instance.ProgressPortals.OnEnemySpawned();
        minionCount++;
        this.Active();
        this.ClearEnemySpawn();
    }
    private void UpdateClear()
    {
        currentListEnemies = 0;

        this.enemies.Clear();
    }
    private void ClearEnemySpawn()
    {
        if (minionCount < minionLimit) return;

        if (currentListEnemies < enemies.Count)
        {
            this.nameEnemyandCount.Clear();
            AddEnemies();
        }
    }

    protected override bool CheckTypeAbility()
    {
        if (minionCount < minionLimit) return false;

        if (currentListEnemies < enemies.Count) return false;

        if (minions.Count > 0) return false;

        Debug.Log("Da goi");

        return true;
    }
}