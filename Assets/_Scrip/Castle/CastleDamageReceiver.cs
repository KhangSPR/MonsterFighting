using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDamageReceiver : DamageReceiver
{
    [Header("Castle")]
    [SerializeField] protected CastleCtrl castleCtrl;
    public CastleCtrl CastleCtrl => castleCtrl;
    [SerializeField] protected BoxCollider2D boxCollider;
    public BoxCollider2D BoxCollider { get { return boxCollider; } set { boxCollider = value; } }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCastleCtrl();
        this.loadBoxCollider2D();
    }
    protected virtual void loadCastleCtrl()
    {
        if (this.castleCtrl != null) return;
        this.castleCtrl = transform.parent.GetComponent<CastleCtrl>();
        Debug.Log(gameObject.name + ": loadCastleCtrl" + gameObject);
    }
    protected virtual void loadBoxCollider2D()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = transform.GetComponent<BoxCollider2D>();
        Debug.Log(gameObject.name + ": loadBoxCollider2D" + gameObject);
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
        //this.castleCtrl.Despawn.ResetCanDespawnFlag();

        this.ActiveObjBreak();
    }
    private void ReBornCaslte()
    {     
        if(GameManager.Instance.max_hp >0)
        {
            this.isMaxHP = GameManager.Instance.max_hp;
            this.isHP = isMaxHP;

        }
    }
    public override void DeductHealth(int Deduct, AttackType attackType)
    {
        GameManager.Instance.Castle_On_Damage(Deduct);
        base.DeductHealth(Deduct, attackType);

        Debug.Log("Call DeductHealth");
    }
    protected void ActiveObjBreak()
    {
        this.castleCtrl.Modle.gameObject.SetActive(false);
        this.castleCtrl.ObjBreak.gameObject.SetActive(true);
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
