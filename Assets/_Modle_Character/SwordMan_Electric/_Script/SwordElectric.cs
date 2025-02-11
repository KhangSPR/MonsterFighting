using UnityEngine;

public class SwordElectric : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {

        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.SwordElectric, objectCtrl.TargetSpawn.position, Quaternion.identity);

        SwordElectricCtrl iskill = newFXSkill.GetComponent<SwordElectricCtrl>();

        if (iskill == null) return;
        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
