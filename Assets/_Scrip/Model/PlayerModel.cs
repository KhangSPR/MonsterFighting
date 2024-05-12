using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : AbstractModel
{
    protected override void AnimationLoading()
    {
        bool shouldAttack = false;

        switch (currentState)
        {
            case State.Idle:
                PlayAnimation("Attack", false);
                isAttacking = false;
                break;
            case State.Attack:
                if (!isAttacking && currentDelay <=0)
                {
                    PlayAnimation("Attack", true);
                    isAttacking = true;
                    isAnimationComplete = false;
                }
                break;
        }

        if (this.playerCtrl.PlayerAttack.canAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Idle;
        }

        if (isAttacking && isAnimationComplete)
        {
            this.AttackType();
            isAnimationComplete = false;
            currentDelay = delayAttack; // Set time Wait
            currentState = State.Idle; // Next State Idle

            Debug.Log(isAnimationComplete);

        }
    }

    protected override void AttackType()
    {
        switch (attackType)
        {
            case attackType.BulletDefault:
                this.playerCtrl.PlayerShooter.Shoot();
                break;
            case attackType.BulletPX:
                this.playerCtrl.PlayerShooter.ShootPX();
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
