using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDameSender : DamageSender
{
    
    //public override void Send(DamageReceiver receiver)
    //{
    //    base.Send(receiver);
    //    //this.desTroyOBJ();
    //}
    //public virtual void desTroyOBJ()
    //{
    //    this.playerCtrl.Despawn.HidepawnObjChild();
    //}
    public override void Send(DamageReceiver receiver)
    {
        base.Send(receiver);
        Vector3 hitPos = transform.position;
        Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextDamageFX(hitPos);

        //this.desTroyOBJ();
    }
    protected virtual void CreateTextDamageFX(Vector3 hitPos)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(this.Damage,skillType);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
}
