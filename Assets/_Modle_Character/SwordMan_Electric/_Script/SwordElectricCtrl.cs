using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordElectricCtrl : SkillCtrl
{
    public override void SkillAction()
    {
        
    }

    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }

        if (damageReceiver.IsDead) return;


        //Add Skill
        objectCtrl.ObjectDamageReceiver.StartStun();
        this.DamageSender.SendFXImpact(damageReceiver, objectCtrl);
    }
}
