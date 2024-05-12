using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySummon : ActiveAbility
{
    [Header("Ability Summon")]
    [SerializeField] protected Spawner spawner;
    [SerializeField] protected string namePrefab;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!this.isReady) return;
        this.Summoning();
    }

    protected virtual Transform Summon()
    {
        Debug.Log("Spawn ");

        Transform spawnPos = this.abilities.PortalCtrl.SpawnPoints.GetRandom();

        Transform minionPrefab = this.spawner.GetPrefabByName(namePrefab); //repair
        Transform minion = this.spawner.Spawn(minionPrefab, spawnPos.position, spawnPos.rotation);

        minion.gameObject.SetActive(true);

        return minion;
    }
    protected abstract void Summoning();
}
