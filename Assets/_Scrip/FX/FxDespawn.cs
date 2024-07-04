using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxDespawn : DespawnByTime
{
    protected override void deSpawnObjParent()
    {
        FXSpawner.Instance.Despawn(transform.parent);
    }
}
