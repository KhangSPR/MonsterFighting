using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamageReceiver : DamageReceiverdByType
{
    [Header("Object")]
    [SerializeField] protected ObjectCtrl ObjectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadObjctrl();
    }
    protected virtual void loadObjctrl()
    {
        if (this.ObjectCtrl != null) return;
        this.ObjectCtrl = transform.parent.GetComponent<ObjectCtrl>();
        Debug.Log(gameObject.name + ": loadjunkctrl" + gameObject);
    }
    public override void onDead()
    {
        this.ObjectCtrl.Despawn.ResetCanDespawnFlag();
    }
    public override bool IsDead()
    {
        return base.IsDead();
    }
    public override void ReBorn()
    {
        this.isMaxHP = playerCtrl?.CardCharacter.CharacterStats.Life ?? enemyCtrl?.EnemySO.basePointsLife ?? this.isMaxHP;

        base.ReBorn();
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
