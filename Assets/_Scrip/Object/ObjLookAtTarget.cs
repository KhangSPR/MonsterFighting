using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class ObjLookAtTarget : LookAtTargetAbstract
{
    Transform TargetTransform;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBowTrasfrom();
    }
    protected virtual void LoadBowTrasfrom()
    {
        if (this.TargetTransform != null) return;
        this.TargetTransform = transform.parent.Find("Modle/LookAtTarget");
        Debug.Log(transform.name + ": LoadPlayerCtrl", gameObject);
    }
    protected override void lookAtTarget()
    {
        Quaternion rotation = GetDesiredRotation();

        if (rotation != Quaternion.identity)
        {
            if (TargetTransform != null)
            {
                TargetTransform.rotation = rotation;
            }
            else
            {
                transform.parent.rotation = rotation;
            }
        }
    }

    private Quaternion GetDesiredRotation()
    {
        if (TargetTransform != null)
        {
            Vector3 diff = this.targetPosition - TargetTransform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0f, 0f, rot_z);
        }
        else
        {
            Vector3 diff = this.targetPosition - transform.parent.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            return Quaternion.Euler(0f, 0f, rot_z);
        }
    }
}
