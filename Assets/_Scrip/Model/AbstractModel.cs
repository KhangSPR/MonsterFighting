using System.Collections;
using System.Collections.Generic;
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


    protected bool isAttacking = false;
    protected bool isAnimationComplete = false;
    protected float delayAttack = 3.0f; 
    protected float currentDelay = 0f; 
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCircleCollider2D();
        this.LoadBoxCollider2D();
        this.LoadRigibody();
        this.LoadAnimator();
        this.LoadAnimationImpact();
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
        Debug.Log(transform.name + ": LoadLoadAnimator", gameObject);
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
        this.isAnimationComplete = true;
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
    protected void Idle()
    {
        this.animator.Play("Idle");
    }
}
