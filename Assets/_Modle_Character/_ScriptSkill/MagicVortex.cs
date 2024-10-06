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

            // Spawn new FXSkill
            Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.MagicVortex, objectCtrl.BulletShooter.GunPoint.position, Quaternion.Euler(-105f, 0f, 0f));

            newFXSkill.gameObject.SetActive(true);


            MagicVortexCtrl iskill = newFXSkill.GetComponent<MagicVortexCtrl>(); // Repair

            // Lấy TargetSkill một lần duy nhất
            TargetSkill targetSkill = objEnemy.GetComponentInChildren<TargetSkill>();

            targetSkill.listSkillCtrl.Add(iskill);


            if (targetSkill.listSkillCtrl.Count > 1)
            {
                //skill stack
                targetSkill.StartCoroutine(WaitForSkillCompletion(targetSkill));
            }


            if (iskill != null)
            {
                // Gán damage và target cho skill
                iskill.DamageSender.Damage = (int)damage;
                iskill.targetBottom = targetSkill.transform;
                iskill.SetObjectCtrl(targetSkill.EnemyCtrl);

                // Kích hoạt FXSkill và thực hiện hành động skill
                iskill.SkillAction();
            }
        }
    }
    private IEnumerator WaitForSkillCompletion(TargetSkill targetSkill)
    {
        MagicVortexCtrl firstSkillCtrl = (MagicVortexCtrl)targetSkill.listSkillCtrl[0];

        yield return new WaitUntil(() => firstSkillCtrl.IsSkillActionComplete);

        // Sau khi skill hoàn thành, reset despawn flag
        firstSkillCtrl.FxDespawn.ResetCanDespawnFlag();

        Debug.Log("WaitForSkillCompletion");
    }
}
