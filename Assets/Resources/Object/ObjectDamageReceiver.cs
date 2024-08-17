// Object damage receiver class
using UnityEngine;

public class ObjectDamageReceiver : DamageReceiverByType
{
    public override void OnDead()
    {
        base.OnDead();
        if (ObjectCtrl != null)
        {
            ObjectCtrl.Despawn.ResetCanDespawnFlag();
            ObjectCtrl.AbstractModel.DameFlash.StopCoroutieSlash();
        }
        else
        {
            Debug.LogError("ObjectCtrl is not assigned for " + gameObject.name);
        }
    }  
    public override void ReBorn()
    {
        // Ensure the ObjectCtrl and its dependencies are loaded first
        LoadObjectCtrl();

        if (ObjectCtrl != null)
        {
            isMaxHP = PlayerCtrl?.CardCharacter.CharacterStats.Life
                      ?? EnemyCtrl?.EnemySO.basePointsLife
                      ?? isMaxHP;
        }

        base.ReBorn();
    }
}