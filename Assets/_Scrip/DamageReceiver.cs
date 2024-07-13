using System;
using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public abstract class DamageReceiver : SaiMonoBehaviour
{
    [Header("Damage Receiver")]
    [SerializeField] public int isHP = 1;
    [SerializeField] protected int isMaxHP = 3;
    [SerializeField] public int isMana = 1;
    [SerializeField] protected int isMaxMana = 3;
    public bool isDead = false;

    public int IsHP => isHP;
    public int IsMaxHP => isMaxHP;

    public int IsMana => isMana;
    public int IsMaxMana => isMaxMana;
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
        this.isMana = this.isMaxMana;
        this.isDead = false;
    }
    public virtual void ReBornHPCastleEvent()
    {
        this.isHP = this.isMaxHP;
        this.isDead = false;
    }
    public virtual void deDuctHP(int Deduct)
    {
        deDuctHP(Deduct, false);
        CreateTextDamageFX(transform.position, Deduct);
    }
    public void deDuctMana(int deduct)
    {
        if (this.transform.parent.GetComponent<PlayerCtrl>() == null) return;
        this.transform.parent.GetComponent<PlayerCtrl>().EnableCanvas(true);
        this.transform.parent.GetComponent<PlayerCtrl>().UpdateHealhbar(this.isHP);

        this.isMana -= deduct;
    }
    public virtual void deDuctHP(int Deduct,bool damageByPlayer) // có phải player gây damage không ? , sau này sẽ thay đổi tên biến và kiểu dữ liệu này 
    {
        Debug.Log("Deduct By Player",this.transform);
        if (!damageByPlayer)
        {

            if (this.transform.parent.GetComponent<EnemyCtrl>() != null)
            {

                this.transform.parent.GetComponent<EnemyCtrl>().EnableCanvas(true);
                this.transform.parent.GetComponent<EnemyCtrl>().UpdateHealhbar(this.isHP);
            }
        }
        else
        {
            if (this.transform.parent.GetComponent<PlayerCtrl>() != null)
            {
                this.transform.parent.GetComponent<PlayerCtrl>().EnableCanvas(true);
                this.transform.parent.GetComponent<PlayerCtrl>().UpdateHealhbar(this.isHP);
            }
        }
        Debug.Log("ISDEAD" + isDead);
        
        if (this.isDead == true) return;
        this.isHP -= Deduct;

        Debug.Log("DEDUCT" + Deduct);

        if (this.isHP < 0)
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

    protected virtual void CreateTextDamageFX(Vector3 hitPos,int dame)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.SetDamage(dame);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
}
