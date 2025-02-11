using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDamageReceiver : DamageReceiver
{
    public override void OnDead()
    {
        skillCtrl.FxDespawn.ResetCanDespawnFlag();

        Debug.Log("On Dead: " + transform.parent.name);
    }
    public override void ReBorn()
    {
        ITrapHpSkill trapHpSkill = skillCtrl as ITrapHpSkill;

        if (trapHpSkill != null)
        {
            isMaxHP = (int)trapHpSkill.TrapHp;
        }
        else
        {
            Debug.LogWarning("SkillCtrl không có thuộc tính trapHp");
        }

        base.ReBorn();
    }
    public void AddHeal(int amount)
    {
        this.AddHealth(amount);
    }
}
