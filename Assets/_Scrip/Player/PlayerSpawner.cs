using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    private static PlayerSpawner instance;
    public static PlayerSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (PlayerSpawner.instance != null) Debug.LogError("Onlly 1 PlayerSpawner Warning");
        PlayerSpawner.instance = this;
    }
    public override Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPlayer = base.Spawn(prefab, spawnPos, rotation);
        this.AddHPObj(newPlayer);

        //Add All Spawn Enemy
        GameManager.Instance.AllModleSpawn.Add(newPlayer);

        return newPlayer;
    }
    protected virtual void AddHPObj(Transform newEnemy)
    {
        PlayerCtrl newPlayerCtrl = newEnemy.GetComponent<PlayerCtrl>();
        Transform newHPBar = BarSpawner.Instance.Spawn(BarSpawner.HPBar, newEnemy.position, Quaternion.identity);


        CharacterBar hpBar = newHPBar.GetComponent<CharacterBar>();

        hpBar.SetObjectCtrl(newPlayerCtrl);
        hpBar.SetFollowTarget(newPlayerCtrl.TargetBar);

        newHPBar.transform.gameObject.SetActive(true);

    }
}