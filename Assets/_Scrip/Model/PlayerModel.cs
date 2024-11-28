using UnityEngine;

public class PlayerModel : AbstractModel
{
    protected override void AnimationLoading()
    {
        if (!animator.enabled) return;

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

                // Despawn
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
            if(IsHit)
            {
                isHit = false;

                Debug.Log("Log false");

            }
            Debug.Log("Call Stun");
        }
        if(isHit && !isStun &&  !skillEnable)
        {
            currentState = State.Hit;

            if(IsAnimationPlaying("Stun"))
            {
                isHit = false;
                this.ActivateTrigger("Upper");
            }

            Debug.Log("Log Hit");
        }
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
                PlayAnimation("Deff", false);

                isAttacking = false;
                ResetTrigger("Upper");
                ResetTrigger("Lower");
                //activeAttack = false;
                break;
            case State.Deff:
                if (this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);
                PlayAnimation("Deff", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Skill1", false);
                PlayAnimation("Stun", false);
                PlayAnimation("Hit", false);


                ResetTrigger("Upper");
                ResetTrigger("Lower");

                break;
            case State.Hit: //apply to shield
                if (this.effectCharacter.Vfx_Stun.activeSelf)
                    this.effectCharacter.Vfx_Stun.SetActive(false);
                PlayAnimation("Hit", true);

                ResetTrigger("Upper");
                ResetTrigger("Lower");

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
                    PlayAnimation("Deff", false);


                    isAttacking = true;

                    if (activeAttack && !comPleteStateTransition)
                    {
                        activeAttack = false;
                        isAnimationAttackComplete = false;
                    }
                    comPleteStateTransition = false;
                }
                break;

            case State.Skill1:
                PlayAnimation("Skill1", true);
                PlayAnimation("Idle", false);
                PlayAnimation("Attack", false);
                PlayAnimation("Skill2", false);
                PlayAnimation("Melee", false);
                PlayAnimation("Deff", false);
                ResetTrigger("Upper");
                ResetTrigger("Lower");

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
                PlayAnimation("Deff", false);

                ResetTrigger("Upper");
                ResetTrigger("Lower");
                PlayAnimation("Hit", false);

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
                    comPleteStateTransition = false;
                }
                break;
        }

        if (skillEnable)
        {
            if(attackTypeAnimation == AttackTypeAnimation.Deff)
            {
                this.ActivateTrigger("Lower");
                this.IsHit = false;
            }
            return;
        }
        if (this.playerCtrl.PlayerAttack.CheckCanAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack && CallAnimationSkill())
        {
            Debug.Log("Call Skill Player");
            return;
        }

        // Animation Attack - Idle
        if (this.objCtrl.ObjMelee != null && this.objCtrl.ObjMelee.CheckCanAttack)
        {
            currentState = State.MeleeWitch;

            //Debug.Log("Call Melee Attack");
        }
        else if (shouldAttack && attackTypeAnimation == AttackTypeAnimation.Deff)
        {
            currentState = State.Deff;
            isHit = false;
            this.ActivateTrigger("Upper");

            //Debug.Log("ActivateTrigger Upper");
        }
        else if (shouldAttack && currentState != State.MeleeWitch)
        {
            StartStateTransition(State.Attack);
        }
        else
        {
            currentState = State.Idle;

            //Debug.Log("ActivateTrigger Lower");
        }

        if (attackTypeAnimation == AttackTypeAnimation.Deff)
        {
            if (currentState == State.Idle || currentState == State.Skill1)
            {
                this.ActivateTrigger("Lower");

                //Debug.Log("Trigger Active");
            }
        }

        //Deff No Call
        if (isAttacking && isAnimationAttackComplete)
        {
            Debug.Log("State Current: " + currentState);

            if (currentState == State.MeleeWitch)
            {
                attackTypeAnimation = AttackTypeAnimation.Animation;
                this.SetDelayCharacter(playerCtrl.CharacterStatsFake.AttackSpeedMelee);
            }
            else
            {
                attackTypeAnimation = this.currentAttackTypeAnimation;
                this.SetDelayCharacter(playerCtrl.CharacterStatsFake.AttackSpeed);
            }

            if (isStateTransitioning && currentState == State.Attack)
            {
                Debug.Log("State Current Return: " + currentState);
                isStateTransitioning = false;
                isAnimationAttackComplete = false;
                //currentState = State.Attack;

                FXSpawner.Instance.SendFXTextMessage("Miss!", SkillType.Miss, objCtrl.TargetPosition, Quaternion.identity);

                return;
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
                isStateTransitioning = false;
            }
        }
    }

    [SerializeField]
    private bool isStateTransitioning = false;
    private void StartStateTransition(State newState)
    {
        if(!comPleteStateTransition && attackTypeAnimation == AttackTypeAnimation.Animation)
        {
            isStateTransitioning = true; 


        }
        currentState = newState; // Cập nhật trạng thái mới
    }
}
