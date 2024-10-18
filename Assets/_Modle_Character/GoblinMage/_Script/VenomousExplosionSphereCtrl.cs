using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomousExplosionSphereCtrl : SkillCtrl
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


        //Action Stun 
        objectCtrl.ObjectDamageReceiver.StartStun();
        objectCtrl.ObjectDamageReceiver.StartPotioning(this.damageSender.SkillType);

        //Add Skill
        this.DamageSender.SendFXImpact(damageReceiver);
    }
    #region FX_StartPotioning_Coroutine

    public void StartPotioning(int damagePerSecond)
    {
    }

    public void StopPotioning()
    {
    }
    #endregion
}
