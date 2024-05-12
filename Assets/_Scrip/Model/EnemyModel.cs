using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : AbstractModel
{
    protected override void AnimationLoading()
    {
        bool shouldAttack = false;

        switch (this.currentState)
        {
            case State.Move:
                PlayAnimation("Attack", false);
                PlayAnimation("Moving", true);
                isAttacking = false;
                if (this.enemyCtrl.ObjMovement.gameObject.activeSelf)
                {
                    this.enemyCtrl.ObjMovement.Move();
                }
                break;

            case State.Attack:
                if (!isAttacking && currentDelay <= 0)
                {
                    PlayAnimation("Attack", true);
                    PlayAnimation("Moving", false);
                    PlayAnimation("Idle", false);
                    isAttacking = true;
                    isAnimationComplete = false;
                }
                else
                {
                    currentState = State.Idle;
                }
                break;

            case State.Idle:
                PlayAnimation("Moving", false);
                PlayAnimation("Idle", true);
                PlayAnimation("Attack", false);
                isAttacking = false;
                break;
        }

        if (this.enemyCtrl.EnemyAttack.canAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack)
        {
            if (isAttacking && isAnimationComplete)
            {
                this.AttackType();
                isAnimationComplete = false;
                currentDelay = delayAttack; // Set time Wait
                currentState = State.Idle; // Next State Idle

                //Debug.Log(isAnimationComplete);
            }
            else if(currentState == State.Idle && currentDelay > 0)
            {
                currentState = State.Idle;
            }
            else
            {
                currentState = State.Attack;
            }
        }
        else
        {
            currentState = State.Move;
        }
    }

    protected override void AttackType()
    {
        switch (attackType)
        {
            case attackType.BulletDefault:
                this.enemyCtrl.EnemyShooter.Shoot();
                break;
            case attackType.BulletPX:
                this.enemyCtrl.EnemyShooter.ShootPX();
                break;
            case attackType.Animation:
                animationImpact.damageSent = false;
                animationImpact.gameObject.SetActive(true);
                break;
            default:
                // Xử lý cho các trường hợp khác (nếu cần)
                break;
        }
    }
}
