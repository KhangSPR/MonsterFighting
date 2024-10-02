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

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageSender();
        this.LoadFXDeSpawn();
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

    public void SetObjectCtrl(ObjectCtrl objectCtrl)
    {
        this.objectCtrl = objectCtrl;
    }
    public abstract void SkillColider(ObjectCtrl objectCtrl);
    public abstract void SkillAction();
}
