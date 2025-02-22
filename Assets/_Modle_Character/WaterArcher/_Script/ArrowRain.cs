using UnityEngine;

public class ArrowRain : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        //FX Spawn -> Damage -> CountDown

        // Convert Transform to Vector3 by using .position
        Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.ArrowRain, Vector3.zero, Quaternion.identity);

        if (objectCtrl.ObjAttack.ListObjAttacks.Count > 0)
        {
            // Lấy vị trí hiện tại của newFXSkill và cập nhật giá trị x, y
            Vector3 newPosition = newFXSkill.position;
            newPosition.x = objectCtrl.ObjAttack.ListObjAttacks[0].position.x - 0.5f;
            newPosition.y = 3.5f;
            newFXSkill.position = newPosition; // Gán lại toàn bộ vị trí

            // Sửa lỗi CS1612: Gán lại toàn bộ Quaternion thay vì chỉnh sửa y riêng lẻ
            newFXSkill.rotation = Quaternion.Euler(-23f, 0, 0);
        }
        else
        {
            if (objectCtrl.ObjAttack.PreviousTransfrom != null)
            {
                Vector3 newPosition = newFXSkill.position;
                newPosition.x = objectCtrl.ObjAttack.PreviousTransfrom.position.x - 0.5f; // Sửa lỗi thiếu .position
                newPosition.y = 3.5f;
                newFXSkill.position = newPosition;

                // Sửa lỗi CS1612
                newFXSkill.rotation = Quaternion.Euler(-23f,0, 0);
            }
        }

        ArrowRainCtrl iskill = newFXSkill.GetComponent<ArrowRainCtrl>();

        if (iskill == null) return;
        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.damageHit = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
