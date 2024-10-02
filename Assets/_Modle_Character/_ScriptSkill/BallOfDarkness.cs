using Unity.Mathematics;
using UnityEngine;

public class BallOfDarkness : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.BallOfDarkness, objectCtrl.BulletShooter.GunPoint.position, quaternion.identity);

        BallOfDarknessCtrl iskill = newFXSkill.GetComponent<BallOfDarknessCtrl>();

        if (iskill == null) return;


        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);

    }
}
