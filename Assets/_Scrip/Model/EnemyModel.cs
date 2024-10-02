using UnityEngine;

public class EnemyModel : AbstractModel
{
    protected override void AnimationLoading()
    {
        // Kiểm tra nếu nhân vật đang chết
        if (this.objCtrl.ObjectDamageReceiver.IsDead)
        {
            Debug.Log("Play Animation Dead");
            this.Dead();
            return;
        }


        bool shouldAttack = false;

        if (isStun)
        {
            currentState = State.Stun;         
        }

        switch (this.currentState)
        {
            case State.Move:
                PlayAnimation("Attack", false);
                PlayAnimation("Moving", true);
                PlayAnimation("Stun", false);
                PlayAnimation("Idle", false);

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
                    PlayAnimation("Stun", false);

                    isAttacking = true;
                    isAnimationAttackComplete = false;
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
                PlayAnimation("Stun", false);

                isAttacking = false;
                break;
            case State.Stun:
                PlayAnimation("Stun", true);
                isAttacking = false;
                

                break;
        }

        if (this.enemyCtrl.EnemyAttack.canAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack)
        {
            if (isAttacking && isAnimationAttackComplete)
            {
                this.AttackType();
                isAnimationAttackComplete = false;
                currentDelay = delayAttack; // Set time Wait
                currentState = State.Idle; // Next State Idle
            }
            else if (currentState == State.Idle && currentDelay > 0)
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

}