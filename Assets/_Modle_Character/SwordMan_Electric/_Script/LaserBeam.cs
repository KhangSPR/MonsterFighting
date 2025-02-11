using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.LaserBeam, objectCtrl.TargetSkill.position, Quaternion.identity);

        LaserBeamCtrl iskill = newFXSkill.GetComponent<LaserBeamCtrl>();

        if (iskill == null) return;
        iskill.SetObjectCtrl(objectCtrl);
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
