using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLookAtTargetSetter : ObjLookAtTarget
{
    public Transform target;
    protected override void FixedUpdate()
    {
        this.GetTargetPosition();
        base.FixedUpdate();
    }
    protected virtual void GetTargetPosition() //Hàm con trỏ chuột
    {
        if (this.target == null) return;

        this.targetPosition = this.target.transform.position;
        this.targetPosition.z = 0;
    }
}
