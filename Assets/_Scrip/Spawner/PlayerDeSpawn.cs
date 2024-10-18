using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeSpawn : Despawn
{
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl => playerCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadPlayerCtrl();
    }
    protected virtual void loadPlayerCtrl()
    {
        if (this.playerCtrl != null) return;
        this.playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
        Debug.Log(gameObject.name + ": loadPlayerCtrl" + gameObject);
    }
    protected override void deSpawnObjParent()
    {
        PlayerSpawner.Instance.Despawn(transform.parent);

        playerCtrl.AbstractModel.DameFlash.SetMaterialDamageFlash();
    }

    protected override bool canDespawn()
    {
        return canDespawnFlag = false;
    }
}
