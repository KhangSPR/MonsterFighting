using System.Numerics;
using UnityEngine;

public class VenomousExplosionSphere : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.VenomousExplosionSphere, objectCtrl.TransformSkill.position, UnityEngine.Quaternion.Euler(-90f, 0f, 0f));

        VenomousExplosionSphereCtrl iskill = newFXSkill.GetComponent<VenomousExplosionSphereCtrl>();

        if (iskill == null) return;


        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
