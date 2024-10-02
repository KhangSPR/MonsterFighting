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

            // Lấy TargetSkill một lần duy nhất
            TargetSkill targetSkill = objEnemy.GetComponentInChildren<TargetSkill>();
            if (targetSkill == null || targetSkill.IsSkill)
            {
                continue; 
            }

            // Spawn new FXSkill
            Transform newFXSkill = FXSpawner.Instance.Spawn(FXSpawner.MagicVortex, objectCtrl.BulletShooter.GunPoint.position, Quaternion.Euler(-105f, 0f, 0f));

            MagicVortexCtrl iskill = newFXSkill.GetComponent<MagicVortexCtrl>();
            if (iskill != null)
            {
                // Gán damage và target cho skill
                iskill.DamageSender.Damage = (int)damage;
                iskill.targetBottom = targetSkill.transform;
                iskill.SetObjectCtrl(targetSkill.EnemyCtrl);

                // Kích hoạt FXSkill và thực hiện hành động skill
                newFXSkill.gameObject.SetActive(true);
                iskill.SkillAction();
            }

            targetSkill.IsSkill = true;
        }
    }
}
