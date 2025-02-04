using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByDistance : Despawn
{
    [SerializeField] protected float Dislimit = 50f;
    [SerializeField] protected float Distance = 0f;
    [SerializeField] protected Camera maincam;

    protected override void LoadComponents()
    {
        this.loadCamera();
    }
    protected virtual void loadCamera()
    {
        if (this.maincam != null) return;
        this.maincam = Transform.FindObjectOfType<Camera>();
        Debug.Log(transform.parent.name + "Load Camera " + gameObject);

    }

    protected override bool canDespawn()
    {
        this.Distance = Vector3.Distance(transform.position, maincam.transform.position);
        if (this.Distance > this.Dislimit) return canDespawnFlag = true;
        return canDespawnFlag = false;
    }

    protected override void deSpawnObjParent()
    {
    }
}
