using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCtrl : SaiMonoBehaviour
{
    //Obj Spawn Skill
    [SerializeField]
    protected ObjectCtrl objectCtrl;

    public ObjectCtrl ObjectCtrl => objectCtrl;

    [SerializeField] protected DamageSender damageSender;
    public DamageSender DamageSender => damageSender;
    [SerializeField] protected FxDespawn fxDespawn;
    public FxDespawn FxDespawn => fxDespawn;
    [SerializeField] protected FXDamageReceiver fxDamageReceiver;
    public FXDamageReceiver FXDamageReceiver => fxDamageReceiver;
    [SerializeField] protected FXImpact fXImpact;
    public FXImpact FXImpact => fXImpact;
    protected override void Update()
    {
        base.Update();

        if (objectCtrl?.gameObject.activeSelf == false)
        {
            PlayerSpawner.Instance.Despawn(objectCtrl.transform);
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageSender();
        this.LoadFXDeSpawn();
        this.LoadFXDamageReceiver();
        this.LoadFXImpact();
    }
    protected void LoadFXImpact()
    {
        if (this.fXImpact != null) return;
        this.fXImpact = transform.GetComponentInChildren<FXImpact>();

        Debug.Log("LoadFXImpact");
    }
    protected void LoadFXDeSpawn()
    {
        if (this.fxDespawn != null) return;
        this.fxDespawn = transform.GetComponentInChildren<FxDespawn>();

        Debug.Log("LoadFXDeSpawn");
    }
    protected void LoadDamageSender()
    {
        if (this.damageSender != null) return;
        this.damageSender = transform.GetComponentInChildren<DamageSender>();

        Debug.Log("LoadDamage Sender");
    }
    protected void LoadFXDamageReceiver()
    {
        if (this.fxDamageReceiver != null) return;
        this.fxDamageReceiver = transform.GetComponentInChildren<FXDamageReceiver>();

        Debug.Log("LoadFXDamageReceiver");
    }

    public void SetObjectCtrl(ObjectCtrl objectCtrl)
    {
        this.objectCtrl = objectCtrl;
    }
    public abstract void SkillColider(ObjectCtrl objectCtrl);
    public virtual void SkillColliderCastle(CastleCtrl castleCtrl)
    {

    }
    public abstract void SkillAction();
}
