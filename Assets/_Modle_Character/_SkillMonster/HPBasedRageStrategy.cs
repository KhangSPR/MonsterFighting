using UnityEngine;

public class HPBasedRageStrategy : IRageStrategy
{
    public void ActivateRage(ObjectCtrl objectCtrl, ObjRageSkill rageSkill)
    {

        // Tăng sát thương và tốc độ
        objectCtrl.DamageSender.Damage = Mathf.RoundToInt(objectCtrl.DamageSender.Damage * rageSkill.RageDamageMultiplier);

        if (objectCtrl is EnemyCtrl enemy)
        {
            enemy.ObjMovement.MoveSpeed *= rageSkill.RageSpeedMultiplier;
        }


        rageSkill.Activate();
        //..........v.v


    }
}
