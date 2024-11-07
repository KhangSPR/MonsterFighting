using System;
using UnityEngine;
using static ObjRageSkill;

public class EnemyModel : AbstractModel
{
    protected bool isVFXCalled = false;
    [SerializeField] Transform targetDead;
    protected override void AnimationLoading()
    {
        if (this.objCtrl.ObjectDamageReceiver.IsDead)
        {
            if (targetDead != null && this.isRage) // Kiểm tra xem targetDead có giá trị hợp lệ không
            {
                this.Dead();

                if (this.effectCharacter.FadeCharacter)
                {
                    Debug.Log("Dead Position X: " + deadPosition.x);

                    targetDead.position = new Vector3(Mathf.Abs(deadPosition.x), transform.position.y, transform.position.z);
                    Debug.Log("TargetDead Position Set To: " + targetDead.position);

                    isSetActiveModle = true;
                }
            }
            if(isSetActiveModle) this.effectCharacter.ResetAlpha();

            if (this.effectCharacter.FadeCharacter) return;


            Debug.Log("IS Dead Play Animation");
            this.DisablePhysics();
            this.SetFalseAnimation();
            this.animator.Play("Dead");



            if (!isAnimationDeadComplete) return;
            //Haven't VFXDissolve
            if (this.effectCharacter.VFX_Dissolve == null)
            {
                this.Dead();

                if (this.effectCharacter.FadeCharacter)
                {
                    animator.Rebind();

                    this.isSetActiveModle = false;
                    this.ObjectCtrl.Despawn.ResetCanDespawnFlag();
                }
                return;
            }

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
                    this.isVFXCalled = false;
                    this.isSetActiveModle = false;
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
            if (!skillEnable || (!IsHorn && !skillEnable))
            {
                currentState = State.Stun;
                //currentState = State.StunAxe;

                //Debug.Log("Skill ! Enable Stun");
            }
            else
            {
                if (!isVfxStunActive)
                {
                    if (this.effectCharacter.Vfx_Stun != null)
                    {
                        this.effectCharacter.Vfx_Stun.SetActive(true);
                    }
                    isVfxStunActive = true;
                    //Debug.Log("Set Active VFX to true");
                }
            }

            //Debug.Log("Call Stun");
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
                PlayAnimation("NormalRage", false);
                PlayAnimation("Fury", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Dance", false);
                PlayAnimation("Melee", false);


                isAttacking = false;
                currentDelay = 0.08f;

                if (this.enemyCtrl.ObjMovement.gameObject.activeSelf)
                {
                    this.enemyCtrl.ObjMovement.Move();

                    //Debug.Log("Move Animation");
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
                    PlayAnimation("NormalRage", false);
                    PlayAnimation("Fury", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Melee", false);


                    isAttacking = true;
                    if (activeAttack && !comPleteStateTransition)
                    {
                        activeAttack = false;
                        isAnimationAttackComplete = false;
                    }
                    comPleteStateTransition = false;

                    Debug.Log("Goi Attack");
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
                PlayAnimation("NormalRage", false);
                PlayAnimation("Fury", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Dance", false);
                PlayAnimation("Melee", false);


                isAttacking = false;
                break;

            case State.Stun:
                if (this.effectCharacter.Vfx_Stun != null && this.effectCharacter.Vfx_Stun.activeSelf)
                {
                    this.effectCharacter.Vfx_Stun.SetActive(false);
                }
                ////Apply Goblin
                //if(this.EffectCharacter.)

                PlayAnimation("Stun", true);
                if (IsFuryGain)
                {
                    PlayAnimation("Fury", true);
                }
                else
                {
                    PlayAnimation("Fury", false);
                }

                PlayAnimation("Moving", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Melee", false);


                isAttacking = false;
                break;

            case State.NormalRage:
                // Kiểm tra nếu animator có Attack2 trước khi gọi
                //if (hasAttack2) // Kiểm tra ở layer 0
                //{
                //    PlayAnimation("Attack2", true); //Repair
                //}
                Debug.Log("NormalRage");

                if (!isAttacking && currentDelay <= 0)
                {
                    if (this.effectCharacter.Vfx_Stun != null)
                    {
                        if (this.effectCharacter.Vfx_Stun.activeSelf)
                            this.effectCharacter.Vfx_Stun.SetActive(false);
                    }
                    PlayAnimation("NormalRage", true);
                    PlayAnimation("Attack", true);
                    PlayAnimation("Attack2", true);

                    PlayAnimation("Stun", false);
                    PlayAnimation("Moving", false);
                    PlayAnimation("Idle", false);

                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Melee", false);


                    deadPosition = transform.position;

                    if (isAnimationAttackComplete)
                    {
                        isAnimationAttackComplete = false;
                    }

                    isAttacking = true;
                    comPleteStateTransition = false;

                }
                else
                {
                    currentState = State.Idle;

                }

                break;
            case State.Fury:
                PlayAnimation("Fury", true);
                PlayAnimation("Stun", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Melee", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Idle", false);

                isAttacking = false;

                Debug.Log("Fury: " + isAttacking);
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
            case State.MeleeWitch:
                if (!isAttacking && currentDelay <= 0)
                {
                    if (this.effectCharacter.Vfx_Stun != null)
                    {
                        if (this.effectCharacter.Vfx_Stun.activeSelf)
                            this.effectCharacter.Vfx_Stun.SetActive(false);
                    }
                    PlayAnimation("Attack", false);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Stun", false);
                    PlayAnimation("Melee", true);
                    PlayAnimation("Dance", false);
                    PlayAnimation("Moving", false);


                    isAttacking = true;
                    comPleteStateTransition = false;
                }
                break;
            case State.Horn:
                PlayAnimation("Horn", true);
                PlayAnimation("Attack", false);
                PlayAnimation("Moving", false);
                PlayAnimation("Idle", false);

                if (this.effectCharacter.Vfx_Stun.activeSelf && !this.isStun)
                    this.effectCharacter.Vfx_Stun.SetActive(false);
                //isAttacking = false;

                break;
            case State.AttackAxe:
                if (!isAttacking && currentDelay <= 0)
                {
                    if (this.effectCharacter.Vfx_Stun != null)
                    {
                        if (this.effectCharacter.Vfx_Stun.activeSelf)
                            this.effectCharacter.Vfx_Stun.SetActive(false);
                    }
                    PlayAnimation("AttackAxe", true);
                    PlayAnimation("MovingAxe", false);
                    PlayAnimation("IdleAxe", false);
                    PlayAnimation("StunAxe", false);
                    PlayAnimation("Horn", false);
                    PlayAnimation("Fury", false);
                    PlayAnimation("Stun", false);


                    isAttacking = true;
                    comPleteStateTransition = false;


                }
                else
                {
                    currentState = State.IdleAxe;

                }
                break;
            case State.IdleAxe:
                PlayAnimation("MovingAxe", false);
                PlayAnimation("AttackAxe", false);
                PlayAnimation("IdleAxe", true);
                PlayAnimation("StunAxe", false);
                PlayAnimation("Horn", false);
                PlayAnimation("Fury", false);
                PlayAnimation("Stun", false);


                isAttacking = false;
                break;
            case State.MoveAxe:
                PlayAnimation("MovingAxe", true);
                PlayAnimation("AttackAxe", false);
                PlayAnimation("IdleAxe", false);
                PlayAnimation("StunAxe", false);
                PlayAnimation("Horn", false);
                PlayAnimation("Fury", false);
                PlayAnimation("Stun", false);



                if (this.enemyCtrl.ObjMovement.gameObject.activeSelf)
                {
                    this.enemyCtrl.ObjMovement.Move();

                    Debug.Log("Move Animation");
                }
                isAttacking = false;
                break;
        }
        // If in skill state, do not perform other actions
        if (skillEnable || danceEnable || IsHorn)
        {
            return;
        }

        if (this.enemyCtrl.EnemyAttack.CheckCanAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack && CallAnimationSkill()) // Gọi hàm và kiểm tra điều kiện
        {
            return;
        }
        if (IsRage && shouldAttack && !IsStun) //IsRage Call Animation Fury Event
        {
            string rageTypeName = objCtrl.ObjRageSkill.CurrentRageType.ToString();

            if (Enum.TryParse(rageTypeName, out State resultState))
            {
                currentState = resultState;

                //Debug.Log("Set Rage");
            }
            else
            {
                Debug.LogWarning($"Giá trị {rageTypeName} không tồn tại trong State.");
            }



            // Xử lý chung cho animation sau khi hoàn thành tấn công
            if (isAttacking && isAnimationAttackComplete)
            {
                if (!activeAttack)
                {
                    this.AttackType();
                    Debug.Log("Attack Type Action Rage " + transform.parent);
                }
                activeAttack = true;

                if (comPleteStateTransition)
                {
                    activeAttack = false;
                    isAnimationAttackComplete = false;
                    currentState = State.Idle;

                    currentDelay = delayAttack;
                }

            }
            return;
        }
        // Kiểm tra tấn công cận chiến bất kể trạng thái shouldAttack
        if (hasAxeFisrtActive)
        {
            if (shouldAttack)
            {
                currentState = State.AttackAxe;
            }
            else
            {
                currentState = State.MoveAxe;
            }
        }
        else if (this.objCtrl.ObjMelee != null && this.objCtrl.ObjMelee.CheckCanAttack)
        {
            currentState = State.MeleeWitch;
            //Debug.Log("Call Melee Attack");
        }
        else if (shouldAttack) // Kiểm tra shouldAttack chỉ cho các tấn công khác
        {
            // Nếu không phải là Melee Attack, thực hiện tấn công thường
            if (currentState != State.MeleeWitch)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;

                Debug.Log("Idle ShouldAttack");
            }

        }
        else
        {
            currentState = State.Move; // Nếu không tấn công, di chuyển
        }
        // Xử lý chung cho animation sau khi hoàn thành tấn công
        if (isAttacking && isAnimationAttackComplete)
        {
            // Lựa chọn loại animation dựa trên trạng thái tấn công hiện tại
            if (currentState == State.MeleeWitch)
            {
                attackTypeAnimation = AttackTypeAnimation.Animation;
                this.SetDelayCharacter(enemyCtrl.EnemySO.attackSpeedMelee); // Delay dựa trên tốc độ cận chiến

                //Debug.Log("Da Set attackSpeedMelee");
            }
            else
            {
                attackTypeAnimation = this.currentAttackTypeAnimation;


                this.SetDelayCharacter(enemyCtrl.EnemySO.attackSpeed); // Delay dựa trên tốc độ tấn công
            }
            if (!activeAttack)
            {
                this.AttackType();
                Debug.Log("Attack Type Action");
            }
            activeAttack = true;

            if (comPleteStateTransition)
            {
                activeAttack = false;
                isAnimationAttackComplete = false;
                currentState = State.Idle;

                currentDelay = delayAttack;
            }

        }
    }
}
