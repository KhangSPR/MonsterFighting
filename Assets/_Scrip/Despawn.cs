using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Despawn : SaiMonoBehaviour
{
    [SerializeField]
    protected bool canDespawnFlag = false;

    private void FixedUpdate()
    {
        this.Despawning();
        this.canDespawn();
    }

    protected virtual void Despawning()
    {
        if (!this.canDespawnFlag) return;
        this.deSpawnObjParent();
        this.canDespawnFlag = false;
    }

    protected virtual void deSpawnObjParent()
    {

    }
    public void ResetCanDespawnFlag()
    {
        this.canDespawnFlag = true;
        Debug.Log("ResetCanDespawnFlag");
    }
    protected abstract bool canDespawn();

}
