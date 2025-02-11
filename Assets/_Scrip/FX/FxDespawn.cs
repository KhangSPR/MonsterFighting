using System;
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

    public event Action OnFXSkill;
    public float delayTimerTrigger = 0f;

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
        this.FXSkill();

        StartCoroutine(DestroyFragmentsAfterDelay(delayTimerTrigger));

        base.deSpawnObjParent();
    }
    public virtual void FXSkill()
    {
        OnFXSkill?.Invoke();
    }
    private IEnumerator DestroyFragmentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        this.ResetTimer();

        FXSpawner.Instance.Despawn(transform.parent);


    }
}
