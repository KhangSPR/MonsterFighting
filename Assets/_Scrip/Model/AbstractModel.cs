using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum attackType
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
    [SerializeField] protected attackType attackType;
    [SerializeField] protected AnimationImpact animationImpact;
    [SerializeField] protected DameFlash dameFlash;
    public DameFlash DameFlash => dameFlash;
    [SerializeField] protected EffectCharacter effectCharacter;
    public EffectCharacter EffectCharacter => effectCharacter;

    protected bool isAttacking = false;
    protected bool isAnimationAttackComplete = false;
    protected bool isAnimationDeadComplete = false;
    protected float delayAttack = 3.0f;
    protected float currentDelay = 0f;
    protected bool isStun = false;
    public bool IsStun { get { return isStun; } set { isStun = value; } }


    //[SerializeField] protected float fadeDuration = 2.0f;
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
        this.animator.SetBool(animationName, state);
    }

    protected abstract void AnimationLoading();
    protected virtual void AttackType()
    {
        switch (attackType)
        {
            case attackType.BulletDefault:
                this.objCtrl.BulletShooter.Shoot();
                break;
            case attackType.BulletPX:
                this.objCtrl.BulletShooter.ShootPX();
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
    }
    protected void Dead()
    {
        HandleDeath();
    }
    private void HandleDeath()
    {
        this.effectCharacter.StartFadeOut();

    }
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

    public void OnSkillEnableComplete()
    {
        skillEnable = false;
    }

    public void SetSkill(float manaSkill1, bool lockSkill1, float damage1, ISkill skillType1, float manaSkill2, bool lockSkill2, float damage2, ISkill skillType2)
    {
        Skill1 = new SkillCharacter(manaSkill1, lockSkill1, damage1, skillType1);
        Skill2 = new SkillCharacter(manaSkill2, lockSkill2, damage2, skillType2);
    }

    protected virtual void CallAnimationSkill()
    {
        if (skillEnable) return;

        // Kiểm tra Skill2 trước, nếu không thì kiểm tra Skill1
        if (TryUseSkill(Skill2, "Skill2") || TryUseSkill(Skill1, "Skill1"))
        {
            skillEnable = true;
        }
    }

    private bool TryUseSkill(SkillCharacter skill, string animationName)
    {
        if (skill.CanUseSkill(ObjectCtrl.ObjMana))
        {
            currentState = State.Skill;
            animator.Play(animationName);
            currentActiveSkill = skill;  // Lưu skill hiện tại vào biến tạm
            skill.UseSkill(ObjectCtrl);
            Debug.Log("Đã gọi " + animationName + " : " + transform.parent.name);
            return true;
        }
        return false;
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

