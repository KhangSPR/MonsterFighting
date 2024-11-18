using System;
using System.Collections;
using UIGameDataManager;
using UnityEngine;


// Base class for damage receivers
public abstract class DamageReceiver : ObjModleAbstact
{
    [Header("Damage Receiver")]
    [SerializeField] protected int isHP = 1;
    public int IsHP => isHP;
    [SerializeField] protected int isMaxHP = 3;
    public int IsMaxHP => isMaxHP;
    [SerializeField]
    protected bool isBurning;
    [SerializeField]
    protected bool isTwitching;
    [SerializeField]
    protected bool isGlacing;
    [SerializeField]
    protected bool isPoition;
    [SerializeField]
    protected bool isDarking;
    [SerializeField]
    protected bool isStun;
    [SerializeField]
    protected bool isDead;
    public bool IsDead => isHP <= 0;


    //Event
    public event Action OnTakeDamage;
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

        // Tăng HP, nhưng không vượt quá giá trị isMaxHP
        isHP = Mathf.Min(isHP + amount, isMaxHP);
    }

    public virtual void DeductHealth(int amount, AttackType attackType)
    {

        Debug.Log("DeductHealth + " + transform.parent.name);

        if (isDead) return;
        if(!isBurning && !isGlacing && !isTwitching && !isPoition)//Add Effect...
        {
            HandleSlashDamage();


            Debug.Log("Dame Flash Of: " + transform.parent.name);
        }    
        //Debug.Log(amount);
        isHP -= amount;

        if(attackType == AttackType.Default)
            OnTakeDamage?.Invoke();

        if(abstractModel!= null)
        {
            if(this.abstractModel.AttackTypeAnimation == AttackTypeAnimation.Deff)
            {
                if (!this.abstractModel.IsHit)
                    this.abstractModel.IsHit = true;
            }
        }

        CheckIfDead();
    }
    protected virtual void CheckIfDead()
    {
        Debug.Log("CheckIfDead Is Fasle");

        if (IsDead)
        {
            Debug.Log("CheckIfDead Is True");

            isDead = true;
            OnDead();
            //if (damageByPlayer)
            //{
            //    Debug.Log($"Enemy {this.transform.name} Dead");
            //    if (GameDataManager.Instance.currentMapSO.GetStarsCondition(GameDataManager.Instance.currentMapSO.difficult).GetType() == typeof(KillEnemyCondition))
            //    {
            //        Debug.Log($"Active Kill Enemy Condition");
            //        KillEnemyCondition killEnemyCondition = GameDataManager.Instance.currentMapSO.GetStarsCondition(GameDataManager.Instance.currentMapSO.difficult) as KillEnemyCondition;
            //        killEnemyCondition.enemyKill++;
            //        killEnemyCondition.currentThreshold = killEnemyCondition.enemyKill;
            //    }
            //}
        }
    }
    public void HandleSlashDamage()
    {
        if(AbstractModel !=null)
            AbstractModel.DameFlash.CallDamageFlash();
    }
    public abstract void OnDead();

}