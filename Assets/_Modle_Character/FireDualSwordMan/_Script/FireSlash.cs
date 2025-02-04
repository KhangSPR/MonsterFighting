using Unity.Mathematics;
using UnityEngine;

public class FireSlash : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.FireDual, objectCtrl.TargetSpawn.position, quaternion.identity);

        FireSlashCtrl iskill = newFXSkill.GetComponent<FireSlashCtrl>();

        if (iskill == null) return;


        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
