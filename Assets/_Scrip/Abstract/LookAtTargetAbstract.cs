using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LookAtTargetAbstract : SaiMonoBehaviour
{
    [Header("LookAt Target")]
    [SerializeField] protected Vector3 targetPosition;
    [SerializeField] protected float speedRot = 1f;
    
    protected virtual void FixedUpdate()
    {
        this.lookAtTarget();
    }
    public virtual void SetSpeedRot(float setSpeedRot)
    {
        this.speedRot = setSpeedRot;
    }

    protected abstract void lookAtTarget();
}
