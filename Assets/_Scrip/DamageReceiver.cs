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


    protected bool isDead;
    public bool IsDead => isHP <= 0;

    protected override void OnEnable()
    {
        ReBorn();
    }

    protected virtual void LoadValue()
    {
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

        //Debug.Log(amount);
        isHP -= amount;
        DameSlash();
        CheckIfDead();
    }
    protected virtual void DameSlash()
    {

    }
    protected virtual void CheckIfDead()
    {
        if (IsDead)
        {
            isDead = true;
            OnDead();
        }
    }

    public abstract void OnDead();

}