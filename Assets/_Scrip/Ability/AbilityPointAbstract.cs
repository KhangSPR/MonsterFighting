using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityPointAbstract : SaiMonoBehaviour
{
    [Header("Ability ObjectCtrl")]
    [SerializeField] protected BaseSpawnPoints spawnPoints;
    public BaseSpawnPoints SpawnPoints => spawnPoints;


    [SerializeField] protected Abilities abilities;
    public Abilities Abilities => abilities;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSBaseSpawnPoints();
        this.loadAbilities();

    }
    public void LoadSBaseSpawnPoints()
    {
        if (this.spawnPoints != null) return;
        this.spawnPoints = transform.GetComponentInChildren<BaseSpawnPoints>();
        Debug.Log(gameObject.name + ": LoadSpawnOutSizePoints" + gameObject);
    }
    protected virtual void loadAbilities()
    {
        if (this.abilities != null) return;
        this.abilities = transform.GetComponentInChildren<Abilities>();
        Debug.Log(gameObject.name + ": loadAbilities" + gameObject);
    }
}
