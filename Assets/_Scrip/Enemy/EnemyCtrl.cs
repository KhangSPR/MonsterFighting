using System.Collections;
using UIGameDataManager;
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

        if(objRageSkill != null)
             this.objRageSkill.SetRage(false);
    }
    protected override void Start()
    {
        base.Start();

        this.SetSkill(enemySO);
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
    private void SetSkill(EnemySO enemySO)
    {
        // Skill 1
        SkillSO skill1 = enemySO.skill1;
        float manaSkill1 = skill1 != null ? skill1.manaRequirement : 0f;
        float dmg1 = skill1 != null ? skill1.damage : 0f;
        bool lockSkill1 = skill1 != null ? skill1.skillUnlock : false;
        ISkill classSkill1 = skill1 != null ? skill1.GetSkillInstance() : null;

        // Skill 2
        SkillSO skill2 = enemySO.skill2;
        float manaSkill2 = skill2 != null ? skill2.manaRequirement : 0f;
        bool lockSkill2 = skill2 != null ? skill2.skillUnlock : false;
        float dmg2 = skill2 != null ? skill2.damage : 0f;
        ISkill classSkill2 = lockSkill2 && skill2 != null ? skill2.GetSkillInstance() : null;

        // Call Function SetSkill for AbstractModel
        this.abstractModel.SetSkill(manaSkill1, lockSkill1, dmg1, classSkill1, manaSkill2, lockSkill2, dmg2, classSkill2);



        Debug.Log("Set Skill 1 Lan EnemyCtrl");
    }
}
