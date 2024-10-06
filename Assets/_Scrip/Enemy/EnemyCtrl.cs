using System.Collections;
using UnityEngine;

public class EnemyCtrl : ObjectCtrl
{
    [Header("Enemy Ctrl")]
    [SerializeField] protected EnemyAttack enemyAttack;
    public EnemyAttack EnemyAttack => enemyAttack;
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
    [SerializeField] protected TargetSkill targetSkill;
    public TargetSkill TargetSkill => targetSkill;
    protected override string GetObjectTypeString()
    {
        return ObjectType.Enemy.ToString();
    }
    public virtual void Init(Vector3Int cellPos)
    {
        cellPosition = cellPos;
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        this.abstractModel.SetOnDeadAnimation();
        this.enemyModel.EffectCharacter.SetVFX_Dissolve(false);
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadEnemyAttack();
        this.loadEnemyModel();
        this.loadObjMovement();
        this.LoadEnemyDropItem();
        this.loadObjAppearBigger();
        this.loadTargetSkill();
    }
    protected virtual void loadObjAppearBigger()
    {
        if (this.objAppearBigger != null) return;
        this.objAppearBigger = transform.GetComponentInChildren<ObjAppearBigger>();
        Debug.Log(gameObject.name + ": loadObjAppearBigger" + gameObject);
    }
    protected virtual void loadTargetSkill()
    {
        if (this.targetSkill != null) return;
        this.targetSkill = transform.GetComponentInChildren<TargetSkill>();
        Debug.Log(gameObject.name + ": loadTargetSkill" + gameObject);
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
    protected virtual void LoadEnemyDropItem()
    {
        if (this.enemyDropItem != null) return;
        this.enemyDropItem = transform.GetComponentInChildren<EnemyDropItem>();
        Debug.Log(gameObject.name + ": LoadEnemyDropItem" + gameObject);
    }

}
