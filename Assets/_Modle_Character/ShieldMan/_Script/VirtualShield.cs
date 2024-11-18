using UnityEngine;

public class VirtualShield : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {

        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.VirtualShield, objectCtrl.TargetSkill.position, Quaternion.identity);

        VirtualShieldCtrl iskill = newFXSkill.GetComponent<VirtualShieldCtrl>();

        if (iskill == null) return;
        iskill.SetObjectCtrl(objectCtrl);

        newFXSkill.gameObject.SetActive(true);

    }
}
