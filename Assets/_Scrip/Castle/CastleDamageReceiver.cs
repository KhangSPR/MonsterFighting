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
        GameManager.CastleSetHpMax += ReBornCaslte;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.CastleSetHpMax -= ReBornCaslte;
    }
    public override void OnDead()
    {
        this.castleCtrl.Despawn.ResetCanDespawnFlag();
    }
    public void ReBornCaslte()
    {     
        if(GameManager.Instance.max_hp >0)
        {
            this.isMaxHP = GameManager.Instance.max_hp;
        }
    }
    public override void DeductHealth(int Deduct)
    {
        GameManager.Instance.Castle_On_Damage(Deduct);
        base.DeductHealth(Deduct);
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
