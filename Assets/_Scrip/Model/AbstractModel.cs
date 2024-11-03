using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AttackTypeAnimation
{
    BulletDefault,
    BulletPX,
    Animation,
}

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractModel : AbstractCtrl
{
    [Header("Abstract Model")]
    [SerializeField] protected CircleCollider2D circleCollider;
    [SerializeField] protected BoxCollider2D boxCollider;
    [SerializeField] protected Rigidbody2D _rigidbody;

    [SerializeField] protected bool canAttack = false;
    [SerializeField] protected State currentState;
    [SerializeField] protected Animator animator;
    [SerializeField] protected AttackTypeAnimation attackTypeAnimation;
    protected AttackTypeAnimation currentAttackTypeAnimation;
    [SerializeField] protected AnimationImpact animationImpact;
    [SerializeField] protected DameFlash dameFlash;
    public DameFlash DameFlash => dameFlash;
    [SerializeField] protected EffectCharacter effectCharacter;
    public EffectCharacter EffectCharacter => effectCharacter;
    [SerializeField]
    protected bool isAttacking = false;
    public bool IsAttacking => isAttacking;
    [SerializeField]
    protected bool isAnimationAttackComplete = false;
    [SerializeField]
    protected bool isAnimationDeadComplete = false;
    [SerializeField]
    protected float delayAttack;
    public float DelayAttack => delayAttack;
    [SerializeField]
    protected float currentDelay = 0f;
    [SerializeField]
    protected bool isStun = false;
    public bool IsStun { get { return isStun; } set { isStun = value; } }


    [SerializeField]
    protected bool isRage = false;
    public bool IsRage { get { return isRage; } set { isRage = value; } }

    [SerializeField]
    protected bool isFuryGain = false;
    public bool IsFuryGain { get { return isFuryGain; } set { isFuryGain = value; } }

    //[SerializeField] protected float fadeDuration = 2.0f;
    [SerializeField]
    protected bool isHorn = false;
    public bool IsHorn { get { return isHorn; } set { isHorn = value; } }

    protected bool isUsingAttack1 = true; // Variable to check current attack type
    [SerializeField]
    protected bool hasAttack2 = false;
    //protected State previousState;
    [SerializeField]
    protected bool activeAttack = false;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCircleCollider2D();
        this.LoadBoxCollider2D();
        this.LoadRigibody();
        this.LoadAnimator();
        this.LoadAnimationImpact();
        this.LoadDameFlash();
        this.LoadEffectCharacter();
    }
    protected override void Start()
    {
        base.Start();
        this.LoaHasAttack2();
        this.LoadCurrentAttackAnimation();

    }
    public void SetDelayCharacter(float delayTime)
    {
        delayAttack = delayTime;
    }
    public void ResetDamageCharacter() //Reset Damage
    {
        this.ObjectCtrl.DamageSender.Damage = enemyCtrl.EnemySO.basePointsAttack;
    }
    protected virtual void LoadCurrentAttackAnimation()
    {
        this.currentAttackTypeAnimation = this.attackTypeAnimation;
    }
    protected virtual void LoaHasAttack2()
    {
        hasAttack2 = HasParameter(animator, "Attack2");
    }
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
    protected virtual void LoadBoxCollider2D()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.boxCollider.isTrigger = true;
        Debug.Log(transform.name + ": LoadBoxCollider2D", gameObject);
    }

    protected virtual void LoadCircleCollider2D()
    {
        if (this.circleCollider != null) return;
        this.circleCollider = GetComponent<CircleCollider2D>();
        this.circleCollider.isTrigger = true;
        Debug.Log(transform.name + ": LoadCollider", gameObject);
    }

    public void LoadAnimator()
    {
        if (animator != null) return;
        animator = transform.GetComponent<Animator>();
        Debug.Log(transform.name + ": LoadAnimator", gameObject);
    }

    public void LoadDameFlash()
    {
        if (dameFlash != null) return;
        dameFlash = transform.GetComponent<DameFlash>();
        Debug.Log(transform.name + ": LoadDameFlash", gameObject);
    }

    public void LoadEffectCharacter()
    {
        if (effectCharacter != null) return;
        effectCharacter = transform.GetComponent<EffectCharacter>();
        Debug.Log(transform.name + ": LoadEffectCharacter", gameObject);
    }

    protected virtual void LoadRigibody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log(transform.name + ": LoadRigibody", gameObject);
    }

    protected virtual void LoadAnimationImpact()
    {
        //if (attackType != attackType.Animation) return;
        if (this.animationImpact != null) return;

        //this.animationImpact = transform.Find("AnimationImpact").GetComponent<AnimationImpact>();
        this.animationImpact = transform.GetComponentInChildren<AnimationImpact>();
        Debug.Log(transform.name + ": LoadAnimationImpact", gameObject);
    }

    protected void FixedUpdate()
    {
        this.AnimationLoading();
    }

    protected override void Update()
    {
        base.Update();
        this.CheckDelay();
    }
    [SerializeField]
    protected bool comPleteStateTransition = false;
    public void OnComPleteStateTransition()
    {
        this.comPleteStateTransition = true;
    }    
    public void OnAttackAnimationEnd()
    {
        this.isAnimationAttackComplete = true;
    }
    public void OnDeadAnimationEnd()
    {
        this.isAnimationDeadComplete = true;
    }
    public void SetOnDeadAnimation()
    {
        this.isAnimationDeadComplete = false;
        this.EnablePhysics();
        this.effectCharacter.ResetAlpha();

        Debug.Log("Set On Dead");
    }

    protected virtual void CheckDelay()
    {
        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            if (currentDelay <= 0)
            {
                currentDelay = 0;
            }
        }
    }

    public void EventDameSent()
    {
        animationImpact.damageSent = false;
    }

    public void PlayAnimation(string animationName, bool state)
    {
        if (HasParameter(animationName))
        {
            this.animator.SetBool(animationName, state);
        }
        //else
        //{

        //}
    }

    private bool HasParameter(string paramName)
    {
        foreach (AnimatorControllerParameter param in this.animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }



    protected abstract void AnimationLoading();
    protected virtual void AttackType()
    {
        switch (attackTypeAnimation)
        {
            case global::AttackTypeAnimation.BulletDefault:
                this.objCtrl.BulletShooter.Shoot();
                break;
            case global::AttackTypeAnimation.BulletPX:
                this.objCtrl.BulletShooter.ShootPX();
                break;
            case global::AttackTypeAnimation.Animation:
                animationImpact.damageSent = false;
                animationImpact.gameObject.SetActive(true);


                Debug.Log("Animation Set True");
                break;
            default:
                // Xử lý cho các trường hợp khác (nếu cần)
                break;
        }
    }
    protected void Move()
    {
        this.animator.Play("Move");
    }

    public void Idle()
    {
        this.animator.Play("Idle");
    }
    protected virtual void DisablePhysics()
    {
        this.boxCollider.enabled = false;
        this.circleCollider.enabled = false;
        this._rigidbody.simulated = false;
    }
    protected virtual void EnablePhysics()
    {
        this.boxCollider.enabled = true;
        this.circleCollider.enabled = true;
        this._rigidbody.simulated = true;
        this.animator.enabled = true;
    }
    protected virtual void SetFalseAnimation()
    {
        this.isStun = false;
        this.isRage = false;
        this.isFuryGain = false;
        this.IsHorn = false;
        this.hasAxeFisrtActive = false;
    }
    protected void Dead()
    {
        HandleDeath();
    }
    private void HandleDeath()
    {
        this.effectCharacter.StartFadeOut();

    }
    [SerializeField]
    protected Vector3 deadPosition;
    protected bool isSetActiveModle = false;
    public void SetEventActiveModle()
    {
        this.isSetActiveModle = true;
    }

    //Rage Skill
    public void SetRageState(ObjRageSkill.RageType rageType)
    {
        switch (rageType)
        {
            case ObjRageSkill.RageType.NormalRage:
                // Chuyển sang trạng thái Rage thông thường
                currentState = State.NormalRage;
                Debug.Log("Switched to Normal Rage");
                break;

            case ObjRageSkill.RageType.AttackAxe:
                // Chuyển sang trạng thái Rage khi không có rìu
                currentState = State.AttackAxe;
                // Bạn có thể thêm logic riêng cho trường hợp này nếu cần
                Debug.Log("Switched to Rage Without Axe");
                break;

            default:
                // Mặc định là không có trạng thái Rage
                currentState = State.NormalRage;
                Debug.Log("Switched to Normal State (no Rage)");
                break;
        }
    }
    [SerializeField] protected bool hasAxeFisrtActive = false;
    //Horn Animation
    public void OnHornAnimationComplete()
    {
        IsHorn = false;
    }
    //Axe Animation
    public void OnAxeFisrtAnimationComplete()
    {
        hasAxeFisrtActive = true;

        //Obj Attack Boss
        this.objCtrl.ObjAttack.ApplyColliderAdjustments();
    }
    //Fury Animation
    public void SetEventAnimationFuryEnd()
    {
        isFuryGain = false;
    }
    public void SetEventAnimationrageTrue()
    {
        this.isRage = true;
    }
    public void SetEventAnimationrageFalse()
    {
        //OF Pig // Animation Rage
        this.isRage = false;
    }
    [SerializeField] protected bool danceEnable = false;
    //Dance Animation -> Goblin
    public void SetDanceAnimation()
    {
        danceEnable = true;

        currentState = State.Dance;
    }
    public void OnDaceAnimationComplete()
    {
        danceEnable = false;
    }
    //Call Skill == VFX Stun
    [SerializeField]
    protected bool isVfxStunActive = false;

    ////////////////////////////////////////////////////////////////////////-----------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected SkillCharacter Skill1, Skill2;
    [SerializeField]
    protected bool skillEnable = false;

    // Biến tạm lưu skill đang hoạt động
    private SkillCharacter currentActiveSkill;
    public SkillCharacter CurrentActiveSkill => currentActiveSkill;

    public void OnSkillEnabeleComplete()
    {
        skillEnable = false;
        Debug.Log("Call OnSkillEnabeleComplete");
    }

    public void SetSkill(float manaSkill1, bool lockSkill1, float damage1, ISkill skillType1, float distanceAttack1, float manaSkill2, bool lockSkill2, float damage2, ISkill skillType2, float distanceAttack2)
    {
        Skill1 = new SkillCharacter(manaSkill1, lockSkill1, damage1, skillType1, distanceAttack1);
        Skill2 = new SkillCharacter(manaSkill2, lockSkill2, damage2, skillType2, distanceAttack2);
    }

    protected virtual bool CallAnimationSkill()
    {
        if (skillEnable) return false;

        // Kiểm tra và gọi kỹ năng nếu đủ điều kiện
        if (TryUseAvailableSkill())
        {
            skillEnable = true;
            return true; // Kỹ năng đã được thực thi
        }

        return false; // Không kỹ năng nào được gọi
    }

    private bool TryUseAvailableSkill()
    {
        // Kiểm tra Skill2 trước
        if (Skill2 != null && Skill2.unlockSkill && Skill2.CanUseSkill(ObjectCtrl.ObjMana, ObjectCtrl.transform.position, ObjectCtrl.ObjAttack.ListObjAttacks[0].position))
        {
            CallSkill(Skill2, State.Skill2);  // Gọi hàm CallSkill
            return true;
        }

        // Kiểm tra Skill1
        if (Skill1 != null && Skill1.CanUseSkill(ObjectCtrl.ObjMana, ObjectCtrl.transform.position, ObjectCtrl.ObjAttack.ListObjAttacks[0].position))
        {
            CallSkill(Skill1, State.Skill1);  // Gọi hàm CallSkill

            Debug.Log("CallSkill 1");

            return true;
        }

        return false;
    }

    // Hàm gọi skill với thông tin kỹ năng và trạng thái
    private void CallSkill(SkillCharacter skill, State state)
    {
        currentState = state;               // Cập nhật trạng thái hiện tại
        currentActiveSkill = skill;         // Cập nhật kỹ năng hiện tại
        skill.UseSkill(ObjectCtrl);         // Thực thi kỹ năng

        isVfxStunActive = false;

        Debug.Log("Đã gọi " + state.ToString() + " : " + transform.parent.name);
    }

    // Hàm này sẽ được gọi trong Unity Event
    public void OnActiveSkillAnimation()
    {
        if (currentActiveSkill != null)
        {
            currentActiveSkill.ActiveSkill(objCtrl); // Kích hoạt skill hiện tại
            Debug.Log("Đang kích hoạt skill: " + (currentActiveSkill == Skill1 ? "Skill 1" : "Skill 2"));
        }
    }


}

