using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxDamageSender : DamageSender
{
    public override void Send(DamageReceiver receiver)
    {
        base.Send(receiver);
        //this.desTroyOBJ();

    }
    //public virtual void desTroyOBJ()
    //{
    //    this.bulletCtrl.BulletDespawn.deSpawnObjParent();
    //}
}
