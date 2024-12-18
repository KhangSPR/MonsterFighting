using UIGameDataMap;
using UnityEngine;

public class AbilitySummonPortal : AbilitySummon
{
    [Header("Portal")]
    [SerializeField] Portals portal;
    public Portals Portal { get { return portal; } set { portal = value; } }

    [SerializeField] protected ObjAppearSmall objAppearSmall;
    public ObjAppearSmall ObjAppearSmall => objAppearSmall;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemySpawner();
        this.LoadObjAppearSmall();
    }
    protected virtual void LoadEnemySpawner()
    {
        if (spawner != null) return;
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        spawner = enemySpawner.GetComponent<EnemySpawner>();
        Debug.LogWarning(transform.name + ": LoadAbilities", gameObject);
    }
    protected virtual void LoadObjAppearSmall()
    {
        if (objAppearSmall != null) return;
        if (this.abilities.AbilityCtrl is PortalCtrl portalCtrl)
        {
            this.objAppearSmall = portalCtrl.ObjAppearSmall;

            Debug.LogWarning(transform.name + ": LoadObjAppearSmall", gameObject);
        }
    }

    public void EnableObject()
    {
        this.delay = portal.DelaySpawnFirstEnemy;
        this.nameEnemyandCount = portal.ListNameAndCountEnemy(portal);
        this.minionLimit = portal.SumEnemy(portal);
        this.minionCount = 0;
        this.checkALLEnemyDead = false;

        Debug.Log("IsAppearing: " + this.objAppearSmall.IsAppearing); //True
        Debug.Log("Appeared: " + this.objAppearSmall.Appeared); //False
    }

    public void DisableObject()
    {
        this.Active();
        this.objAppearSmall.InitScale();
    }

    protected override void Summoning()
    {
        if (minionCount >= minionLimit)
        {
            //Debug.Log("Return Sumoning");

            return;
        }
        SetNameSpawn();
        Transform pos = this.abilities.AbilityCtrl.SpawnPoints.GetRandom();
        this.Summon(pos);
        WaveSpawnManager.Instance.ProgressPortals.OnEnemySpawned();
        minionCount++;
        this.Active();
    }

    protected override bool CheckTypeAbility()
    {
        if (objAppearSmall == null)
        {
            //Debug.Log("Return false: objAppearSmall is null");
            return false;
        }

        if (minionCount < minionLimit)
        {
            //Debug.Log("Return false: objectIsEnabled is false and minionCount is less than minionLimit");
            return false;
        }

        if (minions.Count > 0)
        {
            //Debug.Log("Return false: minions count is greater than 0");
            return false;
        }

        this.objAppearSmall.IsAppearing = true;

        if (this.objAppearSmall.Appeared)
        {
            //Debug.Log("Load Spawner");
            PortalSpawner.Instance.Despawn(transform.parent.parent);
        }

        //Debug.Log("Return true");
        return true;
    }

}