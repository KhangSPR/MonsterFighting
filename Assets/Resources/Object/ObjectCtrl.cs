using System;
using System.Collections;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ObjectCtrl : SaiMonoBehaviour
{
    [Header("Object Ctrl")]
    [SerializeField] protected Transform modle;
    public Transform Modle { get => modle; }
    [SerializeField] protected ObjectDamageReceiver Receiver;
    public ObjectDamageReceiver ObjectDamageReceiver => Receiver;
    [SerializeField] protected Spawner spawner; 
    public Spawner Spawner => spawner;

    [SerializeField] protected DamageSender damageSender;
    public DamageSender DamageSender { get => damageSender; }
    [SerializeField] protected Despawn despawn;
    public Despawn Despawn { get => despawn; }
    [SerializeField] protected AbstractModel abstractModel;
    public AbstractModel AbstractModel { get => abstractModel; }
    [SerializeField] protected ObjMana objMana;
    public ObjMana ObjMana => objMana;

    [SerializeField] protected ObjRageSkill objRageSkill;
    public ObjRageSkill ObjRageSkill => objRageSkill;

    [SerializeField] protected BulletShooter bulletShooter;

    public BulletShooter BulletShooter => bulletShooter;

    [SerializeField] protected Transform targetSkill;
    public Transform TargetSkill => targetSkill;
    [SerializeField] protected ObjMelee objMelee;
    public ObjMelee ObjMelee => objMelee;
    [SerializeField] protected ObjAttack objAttack;
    public ObjAttack ObjAttack => objAttack;
    [SerializeField] protected ObjLand objLand;
    public ObjLand ObjLand => objLand;
    [SerializeField] protected Transform targetPostion;
    public Transform TargetPosition => targetPostion;

    [SerializeField] protected ObjDetectAllies objDetectAllies;
    public ObjDetectAllies ObjDetectAllies => objDetectAllies;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.loadShootAbleObjectSO();
        this.loadModle();
        this.loadReceiver();
        this.LoadSpawner();
        this.loadAnimationDameSender();
        this.loadDespawn();
        this.loadAbstractModel();
        this.loadObjMana();
        this.loadObjRageSkill();
        this.LoadBulletShooter();
        this.LoadTransformSkill();
        this.LoadTObjMelee();
        this.LoadObjAttack();
        this.LoadObjLand();
        this.LoadTargetBullet();
        this.LoadTargetSkill();
        this.LoadObjDetectAllies();
    }
    protected virtual void LoadObjDetectAllies()
    {
        if (objDetectAllies != null) return;
        this.objDetectAllies = transform.GetComponentInChildren<ObjDetectAllies>();
        Debug.Log(gameObject.name + ": LoadObjDetectAllies" + gameObject);
    }
    protected virtual void LoadTargetSkill()
    {
        if (targetSkill != null) return;
        targetSkill = transform.Find("TargetSkill");
        Debug.Log(gameObject.name + ": LoadTargetSkill" + gameObject);
    }
    protected virtual void LoadTargetBullet()
    {
        if (targetPostion != null) return;
        targetPostion = transform.Find("Modle/TargetBullet");
        Debug.Log(gameObject.name + ": LoadTargetBullet" + gameObject);
    }
    protected virtual void LoadObjLand()
    {
        if (this.objLand != null) return;
        this.objLand = transform.GetComponentInChildren<ObjLand>();
        Debug.Log(gameObject.name + ": LoadObjLand" + gameObject);
    }
    protected virtual void LoadObjAttack()
    {
        if (this.objAttack != null) return;
        this.objAttack = transform.GetComponentInChildren<ObjAttack>();
        Debug.Log(gameObject.name + ": LoadObjAttack" + gameObject);
    }
    protected virtual void LoadTObjMelee()
    {
        if (this.objMelee != null) return;
        this.objMelee = transform.GetComponentInChildren<ObjMelee>();
        Debug.Log(gameObject.name + ": LoadTObjMelee" + gameObject);
    }
    protected virtual void LoadTransformSkill()
    {
        if (targetSkill != null) return;
        this.targetSkill = transform.Find("TargetSkill");
        Debug.Log(gameObject.name + ": LoadTransformSkill" + gameObject);
    }
    protected virtual void LoadBulletShooter()
    {
        if (this.bulletShooter != null) return;
        this.bulletShooter = transform.GetComponentInChildren<BulletShooter>();
        Debug.Log(gameObject.name + ": loadPCanAttackEnemy" + gameObject);
    }
    protected virtual void loadObjRageSkill()
    {
        if (this.objRageSkill != null) return;
        this.objRageSkill = transform.GetComponentInChildren<ObjRageSkill>();
        Debug.Log(gameObject.name + ": loadObjRageSkill" + gameObject);
    }
    protected virtual void loadObjMana()
    {
        if (this.objMana != null) return;
        this.objMana = transform.GetComponentInChildren<ObjMana>();
        Debug.Log(gameObject.name + ": loadObjMana" + gameObject);
    }
    protected virtual void loadAbstractModel()
    {
        if (this.abstractModel != null) return;
        this.abstractModel = transform.GetComponentInChildren<AbstractModel>();
        Debug.Log(gameObject.name + ": loadAbstractModel" + gameObject);
    }
    protected virtual void loadDespawn()
    {
        if (this.despawn != null) return;
        this.despawn = transform.GetComponentInChildren<Despawn>();
        Debug.Log(gameObject.name + ": loadDespawn" + gameObject);
    }
    protected virtual void loadAnimationDameSender()
    {
        if (this.damageSender != null) return;
        this.damageSender = transform.GetComponentInChildren<DamageSender>();
        Debug.Log(gameObject.name + ": loadDamageSender" + gameObject);
    }
    protected virtual void LoadSpawner()
    {
        if (this.spawner != null) return;
        this.spawner = transform.parent?.parent?.GetComponent<Spawner>();
        Debug.LogWarning(transform.name + ": LoadSpawner", gameObject);
    }
    protected virtual void loadReceiver()
    {
        if (this.Receiver != null) return;
        this.Receiver = transform.Find("Modle").GetComponentInChildren<ObjectDamageReceiver>();
        Debug.Log(gameObject.name + ": loadloadReceiver" + gameObject);
    }
    protected virtual void loadModle()
    {
        if (this.modle != null) return;
        this.modle = transform.Find("Modle");
        Debug.Log(gameObject.name + ": loadModle" + gameObject);
    }
    //protected virtual void loadShootAbleObjectSO() // ScriptableObject
    //{
    //    if (this.objStats != null) return;
    //    string resPath = "Object/" + this.GetObjectTypeString() + "/" + transform.name;
    //    this.objStats = Resources.Load<ObjectStats>(resPath); //Ph?i t?o Folder là Resources
    //    Debug.LogWarning(transform.name + ": LoadShootAbleObjectSO" + resPath, gameObject);
    //}
    protected abstract string GetObjectTypeString();
}
