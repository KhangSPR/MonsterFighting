using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxDamageSender : DamageSender
{
    [SerializeField] protected BulletCtrl bulletCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadBulletCtrl();
    }
    protected virtual void loadBulletCtrl()
    {
        if (this.bulletCtrl != null) return;
        this.bulletCtrl = transform.parent.GetComponent<BulletCtrl>();
        Debug.Log(gameObject.name + ": loadDamageSender" + gameObject);
    }
    //public override void Send(DamageReceiver receiver)
    //{
    //    base.Send(receiver);
    //    this.desTroyOBJ();

    //}
    //public virtual void desTroyOBJ()
    //{
    //    this.bulletCtrl.BulletDespawn.deSpawnObjParent();
    //}
}
