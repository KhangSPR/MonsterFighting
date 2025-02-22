using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBeam : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.BulletBeam, objectCtrl.TargetSkill.position, Quaternion.identity);

        BulletBeamCtrl iskill = newFXSkill.GetComponent<BulletBeamCtrl>();

        if (iskill == null) return;
        iskill.SetObjectCtrl(objectCtrl);
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
