using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : AbstractModel
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

        // Nếu đang ở trạng thái skill, không thực hiện các hành động khác
        if (currentState == State.Skill)
        {
            // Đợi skill hoàn thành
            if (!skillEnable)
            {
                currentState = State.Idle;
            }
            return;
        }

        Debug.Log("Goi 1 lan Animation skill");

        bool shouldAttack = false;

        switch (currentState)
        {
            case State.Idle:
                PlayAnimation("Attack", false);
                isAttacking = false;
                break;

            case State.Attack:
                if (!isAttacking && currentDelay <= 0)
                {
                    PlayAnimation("Attack", true);
                    isAttacking = true;
                    isAnimationAttackComplete = false;
                }
                break;
        }

        // Gọi skill nếu có đủ điều kiện
        if (this.playerCtrl.PlayerAttack.canAttack)
        {
            shouldAttack = true;
        }
        if(shouldAttack &&(this.ObjectCtrl.ObjMana.IsMana >= Skill2.manaSkill || this.ObjectCtrl.ObjMana.IsMana >= Skill1.manaSkill))
        {
            CallAnimationSkill();

            Debug.Log("Goi 1 lan khi du nang luong");

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
