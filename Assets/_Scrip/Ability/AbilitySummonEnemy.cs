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
public class AbilitySummonEnemy : AbilitySummon, IPortalSpawnListener
{
    [Header("List Enemys")]
    [SerializeField] List<EnemyNameAndCount> nameEnemyandCount;
    [SerializeField] List<Transform> minions;
    public List<Transform> Minions { get { return minions; } }

    [Header("Minion Count")]
    [SerializeField] int minionLimit = 0;
    [SerializeField] int minionCount = 0;
    [Header("Portal")]
    [SerializeField] Portals portal;
    public Portals Portal { get { return portal; } set { portal = value; } }

    [Header("Portal Control")]
    //[SerializeField] bool closePortal;
    [SerializeField] bool objectIsEnabled = false;
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(RemoveInactiveMinions), 2f, 1f);
        //if (minions.Count == 0)
        //{
        //    Destroy(gameObject); 
        //}
        InvokeRepeating(nameof(CheckObjectState), 2f, 1f);
    }
    protected void RemoveInactiveMinions()
    {
        for (int i = minions.Count - 1; i >= 0; i--)
        {
            if (!minions[i].gameObject.activeSelf)
            {
                minions.RemoveAt(i);
                PortalSpawnManager.Instance.ProgressPortals.EnemySpawn++;
                Debug.Log("Remove");
            }
        }
    }
    protected void CheckPortalcontrol()
    {
        if (minionCount < minionLimit)
        {
            return;
        }
        if (minionLimit == minionCount && minions.Count == 0)
        {
            this.abilities.PortalCtrl.ObjAppearSmall.IsAppearing = true;
            if(this.abilities.PortalCtrl.ObjAppearSmall.Appeared)
            {
                PortalSpawner.Instance.Despawn(transform.parent.parent);
                this.OnAllPortalsSpawned();
            }
        }
    }
    void CheckObjectState()
    {
        if (objectIsEnabled)
        {
            CheckPortalcontrol();
        }
    }
    public void EnableObject()
    {
        //Delay First Time
        this.delay = portal.DelaySpawnFirstEnemy;
        //Name and Count
        this.nameEnemyandCount = ListNameAndCountEnemy(portal);
        //Minion Max
        this.minionLimit = SumEnemy(portal);
        //Object
        objectIsEnabled = true;
    }
    public void DisableObject()
    {
        objectIsEnabled = false;

        this.Active();

        //InitScale
        this.abilities.PortalCtrl.ObjAppearSmall.InitScale();

        //Minion Count
        this.minionCount = 0;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemySpawner();
    }

    protected virtual void LoadEnemySpawner()
    {
        if (spawner != null) return;
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        spawner = enemySpawner.GetComponent<EnemySpawner>();
        Debug.LogWarning(transform.name + ": LoadAbilities", gameObject);
    }

    protected override void Summoning()
    {
        if (minionCount >= minionLimit) return;
        SetNameSpawn();

        Summon();

        minionCount++;

        this.Active();
    }
    protected override Transform Summon()
    {
        Transform minion = base.Summon();
        //minion.parent = this.abilities.AbilityObjectCtrl.transform;
        this.minions.Add(minion);
        return minion;
    }
    protected void SetNameSpawn()
    { 
        //Take Enemy Count < enemy Max
        List<EnemyNameAndCount> validEnemies = new List<EnemyNameAndCount>();
        foreach (var enemyInfo in nameEnemyandCount)
        {
            if (enemyInfo.spawnCount < enemyInfo.max)
            {
                validEnemies.Add(enemyInfo);
            }
        }
        int randomIndex = Random.Range(0, validEnemies.Count);
        var randomEnemy = validEnemies[randomIndex];

        //Set Delay
        delay = RandomRange(randomEnemy.radomMin, randomEnemy.radomMax);
        randomEnemy.spawnCount++;
        // Set namePrefab
        namePrefab = randomEnemy.name;


    }
    int SumEnemy(Portals portals)
    {
        int totalEnemyCount = 0;

        foreach (EnemyType enemyType in portals.enemyTypes)
        {
            totalEnemyCount += enemyType.countEnemy;
        }

        return totalEnemyCount;
    }
    List<EnemyNameAndCount> ListNameAndCountEnemy(Portals portals)
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
    public void OnAllPortalsSpawned()
    {
        PortalSpawnManager.Instance.portalsSpawning.Remove(portal);
        PortalSpawnManager.Instance.CheckPortalsSpawned();
    }
}