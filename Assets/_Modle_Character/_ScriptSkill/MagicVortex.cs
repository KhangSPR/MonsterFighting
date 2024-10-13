using System.Collections;
using UnityEngine;

public class MagicVortex : ISkill
{
    private const int MaxTargets = 5;

    public void ExecuteSkill(ObjectCtrl objectCtrl, float damage)
    {
        PlayerCtrl playerCtrl = (PlayerCtrl)objectCtrl;
        var canAttackList = playerCtrl.PlayerAttack.CanAttack;
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

            if (targetSkill.listSkillCtrl.Count > 1)
                targetSkill.StartCoroutine(WaitForSkillCompletion(targetSkill));

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
        MagicVortexCtrl firstSkillCtrl = targetSkill.listSkillCtrl[0] as MagicVortexCtrl;

        yield return new WaitUntil(() => firstSkillCtrl.IsSkillActionComplete);

        firstSkillCtrl.FxDespawn.ResetCanDespawnFlag();

        Debug.Log("WaitForSkillCompletion");
    }
}
