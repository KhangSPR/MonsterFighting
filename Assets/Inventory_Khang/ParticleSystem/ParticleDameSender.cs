using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ParticleDameSender : DamageSender
{
    public override void Send(Transform obj)
    {
        base.Send(obj);
        Vector3 hitPos = obj.position;
        Quaternion hitRot = obj.rotation;
        this.CreateTextDamageFX(hitPos);
    }
    public override void Send(DamageReceiver receiver)
    {
        base.Send(receiver);

        Debug.Log("Send");

        //this.CreateImpactFX(hitPos, hitRot);

    }
    protected virtual void CreateTextDamageFX(Vector3 hitPos)
    {
        string damageNumber = LargeNumber.ToString(this.Damage); ;

        Debug.Log("Create Text FX");
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(damageNumber, skillType);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
}
