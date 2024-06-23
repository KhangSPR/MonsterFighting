using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDamageReceiver : DamageReceiver
{
    [Header("Castle")]
    [SerializeField] protected CastleCtrl castleCtrl;
    public CastleCtrl CastleCtrl => castleCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCastleCtrl();
    }
    protected virtual void loadCastleCtrl()
    {
        if (this.castleCtrl != null) return;
        this.castleCtrl = transform.parent.GetComponent<CastleCtrl>();
        Debug.Log(gameObject.name + ": loadCastleCtrl" + gameObject);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.CastleSetHpMax += ReBornHPCastleEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.CastleSetHpMax -= ReBornHPCastleEvent;
    }
    public override void onDead()
    {
        this.castleCtrl.Despawn.deSpawnObjParent();
    }
    public override bool IsDead()
    {
        return base.IsDead();
    }
    public override void ReBornHPCastleEvent()
    {     
        if(GameManager.Instance.max_hp >0)
        {
            this.isMaxHP = GameManager.Instance.max_hp;
            base.ReBornHPCastleEvent();
        }
    }
    public override void deDuctHP(int Deduct)
    {
        GameManager.Instance.Castle_On_Damage(Deduct);
        base.deDuctHP(Deduct);
    }
    #region FX On Dead -----------------------------------------------------------------------------------------

    //protected virtual void ondeaddrop()
    //{
    //    Vector3 pos = transform.position;
    //    Quaternion rot = transform.rotation;
    //    ItemDropSpawner.Instance.Drop(this.shootAbleObjectCtrl.ShootAbleObjectSO.dropList, pos, rot);
    //}

    //protected virtual void ondeadfx()
    //{
    //    string fxname = this.getondeadfxname();
    //    Transform fxondead = FXSpawner.Instance.Spawn(fxname, transform.position, transform.rotation); //ham smoke bang ten
    //    fxondead.gameObject.SetActive(true);
    //}
    //protected virtual string getondeadfxname()
    //{
    //    return FXSpawner.SmokeOne;
    //}
    #endregion
}
