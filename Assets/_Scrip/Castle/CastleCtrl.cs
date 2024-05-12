using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleCtrl : SaiMonoBehaviour
{
    [Header("Castle Ctrl")]
    [SerializeField] protected  CastleDamageReceiver castleDamageReceiver;
    public CastleDamageReceiver CastleDamageReceiver => castleDamageReceiver;

    [SerializeField] protected Despawn despawn;
    public Despawn Despawn => despawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCastleDamageReceiver();
        this.loadDespawn();
    }
    protected virtual void loadCastleDamageReceiver()
    {
        if (this.castleDamageReceiver != null) return;
        this.castleDamageReceiver = transform.GetComponentInChildren<CastleDamageReceiver>();
        Debug.Log(gameObject.name + ": loadCastleDamageReceiver" + gameObject);
    }
    protected virtual void loadDespawn()
    {
        if (this.despawn != null) return;
        this.despawn = transform.GetComponentInChildren<Despawn>();
        Debug.Log(gameObject.name + ": loadDespawn" + gameObject);
    }
}
