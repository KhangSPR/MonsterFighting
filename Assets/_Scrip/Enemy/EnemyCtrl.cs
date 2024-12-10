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
    [SerializeField] protected EnemyDropCoin enemyDropCoin;
    public EnemyDropCoin EnemyDropCoin => enemyDropCoin;
    [SerializeField] protected ObjAppearBigger objAppearBigger;
    public ObjAppearBigger ObjAppearBigger => objAppearBigger;
    [SerializeField] protected TargetSkill targetSkillScript;
    public TargetSkill TargetSkillScript => targetSkillScript;
    [SerializeField] protected HornSpawnerCtrl enemySpawnCtrl;
    public HornSpawnerCtrl EnemySpawnCtrl => enemySpawnCtrl;
    [SerializeField] protected ObjMoveInTheCity objMoveIntheCity;
    public ObjMoveInTheCity ObjMoveIntheCity => objMoveIntheCity;
    
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

        if (this.enemySpawnCtrl != null)
        {
            OneDisableObject();
        }

        this.abstractModel.EffectCharacter.SetOrderLayerRenderer(objLand.LandIndex);

        this.targetSkillScript.listSkillCtrl.Clear();
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        if (this.enemySpawnCtrl != null)
        {
            OnEnableObject();
        }

    }
    protected void OneDisableObject()
    {
        if(this.enemySpawnCtrl.Abilities.AbilitySummon is AbilitySumonHorn abilitySumonHorn)
        {
            abilitySumonHorn.EnableObject();
        }
    }
    protected void OnEnableObject()
    {
        if(this.enemySpawnCtrl.Abilities.AbilitySummon is AbilitySumonHorn abilitySumonHorn)
        {
            abilitySumonHorn.DisableObject();
        }
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
        this.loadEnemySpawnCtrl();
        this.loadObjMoveIntheCity();
        this.loadEnemyDropCoin();
    }
    protected virtual void loadEnemyDropCoin()
    {
        if (this.enemyDropCoin != null) return;
        this.enemyDropCoin = transform.GetComponentInChildren<EnemyDropCoin>();
        Debug.Log(gameObject.name + ": loadEnemyDropCoin" + gameObject);
    }
    protected virtual void loadObjMoveIntheCity()
    {
        if (this.objMoveIntheCity != null) return;
        this.objMoveIntheCity = transform.GetComponentInChildren<ObjMoveInTheCity>();
        Debug.Log(gameObject.name + ": loadObjMoveIntheCity" + gameObject);
    }
    protected virtual void loadEnemySpawnCtrl()
    {
        if (this.enemySpawnCtrl != null) return;
        this.enemySpawnCtrl = transform.GetComponentInChildren<HornSpawnerCtrl>();
        Debug.Log(gameObject.name + ": loadEnemySpawnCtrl" + gameObject);
    }
    protected virtual void loadObjAppearBigger()
    {
        if (this.objAppearBigger != null) return;
        this.objAppearBigger = transform.GetComponentInChildren<ObjAppearBigger>();
        Debug.Log(gameObject.name + ": loadObjAppearBigger" + gameObject);
    }
    protected virtual void loadTargetSkill()
    {
        if (this.targetSkillScript != null) return;
        this.targetSkillScript = transform.GetComponentInChildren<TargetSkill>();
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
        float distanceAttack1 = skill1 != null ? skill1.distanceAttack : 0f;
        ISkill classSkill1 = skill1 != null ? skill1.GetSkillInstance() : null;

        // Skill 2
        SkillSO skill2 = enemySO.skill2;
        float manaSkill2 = skill2 != null ? skill2.manaRequirement : 0f;
        bool lockSkill2 = skill2 != null ? skill2.skillUnlock : false;
        float dmg2 = skill2 != null ? skill2.damage : 0f;
        float distanceAttack2 = skill2 != null ? skill2.distanceAttack : 0f;
        ISkill classSkill2 = lockSkill2 && skill2 != null ? skill2.GetSkillInstance() : null;

        // Call Function SetSkill for AbstractModel
        this.abstractModel.SetSkill(manaSkill1, lockSkill1, dmg1, classSkill1, distanceAttack1, manaSkill2, lockSkill2, dmg2, classSkill2, distanceAttack2);

        Debug.Log("Set Skill 1 Lan EnemyCtrl");
    }
}
