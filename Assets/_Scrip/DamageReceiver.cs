using System;
using System.Collections;
using UnityEngine;


// Base class for damage receivers
public abstract class DamageReceiver : AbstractCtrl
{
    [Header("Damage Receiver")]
    [SerializeField] protected int isHP = 1;
    public int IsHP => isHP;
    [SerializeField] protected int isMaxHP = 3;
    public int IsMaxHP => isMaxHP;

    protected bool isBurning;
    protected bool isTwitching;
    protected bool isGlacing;

    protected bool isDead;
    public bool IsDead => isHP <= 0;
    [Space]
    [Space]
    [Header("Object")]
    [SerializeField] protected ObjectCtrl ObjectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjectCtrl();
    }

    protected virtual void LoadObjectCtrl()
    {
        if (ObjectCtrl != null) return;
        ObjectCtrl = transform.parent.GetComponent<ObjectCtrl>();
        Debug.Log(gameObject.name + ": Loaded ObjectCtrl for " + gameObject.name);
    }

    protected override void OnEnable()
    {
        ReBorn();
    }

    protected virtual void LoadValue()
    {
        LoadObjectCtrl();
        ReBorn();
    }

    public virtual void ReBorn()
    {
        isHP = isMaxHP;
        isDead = false;
    }

    protected virtual void AddHealth(int amount)
    {
        if (isDead) return;
        isHP = Mathf.Min(isHP + amount, isMaxHP);
    }

    public virtual void DeductHealth(int amount)
    {
        if (isDead) return;
        if(!isBurning && !isGlacing && !isTwitching)//Add Effect...
        {
            HandleSlashDamage();
        }    
        //Debug.Log(amount);
        isHP -= amount;
        CheckIfDead();
    }
    protected virtual void CheckIfDead()
    {
        if (IsDead)
        {
            isDead = true;
            OnDead();
        }
    }
    public void HandleSlashDamage()
    {
        if(ObjectCtrl!=null)
            ObjectCtrl.AbstractModel.DameFlash.CallDamageFlash();
    }
    public abstract void OnDead();

}