using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDespawn : Despawn
{
    [SerializeField] protected CastleCtrl castleCtrl;
    public CastleCtrl CastleCtrl => castleCtrl;
    protected override bool canDespawn()
    {
        if(this.CastleCtrl.CastleDamageReceiver.IsDead)
        {
            return canDespawnFlag = true;
        }
        return canDespawnFlag = false;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCastleCtrl();
    }
    protected virtual void loadCastleCtrl()
    {
        if (this.castleCtrl != null) return;
        this.castleCtrl = transform.parent.GetComponent<CastleCtrl>();
        Debug.Log(gameObject.name + ": loadCastleCtrl" + gameObject);
    }
}
