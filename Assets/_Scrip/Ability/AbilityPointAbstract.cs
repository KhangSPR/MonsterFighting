using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityPointAbstract : SaiMonoBehaviour
{
    [Header("Ability ObjectCtrl")]
    [SerializeField] protected SpawnPoints spawnPoints;
    public SpawnPoints SpawnPoints => spawnPoints;

    [SerializeField] protected SpawnEnemyPoints spawnEnemyPoints;
    public SpawnEnemyPoints SpawnEnemyPoints => spawnEnemyPoints;

    [SerializeField] protected Abilities abilities;
    public Abilities Abilities => abilities;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawnPoints();
        this.LoadSpawnOutSizePoints();
        this.loadAbilities();

    }

    public void LoadSpawnPoints()
    {
        if (this.spawnPoints != null) return;
        this.spawnPoints = transform.GetComponentInChildren<SpawnPoints>();
        Debug.Log(gameObject.name + ": loadSpawnPoints" + gameObject);
    }

    public void LoadSpawnOutSizePoints()
    {
        if (this.spawnEnemyPoints != null) return;
        this.spawnEnemyPoints = transform.GetComponentInChildren<SpawnEnemyPoints>();
        Debug.Log(gameObject.name + ": LoadSpawnOutSizePoints" + gameObject);
    }
    protected virtual void loadAbilities()
    {
        if (this.abilities != null) return;
        this.abilities = transform.GetComponentInChildren<Abilities>();
        Debug.Log(gameObject.name + ": loadAbilities" + gameObject);
    }
}
