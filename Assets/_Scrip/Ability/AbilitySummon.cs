using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

[System.Serializable]
public class EnemyNameAndCount
{
    public string name;
    public int max;
    public int spawnCount;
    [Header("Time min/max Random")]
    public float radomMin = 0;
    public float radomMax = 0;
}
public abstract class AbilitySummon : ActiveAbility
{
    [Header("List Enemies")]
    protected List<EnemyNameAndCount> nameEnemyandCount = new List<EnemyNameAndCount>();

    [Header("Minion Count")]
    [SerializeField] protected int minionLimit = 0;
    [SerializeField] protected int minionCount = 0;

    [Header("Ability Summon")]
    [SerializeField] protected Spawner spawner;
    [SerializeField] protected string namePrefab;
    //[SerializeField]
    protected List<Transform> minions = new List<Transform>();
    public List<Transform> Minions { get { return minions; } }
    [Header("Ability Land")]
    [SerializeField] protected int landIndex;
    public int LandIndex { get { return landIndex; } set { landIndex = value; } }
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(RemoveInactiveMinions), 2f, 1f);

        InvokeRepeating(nameof(CheckMinions), 2f, 2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!this.isReady) return;
        this.Summoning();
    }

    protected virtual Transform Summon(Transform spawnPos)
    {

        Transform minionPrefab = this.spawner.GetPrefabByName(namePrefab);
        if (minionPrefab == null)
        {
            Debug.LogError("Prefab is null for name: " + namePrefab);
            return null;
        }

        if (spawner == null)
        {
            Debug.LogError("Spawner is null in AbilitySummon");
            return null;
        }


        Transform minion = this.spawner.Spawn(minionPrefab, spawnPos.position, spawnPos.rotation);

        EnemyCtrl enemyCtrl = minion.GetComponent<EnemyCtrl>();


        enemyCtrl.ObjAppearBigger.CheckCallAppearing = true;
        enemyCtrl.ObjLand.SetLand(landIndex);

        minion.gameObject.SetActive(true);
        this.AddSummon(minion);

        Debug.Log("Sumon " + transform.parent.parent.name);
        return minion;
    }

    protected virtual void RemoveInactiveMinions()
    {
        if (minions == null || minions.Count <= 0) return;

        for (int i = minions.Count - 1; i >= 0; i--)
        {
            if (minions[i] == null || !minions[i].gameObject.activeSelf)
            {
                minions.RemoveAt(i);

            }
        }
    }

    protected void SetNameSpawn()
    {
        List<EnemyNameAndCount> validEnemies = new List<EnemyNameAndCount>();
        foreach (var enemyInfo in nameEnemyandCount)
        {
            if (enemyInfo.spawnCount < enemyInfo.max)
            {
                validEnemies.Add(enemyInfo);
            }
        }

        if (validEnemies.Count == 0)
        {
            Debug.LogError("No valid enemies to spawn.");
            return;
        }

        int randomIndex = Random.Range(0, validEnemies.Count);
        var randomEnemy = validEnemies[randomIndex];
        delay = Random.Range(randomEnemy.radomMin, randomEnemy.radomMax);
        randomEnemy.spawnCount++;
        namePrefab = randomEnemy.name;
    }

    protected void AddSummon(Transform minion)
    {
        this.minions.Add(minion);
    }

    List<EnemyNameAndCount> ListNameAndCountEnemy(Portals portals)
    {
        List<EnemyNameAndCount> enemyNameAndCount = new List<EnemyNameAndCount>();

        foreach (EnemyType enemyType in portals.enemyTypes)
        {
            EnemyNameAndCount enemy = new EnemyNameAndCount
            {
                name = enemyType.name,
                max = enemyType.countEnemy,
                radomMin = enemyType.timerMin,
                radomMax = enemyType.timerMax
            };
            enemyNameAndCount.Add(enemy);
        }

        return enemyNameAndCount;
    }
    protected virtual void CheckMinions()
    {

        if (minions == null) return;

        if (CheckTypeAbility())
        {

            Debug.Log("1 Check" + transform.parent.parent.name);
            Active();

            this.checkALLEnemyDead = true;
        }
        else
        {
            Debug.Log("2 Check");


            this.checkALLEnemyDead = false;
        }
    }
    protected abstract bool CheckTypeAbility();
    protected abstract void Summoning();
}
