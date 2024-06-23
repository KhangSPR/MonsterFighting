using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FireballMoving : StateMachineBehaviour
{
    public RaycastHit2D nearestEnemyHit;
    public Transform nearestEnemyT;
    public Vector3 nearestEnemy;

    public void MoveBullet(Animator animator, Vector3 target)
    {
        //if (bullet == null) return;
        Vector3 Direction = (target - animator.transform.parent.Find("Modle").position).normalized;//
        animator.transform.position += Direction * animator.transform.GetComponent<FireballController>().speed * Time.deltaTime;
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //DetectNearestEnemy(animator);
    }

    private RaycastHit2D DetectNearestEnemy(Animator animator)
    {
        var model = animator.transform.parent.Find("Modle");
        var hits = Physics2D.RaycastAll(model.position, Vector2.right, float.MaxValue);
        Debug.Log("hits :" + hits.Length);
        RaycastHit2D result = new RaycastHit2D();
        foreach (var hit in hits)
        {
            Debug.Log("hit:" + hit.transform.parent.name, hit.transform.parent);
            if (hit.transform.parent.tag == "Enemy")
            {
                result = hit; break;
            }
        }
       
        return result;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nearestEnemyHit = DetectNearestEnemy(animator);
        if (!nearestEnemyHit) return;
        nearestEnemy = nearestEnemyHit.point;
        nearestEnemyT = nearestEnemyHit.transform.parent;
        MoveBullet(animator, nearestEnemyHit.transform.parent.Find("Modle").position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
