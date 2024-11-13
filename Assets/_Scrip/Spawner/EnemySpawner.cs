using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (EnemySpawner.instance != null) Debug.LogError("Onlly 1 EnemySpawner Warning");
        EnemySpawner.instance = this;
    }
    public override Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newEnemy = base.Spawn(prefab, spawnPos, rotation);
        //this.AddHPObj(newEnemy);

        //Add All Spawn Enemy
        GameManager.Instance.AllModleSpawn.Add(newEnemy);

        return newEnemy;
    }
    protected virtual void AddHPObj(Transform newEnemy)
    {
        ObjectCtrl newObjectCtrl =  newEnemy.GetComponent<ObjectCtrl>();
        Transform newHPBar = HPBarSpawner.Instance.Spawn(HPBarSpawner.HPBar, newEnemy.position, Quaternion.identity);


        HPBar hpBar = newHPBar.GetComponent<HPBar>();

        hpBar.SetObjectCtrl(newObjectCtrl);
        hpBar.SetFollowTarget(newEnemy);

        newHPBar.transform.gameObject.SetActive(true);

    }
}