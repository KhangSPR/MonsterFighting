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
            if (!skillEnable)
            {
                currentState = State.Stun;

            }
            else
            {
                if (!isVfxStunActive)
                {
                    this.effectCharacter.Vfx_Stun.SetActive(true);
                    isVfxStunActive = true; 
                    Debug.Log("Set Active VFX to true");
                }
            }

            Debug.Log("Call Stun");
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
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Dance", false);


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
                        else
                        {
                            PlayAnimation("Attack", true);

                            Debug.Log("Calll Attack !");
                        }
                        //Animation Another
                    }

                    if (hasAttack2)
                        isUsingAttack1 = !isUsingAttack1;

                    PlayAnimation("Moving", false);
                    PlayAnimation("Dance", false);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Stun", false);
                    PlayAnimation("Rage", false);
                    PlayAnimation("Fury", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);

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
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Dance", false);

                isAttacking = false;
                break;

            case State.Stun:
                if(this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);

                PlayAnimation("Stun", true);
                PlayAnimation("Fury", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);


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
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);

                isAttacking = false;

                break;

            case State.Fury:
                PlayAnimation("Fury", true);
                PlayAnimation("Stun", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);

                break;
            case State.Skill1:
                PlayAnimation("Skill1", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Attack", false);

                isAttacking = false;


                break;
            case State.Skill2:
                PlayAnimation("Skill2", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Attack", false);

                isAttacking = false;

                break;
            case State.Dance: //After Skill Animation Event
                PlayAnimation("Dance", true);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Attack", false);

                if (this.effectCharacter.Vfx_Stun.activeSelf && !this.isStun)
                    this.effectCharacter.Vfx_Stun.SetActive(false);

                isAttacking = false;

                break;

        }

        // If in skill state, do not perform other actions
        if (skillEnable || danceEnable)
        {
            return;
        }

        if (this.enemyCtrl.EnemyAttack.canAttack)
        {
            shouldAttack = true;
        }
        if (shouldAttack && CallAnimationSkill()) // Gọi hàm và kiểm tra điều kiện
        {
            return;
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
