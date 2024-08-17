using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FXType
{
    text,
    effect,
}
public class FxDespawn : DespawnByTime
{
    [SerializeField] private FXType fxType;
    protected override bool canDespawn()
    {
        if (fxType == FXType.effect)
        {
            return canDespawnFlag = false;
        }

        return base.canDespawn(); // Call the base method for other types
    }
    protected override void deSpawnObjParent()
    {
        Debug.Log("Despawn");
        FXSpawner.Instance.Despawn(transform.parent);
    }
}
