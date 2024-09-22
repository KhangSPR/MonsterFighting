using System.Collections;
using UnityEngine;

public class EnemyCtrl : ObjectCtrl
{
    [Header("Enemy Ctrl")]
    [SerializeField] protected EnemyAttack enemyAttack;
    public EnemyAttack EnemyAttack => enemyAttack;
    [SerializeField] protected EnemyShooter enemyShooter;

    public EnemyShooter EnemyShooter => enemyShooter;
    [SerializeField] protected EnemyModel enemyModel;
    public EnemyModel EnemyModel => EnemyModel;
    [SerializeField] protected ObjMovement objMovement;
    public ObjMovement ObjMovement => objMovement;
    private Vector3Int cellPosition;

    [SerializeField] protected EnemySO enemySO;
    public EnemySO EnemySO => enemySO;
    [SerializeField] protected EnemyDropItem enemyDropItem;
    public EnemyDropItem EnemyDropItem => enemyDropItem;
    [SerializeField] protected ObjAppearBigger objAppearBigger;
    public ObjAppearBigger ObjAppearBigger => objAppearBigger;
    protected override string GetObjectTypeString()
    {
        return ObjectType.Enemy.ToString();
    }
    public virtual void Init(Vector3Int cellPos)
    {
        cellPosition = cellPos;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyShooter();
        this.loadEnemyAttack();
        this.loadEnemyModel();
        this.loadObjMovement();
        this.LoadEnemyDropItem();
        this.loadObjAppearBigger();
    }
    protected virtual void loadObjAppearBigger()
    {
        if (this.objAppearBigger != null) return;
        this.objAppearBigger = transform.GetComponentInChildren<ObjAppearBigger>();
        Debug.Log(gameObject.name + ": loadObjAppearBigger" + gameObject);
    }
    protected virtual void loadObjMovement()
    {
        if (this.objMovement != null) return;
        this.objMovement = transform.GetComponentInChildren<ObjMovement>();
        Debug.Log(gameObject.name + ": loadObjMovement" + gameObject);
    }
    protected virtual void loadEnemyModel()
    {
        if (this.enemyModel != null) return;
        this.enemyModel = transform.GetComponentInChildren<EnemyModel>();
        Debug.Log(gameObject.name + ": loadEnemyModel" + gameObject);
    }
    protected virtual void loadEnemyAttack()
    {
        if (this.enemyAttack != null) return;
        this.enemyAttack = transform.GetComponentInChildren<EnemyAttack>();
        Debug.Log(gameObject.name + ": loadEnemyAttack" + gameObject);
    }
    protected virtual void LoadEnemyShooter()
    {
        if (this.enemyShooter != null) return;
        this.enemyShooter = transform.GetComponentInChildren<EnemyShooter>();
        Debug.Log(gameObject.name + ": loadPCanAttackEnemy" + gameObject);
    }
    protected virtual void LoadEnemyDropItem()
    {
        if (this.enemyDropItem != null) return;
        this.enemyDropItem = transform.GetComponentInChildren<EnemyDropItem>();
        Debug.Log(gameObject.name + ": LoadEnemyDropItem" + gameObject);
    }

}
