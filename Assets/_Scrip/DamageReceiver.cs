using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageReceiver : SaiMonoBehaviour
{
    [Header("Damage Receiver")]
    [SerializeField] protected int isHP = 1;
    [SerializeField] protected int isMaxHP = 3;
    public bool isDead = false;

    public int IsHP => isHP;
    public int IsMaxHP => isMaxHP;
    protected override void OnEnable() // Goi 1 lan moi khi reset
    {
        this.ReBorn();
    }
    protected override void loadValue()
    {
        base.loadValue();
        this.ReBorn();
    }
    public virtual void ReBorn()
    {
        this.isHP = this.isMaxHP;
        this.isDead = false;
    }
    public virtual void ReBornHPCastleEvent()
    {
        this.isHP = this.isMaxHP;
        this.isDead = false;
    }
    protected virtual void Add(int ADD)
    {
        if (this.isDead == true) return;
        this.isHP += ADD;
        if (this.isHP > this.isMaxHP) this.isHP = this.isMaxHP;
    }
    public virtual void deDuct(int Deduct)
    {
        if (this.isDead == true) return;
        this.isHP -= Deduct;
        if (this.isMaxHP < 0)
            this.isHP = 0;
        this.checkDead();
    }
    public virtual bool LoseHealth(int Deduct)
    {
        //health = health - amount
        this.isHP -= Deduct;

        if (this.isHP <= 0)
        {
            this.checkDead();
            return true;
        }
        return false;
    }

    public virtual bool IsDead()
    {
        return this.isHP <= 0;
    }
    protected virtual void checkDead()
    {
        if (!this.IsDead()) return;
        this.isDead = true;
        this.onDead();
    }
    public abstract void onDead();
}
