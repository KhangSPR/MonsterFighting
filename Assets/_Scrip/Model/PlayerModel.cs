using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerModel : AbstractModel
{
    protected override void AnimationLoading()
    {
        // Kiểm tra nếu nhân vật đang chết
        if (this.objCtrl.ObjectDamageReceiver.IsDead)
        {
            Debug.Log("Play Animation Dead");
            this.animator.Play("Dead");

            this.DisablePhysics();
            this.SetFalseAnimation();

            if (!isAnimationDeadComplete) return;


            this.Dead();


            if (this.effectCharacter.FadeCharacter)
            {

                animator.Rebind();

                //Despawn
                this.ObjectCtrl.Despawn.ResetCanDespawnFlag();
            }
            return;
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
        //Debug.Log("Goi 1 lan Animation skill");

        bool shouldAttack = false;

        switch (currentState)
        {
            case State.Idle:
                if (this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);
                PlayAnimation("Attack", false);
                PlayAnimation("Idle", true);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Stun", false);
                PlayAnimation("Melee", false);


                isAttacking = false;
                break;

            case State.Attack:
                if (!isAttacking && currentDelay <= 0)
                {
                    if (this.effectCharacter.Vfx_Stun.activeSelf)
                        this.effectCharacter.Vfx_Stun.SetActive(false);
                    PlayAnimation("Attack", true);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Stun", false);
                    PlayAnimation("Melee", false);

                    isAttacking = true;
                    isAnimationAttackComplete = false;
                    isAnimationAttackCheckFinish = false; //Apply to animation attack defaut 
       
                }
                break;
            case State.Skill1:
                PlayAnimation("Skill1", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Melee", false);

                isAttacking = false;

                break;
            case State.Skill2:
                PlayAnimation("Skill2", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Melee", false);

                isAttacking = false;

                break;
            case State.Stun:
                if (this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);

                PlayAnimation("Stun", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Melee", false);


                isAttacking = false;
                break;
            case State.MeleeWitch:
                if (!isAttacking && currentDelay <= 0)
                {
                    if (this.effectCharacter.Vfx_Stun.activeSelf)
                        this.effectCharacter.Vfx_Stun.SetActive(false);
                    PlayAnimation("Attack", false);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Stun", false);
                    PlayAnimation("Melee", true);

                    isAttacking = true;
                    isAnimationAttackComplete = false;
                    //isAnimationAttackCheckFinish = false;
                }
                break;
        }
        // Nếu đang ở trạng thái skill, không thực hiện các hành động khác
        if (skillEnable)
        {
            return;
        }


        // Gọi skill nếu có đủ điều kiện
        if (this.playerCtrl.PlayerAttack.CheckCanAttack)
        {
            shouldAttack = true;
        }
        if (shouldAttack && CallAnimationSkill()) // Gọi hàm và kiểm tra điều kiện
        {
            Debug.Log("Call Skill Player");

            return;
        }
        //Animation Attack - Idle
        // Check Melee attack
        if (this.objCtrl.ObjMelee != null && this.objCtrl.ObjMelee.CheckCanAttack)
        {
            currentState = State.MeleeWitch;
            Debug.Log("Call Melee Attack");
        }
        else if (shouldAttack && currentState != State.MeleeWitch) // Default attack if not Melee
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Idle;
        }

        // Common handling for animation after the attack is complete
        if (isAttacking && isAnimationAttackComplete)
        {
            // Chọn loại hoạt ảnh dựa trên trạng thái tấn công hiện tại
            if (currentState == State.MeleeWitch && isAnimationAttackCheckFinish) // True nếu đang trong AnimationMelee
            {
                attackTypeAnimation = AttackTypeAnimation.Animation;
                this.SetDelayCharacter(playerCtrl.CharacterStatsFake.AttackSpeedMelee);
            }
            else if (currentState != State.MeleeWitch) // Chỉ thực hiện khi trạng thái không vừa chuyển từ MeleeWitch
            {
                attackTypeAnimation = this.currentAttackTypeAnimation;
                this.SetDelayCharacter(playerCtrl.CharacterStatsFake.AttackSpeed);
            }

            isAnimationAttackComplete = false;

            //previousState = currentState;

            currentState = State.Idle;

            currentDelay = delayAttack; 

            this.AttackType();
        }

    }

}
