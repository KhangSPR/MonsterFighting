using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSender : DamageSender
{
    [SerializeField] protected EnemyCtrl enemyCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadBulletCtrl();
    }
    protected virtual void loadBulletCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = transform.parent.GetComponent<EnemyCtrl>();
        Debug.Log(gameObject.name + ": loadDamageSender" + gameObject);
    }
    //public override void Send(DamageReceiver receiver)
    //{
    //    base.Send(receiver);
    //    //this.desTroyOBJ();

    //}
    //public virtual void desTroyOBJ()
    //{
    //    //this.enemyCtrl.BulletDespawn.deSpawnObj();
    //}
}
