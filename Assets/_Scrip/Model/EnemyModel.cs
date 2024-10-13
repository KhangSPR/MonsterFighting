using UnityEngine;

public class EnemyModel : AbstractModel
{
    protected bool isVFXCalled = false;
    protected override void AnimationLoading()
    {
        if (this.objCtrl.ObjectDamageReceiver.IsDead)
        {
            Debug.Log("Play Animation Dead");
            this.animator.Play("Dead");

            this.DisablePhysics();
            this.SetFalseAnimation();

            if (!isAnimationDeadComplete) return;

            if (!isVFXCalled)
            {
                this.effectCharacter.CallVFXDeadEnemy();
                isVFXCalled = true;
            }

            if (this.effectCharacter.IsDissolveComplete)
            {
                Debug.Log("isDissolveComplete");

                this.Dead();

                if (this.effectCharacter.FadeCharacter)
                {
                    animator.Rebind();

                    this.effectCharacter.SetDissolveCompleteFalse();
                    isVFXCalled = false;

                    this.ObjectCtrl.Despawn.ResetCanDespawnFlag();

                    Debug.Log("Call 1 Lan");
                }
            }
            return;
        }

        if (isFuryGain)
        {
            currentState = State.Fury;
        }
        if (isStun)
        {
            currentState = State.Stun;
        }

        bool shouldAttack = false;

        switch (this.currentState)
        {
            case State.Move:
                PlayAnimation("Attack", false);

                // Kiểm tra nếu animator có Attack2 trước khi gọi
                if (hasAttack2) // Kiểm tra ở layer 0
                {
                    PlayAnimation("Attack2", false); // Tắt Attack2 khi di chuyển
                }

                PlayAnimation("Moving", true);
                PlayAnimation("Stun", false);
                PlayAnimation("Idle", false);
                PlayAnimation("Rage", false);
                PlayAnimation("Fury", false);

                isAttacking = false;
                if (this.enemyCtrl.ObjMovement.gameObject.activeSelf)
                {
                    this.enemyCtrl.ObjMovement.Move();

                    Debug.Log("Move Animation");
                }
                break;

            case State.Attack:
                if (!isAttacking && currentDelay <= 0)
                {
                    // Hoán đổi giữa Attack và Attack2
                    if (isUsingAttack1)
                    {
                        PlayAnimation("Attack", true);
                    }
                    else
                    {
                        // Kiểm tra nếu animator có Attack2 trước khi gọi
                        if (hasAttack2) // Kiểm tra ở layer 0
                        {
                            PlayAnimation("Attack2", true);
                        }
                        //Animation Another
                        PlayAnimation("Attack", true);
                    }

                    if (hasAttack2)
                        isUsingAttack1 = !isUsingAttack1;

                    PlayAnimation("Moving", false);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Stun", false);
                    PlayAnimation("Rage", false);
                    PlayAnimation("Fury", false);

                    isAttacking = true;
                    isAnimationAttackComplete = false;

                    Debug.Log("Goi Attack");
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

                // Kiểm tra nếu animator có Attack2 trước khi gọi
                if (hasAttack2) // Kiểm tra ở layer 0
                {
                    PlayAnimation("Attack2", false);
                }

                PlayAnimation("Stun", false);
                PlayAnimation("Rage", false);
                PlayAnimation("Fury", false);

                isAttacking = false;
                break;

            case State.Stun:
                PlayAnimation("Stun", true);
                PlayAnimation("Fury", false);
                PlayAnimation("Moving", false);

                isAttacking = false;
                break;

            case State.Rage:
                PlayAnimation("Rage", true);
                PlayAnimation("Attack", true);

                // Kiểm tra nếu animator có Attack2 trước khi gọi
                if (hasAttack2) // Kiểm tra ở layer 0
                {
                    PlayAnimation("Attack2", true); //Repair
                }

                PlayAnimation("Stun", false);
                PlayAnimation("Moving", false);

                isAttacking = false;

                break;

            case State.Fury:
                PlayAnimation("Fury", true);
                PlayAnimation("Stun", false);
                PlayAnimation("Moving", false);

                break;
        }

        if (this.enemyCtrl.EnemyAttack.canAttack)
        {
            shouldAttack = true;
        }
        if (IsRage && shouldAttack && !IsStun)
        {
            currentState = State.Rage;
            return;
        }
        if (shouldAttack)
        {
            if (isAttacking && isAnimationAttackComplete)
            {
                this.AttackType();
                isAnimationAttackComplete = false;
                currentDelay = delayAttack; // Set thời gian chờ
                currentState = State.Idle; // Trạng thái tiếp theo là Idle
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
