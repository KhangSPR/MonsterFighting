using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill1_SM : StateMachineBehaviour
{
    public Transform LightningAura;
    public float totalTime_Skill = 3f;
    float timeStart = -1f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeStart = totalTime_Skill;
        LightningAura = animator.transform.parent.GetChild(5);
        if(LightningAura!=null)
        {
            LightningAura.gameObject.SetActive(true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeStart <= 0) { 
            animator.GetComponent<PlayerModel>().OnAttackAnimationEnd(); 
            LightningAura.gameObject.SetActive(false); 
        }else
            timeStart -= Time.deltaTime;
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
