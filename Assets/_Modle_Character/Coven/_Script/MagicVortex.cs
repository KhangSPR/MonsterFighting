using System.Collections;
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

            targetSkill.listSkillCtrl.Add(iskill);

            //int countTarget = 0;
            //for (int j = 0; i < targetSkill.listSkillCtrl.Count; j++)
            //{
            //    if (targetSkill.listSkillCtrl[i] as MagicVortexCtrl)
            //    {
            //        countTarget += 1;
            //    }
            //}

            //if (countTarget > 1)
            //    targetSkill.StartCoroutine(WaitForSkillCompletion(targetSkill));

            if (iskill != null)
            {
                iskill.DamageSender.Damage = (int)damage;
                iskill.targetBottom = targetSkill.transform;
                iskill.SetObjectCtrl(targetSkill.EnemyCtrl);
                iskill.SkillAction();
            }
        }
    }

    private IEnumerator WaitForSkillCompletion(TargetSkill targetSkill)
    {
        if (targetSkill.listSkillCtrl == null || targetSkill.listSkillCtrl.Count == 0)
        {
            Debug.LogWarning("TargetSkill does not have any skills in listSkillCtrl.");
            yield break; // Kết thúc coroutine nếu danh sách trống
        }

        MagicVortexCtrl firstSkillCtrl = null;
        for (int i = 0; i< targetSkill.listSkillCtrl.Count; i++)
        {
            if (targetSkill.listSkillCtrl[i] as MagicVortexCtrl)
            {
                firstSkillCtrl = targetSkill.listSkillCtrl[i] as MagicVortexCtrl;

                break;
            }
        }
        if(firstSkillCtrl == null)
        {
            Debug.LogWarning("firstSkillCtrl is null.");
            yield break;
        }

        yield return new WaitUntil(() => firstSkillCtrl.IsSkillActionComplete);

        if (firstSkillCtrl.FxDespawn != null)
        {
            firstSkillCtrl.FxDespawn.ResetCanDespawnFlag();
        }
        else
        {
            Debug.LogWarning("FxDespawn is null in firstSkillCtrl.");
        }

    }

}
