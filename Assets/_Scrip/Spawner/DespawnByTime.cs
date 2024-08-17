using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnByTime : Despawn
{
    // TODO: Not Finish
    [SerializeField] public float timer = 0f;
    [SerializeField] public float delay = 5f;
    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    this.ResetTimer();
    //}
    protected virtual void ResetTimer()
    {
        this.timer = 0;
    }
    protected override bool canDespawn()
    {
        this.timer += Time.fixedDeltaTime;
        if (this.timer > this.delay)
        {
            this.ResetTimer();
             return canDespawnFlag = true;
        }
        return canDespawnFlag = false;
    }
}
