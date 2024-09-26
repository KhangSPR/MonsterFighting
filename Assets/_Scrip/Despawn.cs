using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Despawn : SaiMonoBehaviour
{

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
        Destroy(transform.parent.gameObject);
    }
    public void ResetCanDespawnFlag()
    {
        this.canDespawnFlag = true;

        //transform.parent.gameObject.SetActive(false);
    }
    protected abstract bool canDespawn();

}
