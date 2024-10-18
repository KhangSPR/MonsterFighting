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
                PlayAnimation("Attack", false);
                PlayAnimation("Idle", true);
                PlayAnimation("Skill1", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Stun", false);


                isAttacking = false;
                break;

            case State.Attack:
                if (!isAttacking && currentDelay <= 0)
                {
                    PlayAnimation("Attack", true);
                    PlayAnimation("Idle", false);
                    PlayAnimation("Skill1", false);
                    PlayAnimation("Skill2", false);
                    PlayAnimation("Stun", false);

                    isAttacking = true;
                    isAnimationAttackComplete = false;
                }
                break;
            case State.Skill1:
                PlayAnimation("Skill1", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Skill2", false);

                isAttacking = false;

                break;
            case State.Skill2:
                PlayAnimation("Skill2", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Skill1", false);

                isAttacking = false;

                break;
            case State.Stun:
                if (this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);

                PlayAnimation("Stun", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);


                isAttacking = false;
                break;
        }
        // Nếu đang ở trạng thái skill, không thực hiện các hành động khác
        if (skillEnable)
        {
            return;
        }


        // Gọi skill nếu có đủ điều kiện
        if (this.playerCtrl.PlayerAttack.canAttack)
        {
            shouldAttack = true;
        }
        if (shouldAttack && CallAnimationSkill()) // Gọi hàm và kiểm tra điều kiện
        {
            Debug.Log("Call Skill Player");

            return;
        }

        if (shouldAttack)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Idle;
        }

        // Sau khi tấn công hoàn tất
        if (isAttacking && isAnimationAttackComplete)
        {
            this.AttackType();
            isAnimationAttackComplete = false;
            currentDelay = delayAttack; // Thời gian chờ sau khi tấn công
            currentState = State.Idle;
        }
    }

}
