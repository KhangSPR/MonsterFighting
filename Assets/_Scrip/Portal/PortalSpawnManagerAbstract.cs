using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalSpawnManagerAbstract : SaiMonoBehaviour
{
    [Header("Portal SpawnManager")]
    [SerializeField] protected WaveSpawnManager portalSpawnManagerCtrl;
    public WaveSpawnManager PortalSpawnerCtrl => portalSpawnManagerCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadPortalSpawnerCtrl();
    }
    protected virtual void loadPortalSpawnerCtrl()
    {
        if (this.portalSpawnManagerCtrl != null) return;
        this.portalSpawnManagerCtrl = transform.parent.GetComponent<WaveSpawnManager>();
        Debug.Log(gameObject.name + ": loadPortalSpawnerCtrl" + gameObject);
    }
}
