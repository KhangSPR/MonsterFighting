using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName ="Debuff/FrozenSlow")]
public class FrozenSlow : Debuff
{
    [Range(0,1f)] public float velocityScale;// Percentage
    public float frozenTotalTime;
    public override void ApplyDebuff(Transform target)
    {
        CoroutineRunner.instance.StartCoroutine(SlowMoving(target));
    }
    IEnumerator SlowMoving(Transform target)
    {
        //Default
        var oldSpeed = target.GetComponentInChildren<ObjMovement>().moveSpeed;
        var oldAnimationSpeed = target.GetComponentInChildren<Animator>().speed;
        //New
        target.GetComponentInChildren<ObjMovement>().moveSpeed -= oldSpeed * velocityScale;
        target.GetComponentInChildren<Animator>().speed -= oldAnimationSpeed * velocityScale;
        yield return new WaitForSeconds(frozenTotalTime);
        target.GetComponentInChildren<ObjMovement>().moveSpeed = oldSpeed;
        target.GetComponentInChildren<Animator>().speed = oldAnimationSpeed;
    }
}
