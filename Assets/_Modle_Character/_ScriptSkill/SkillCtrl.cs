using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCtrl : SaiMonoBehaviour
{
    //Obj Spawn Skill
    protected ObjectCtrl objectCtrl;

    public ObjectCtrl ObjectCtrl => objectCtrl;

    [SerializeField] protected DamageSender damageSender;
    public DamageSender DamageSender => damageSender;
    [SerializeField] protected FxDespawn fxDespawn;
    public FxDespawn FxDespawn => fxDespawn;
    [SerializeField] protected FXDamageReceiver fxDamageReceiver;
    public FXDamageReceiver FXDamageReceiver => fxDamageReceiver;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageSender();
        this.LoadFXDeSpawn();
        this.LoadFXDamageReceiver();
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
    public abstract void SkillAction();
}
