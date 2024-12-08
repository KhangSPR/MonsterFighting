using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicVortex : ISkill
{
    private const int MaxTargets = 5;

    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        PlayerCtrl playerCtrl = (PlayerCtrl)objectCtrl;
        var canAttackList = playerCtrl.PlayerAttack.ListObjAttacks;
        int count = Mathf.Min(MaxTargets, canAttackList.Count);

        for (int i = 0; i < count; i++)
        {
            Transform objEnemy = canAttackList[i];
            ObjectCtrl objCtrl = objEnemy.GetComponent<ObjectCtrl>();

            if (objCtrl == null || objCtrl.ObjectDamageReceiver.IsDead)
                continue;

            // Spawn FXSkill
            Transform newFXSkill = FXSpawner.Instance.Spawn(
                FXSpawner.MagicVortex,
                objectCtrl.BulletShooter.GunPoint.position,
                Quaternion.Euler(-105f, 0f, 0f)
            );

            newFXSkill.gameObject.SetActive(true);

            MagicVortexCtrl iskill = newFXSkill.GetComponent<MagicVortexCtrl>();
            TargetSkill targetSkill = objEnemy.GetComponentInChildren<TargetSkill>();

            if (targetSkill.listSkillCtrl.Any(skill => skill is MagicVortexCtrl))
            {
                CancelExistingMagicVortex(targetSkill.listSkillCtrl);
            }
            else
            {
                targetSkill.listSkillCtrl.Add(iskill);
            }


            if (iskill != null)
            {
                iskill.DamageSender.Damage = (int)damage;
                iskill.targetBottom = targetSkill.transform;
                iskill.SetObjectCtrl(targetSkill.EnemyCtrl);
                iskill.SkillAction();
            }
        }
    }
    public void CancelExistingMagicVortex(List<SkillCtrl> listSkillCtrl)
    {
        // Lặp qua danh sách để tìm và xóa tất cả các instance của MagicVortexCtrl
        for (int i = listSkillCtrl.Count - 1; i >= 0; i--) // Duyệt ngược để tránh lỗi khi xóa phần tử
        {
            if (listSkillCtrl[i] is MagicVortexCtrl)
            {
                listSkillCtrl.RemoveAt(i); // Xóa phần tử nếu là MagicVortexCtrl
                //listSkillCtrl[i].gameObject.SetActive(false);
            }
        }
    }


}
