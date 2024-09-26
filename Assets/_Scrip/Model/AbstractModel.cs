using System;
using System.Collections;
using UnityEngine;

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
        if (attackType != attackType.Animation) return;
        if (this.animationImpact != null) return;

        this.animationImpact = transform.Find("AnimationImpact").GetComponent<AnimationImpact>();
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

    public void DameSent()
    {
        animationImpact.damageSent = false;
    }

    public void PlayAnimation(string animationName, bool state)
    {
        this.animator.SetBool(animationName, state);
    }

    protected abstract void AnimationLoading();
    protected abstract void AttackType();
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
        this.animator.Play("Dead");

        DisablePhysics();

        if (!isAnimationDeadComplete) return;

        this.effectCharacter.StartFadeOut();

        if(this.effectCharacter.FadeCharacter)
        {

            animator.Rebind();

            this.ObjectCtrl.Despawn.ResetCanDespawnFlag();

        }        
    }
    ////////////////////////////////////////////////////////////////////////-----------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected SkillCharacter Skill1;
    [SerializeField]
    protected SkillCharacter Skill2;
    [SerializeField]
    protected bool skillEnable = false;

    public void OnSkillEnableComplete()
    {
        this.skillEnable = false;
    }
    public void SetSkill(float manaSkill1, bool lockSkill1, float manaSkill2, bool lockSkill2)
    {
        // Thiết lập giá trị cho Skill1
        Skill1 = new SkillCharacter
        {
            manaSkill = manaSkill1,
            unlockSkill = lockSkill1
        };

        // Thiết lập giá trị cho Skill2
        Skill2 = new SkillCharacter
        {
            manaSkill = manaSkill2,
            unlockSkill = lockSkill2
        };
    }

    protected virtual void CallAnimationSkill()
    {
        if (this.skillEnable) return; 

        if (Skill2.unlockSkill && this.ObjectCtrl.ObjMana.IsMana >= Skill2.manaSkill)
        {
            currentState = State.Skill; // Chuyển sang trạng thái Skill
            this.animator.Play("Skill2");
            this.ObjectCtrl.ObjMana.DeductMana(Skill2.manaSkill);
            this.skillEnable = true;  // Đặt cờ skill đang chạy

            //PlayAnimation("Attack", false);


            Debug.Log("Da Goi skill 2 : " + transform.parent.name);

            return;
        }

        if (Skill1.unlockSkill && this.ObjectCtrl.ObjMana.IsMana >= Skill1.manaSkill)
        {
            currentState = State.Skill; // Chuyển sang trạng thái Skill
            this.animator.Play("Skill1");
            this.ObjectCtrl.ObjMana.DeductMana(Skill1.manaSkill);
            this.skillEnable = true;  // Đặt cờ skill đang chạy

            //PlayAnimation("Attack", false);


            Debug.Log("Da Goi skill 1 : " + transform.parent.name);

        }
    }

}
[Serializable] 
public class SkillCharacter
{
    public float manaSkill;
    public bool unlockSkill;
}
