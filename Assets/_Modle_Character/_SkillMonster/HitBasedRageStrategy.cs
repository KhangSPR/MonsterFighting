using UnityEngine;

public class HitBasedRageStrategy : IRageStrategy
{
    public void ActivateRage(ObjectCtrl objectCtrl, ObjRageSkill rageSkill)
    {
        // Tăng sát thương và tốc độ
        objectCtrl.DamageSender.Damage = Mathf.RoundToInt(objectCtrl.DamageSender.Damage * rageSkill.RageDamageMultiplier);

        rageSkill.Activate();
        //..........v.v
    }
}
