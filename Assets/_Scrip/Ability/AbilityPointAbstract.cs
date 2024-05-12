using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityPointAbstract : SaiMonoBehaviour
{
    [Header("Ability ObjectCtrl")]
    [SerializeField] protected SpawnPoints spawnPoints;
    public SpawnPoints SpawnPoints => spawnPoints;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawnPoints();
    }
    protected virtual void LoadSpawnPoints()
    {
        if (this.spawnPoints != null) return;
        this.spawnPoints = transform.GetComponentInChildren<SpawnPoints>();
        Debug.Log(gameObject.name + ": loadSpawnPoints" + gameObject);
    }
}
