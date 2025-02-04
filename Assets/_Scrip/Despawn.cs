using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Despawn : SaiMonoBehaviour
{
    [SerializeField]
    protected bool canDespawnFlag = false;

    private void FixedUpdate()
    {
      /*  this.canDespawn();*/ //Repair
        this.Despawning();

    }

    protected virtual void Despawning()
    {
        if (!this.canDespawnFlag) return;
        this.deSpawnObjParent();
        this.canDespawnFlag = false;
    }

    protected abstract void deSpawnObjParent();
    //{
    //    //Destroy(transform.parent.gameObject);
    //}
    public void ResetCanDespawnFlag()
    {
        this.canDespawnFlag = true;

        Debug.Log("ResetCanDespawnFlag");
        //transform.parent.gameObject.SetActive(false);
    }
    protected abstract bool canDespawn();

}
