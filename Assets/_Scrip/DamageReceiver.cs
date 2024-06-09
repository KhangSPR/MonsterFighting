using System;
using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
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
        deDuct(Deduct, false);
    }

    public virtual void deDuct(int Deduct,bool damageByPlayer) // có phải player gây damage không ? , sau này sẽ thay đổi tên biến và kiểu dữ liệu này 
    {
        if (this.isDead == true) return;
        this.isHP -= Deduct;
        if (this.isMaxHP < 0)
            this.isHP = 0;

        this.checkDead(damageByPlayer);
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
        checkDead(false);
    }
    protected virtual void checkDead(bool damageByPlayer)
    {
        if (!this.IsDead()) return;
        this.isDead = true;
        this.onDead();
        if (damageByPlayer)
        {
            Debug.Log($"Enemy {this.transform.name} Dead");
            if (GameDataManager.Instance.currentMapSO.GetStarsCondition(GameDataManager.Instance.currentMapSO.difficult).GetType() == typeof(KillEnemyCondition))
            {
                Debug.Log($"Active Kill Enemy Condition");
                KillEnemyCondition killEnemyCondition = GameDataManager.Instance.currentMapSO.GetStarsCondition(GameDataManager.Instance.currentMapSO.difficult) as KillEnemyCondition;
                killEnemyCondition.enemyKill++;
                killEnemyCondition.currentThreshold = killEnemyCondition.enemyKill;
            }
        }
    }
    public abstract void onDead();
}
