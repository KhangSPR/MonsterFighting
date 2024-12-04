using UnityEngine;

public class VenomousExplosionSphere : ISkill
{
    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        EnemyCtrl enemyCtrl = objectCtrl as EnemyCtrl;

        Vector3 targetPosition;

        // Kiểm tra nếu danh sách đối tượng tấn công rỗng
        if (enemyCtrl.EnemyAttack.ListObjAttacks.Count <= 0)
        {
            // Lấy vị trí hiện tại của enemyCtrl nhưng không thay đổi nó
            targetPosition = enemyCtrl.transform.position;
            targetPosition.x -= 4f; // Thay đổi tọa độ x của bản sao
        }
        else
        {
            // Lấy vị trí đối tượng đầu tiên trong danh sách
            Transform targetTransform = enemyCtrl.EnemyAttack.ListObjAttacks[0];

            if (targetTransform.name == "Castle")
            {
                Transform castleTargetPosition = targetTransform.Find("TargetPosition");
                if (castleTargetPosition != null)
                {
                    targetPosition = castleTargetPosition.position;
                }
                else
                {
                    targetPosition = targetTransform.position;
                }
            }
            else
            {
                targetPosition = targetTransform.position;
            }
        }

        // Tạo hiệu ứng tại vị trí targetPosition
        Transform newFXSkill = FXSpawner.Instance.Spawn(
            FXSpawner.VenomousExplosionSphere,
            targetPosition,
            Quaternion.Euler(-90f, 0f, 0f)
        );

        // Kiểm tra và thực hiện hành động với hiệu ứng
        VenomousExplosionSphereCtrl iskill = newFXSkill.GetComponent<VenomousExplosionSphereCtrl>();

        if (iskill == null) return;

        iskill.SetObjectCtrl(objectCtrl);
        iskill.SkillAction();
        iskill.DamageSender.Damage = (int)damage;

        newFXSkill.gameObject.SetActive(true);
    }
}
