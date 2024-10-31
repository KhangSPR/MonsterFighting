using System.Collections;
using UnityEngine;

public class MagicVortexCtrl : SkillCtrl, ITrapHpSkill
{
    public float timeLockAttack;
    public float speedMove;
    public float trapHp;
    public float TrapHp => trapHp;

    public Transform targetBottom;
    private Vector3 pos;

    private bool sendDameFisrt = false;
    private EnemyCtrl enemyCtrl;

    public bool IsSkillActionComplete { get; private set; }

    protected override void Update()
    {
        if (targetBottom != null)
        {
            pos = targetBottom.position;
            transform.position = Vector3.MoveTowards(transform.position, pos, speedMove * Time.deltaTime);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopEffect();
    }

    public override void SkillAction()
    {
        StartCoroutine(DelaySkillAction());
        Debug.Log("SkillAction");
    }

    private IEnumerator DelaySkillAction()
    {
        yield return new WaitForSeconds(0.6f);

        enemyCtrl = (EnemyCtrl)objectCtrl;

        if (enemyCtrl == null)
        {
            IsSkillActionComplete = true;
            yield break;
        }

        enemyCtrl.EnemyAttack.CheckCanAttack = true;
        IsSkillActionComplete = true;
    }

    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        if (sendDameFisrt) return;

        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null || damageReceiver.IsDead)
            return;

        DamageSender.SendFXImpact(damageReceiver);
        sendDameFisrt = true;

        StartCoroutine(StopAction());
        Debug.Log("Call Skill Colider");
    }

    private IEnumerator StopAction()
    {
        yield return new WaitForSeconds(timeLockAttack);

        if (enemyCtrl == null) yield break;

        fxDespawn.ResetCanDespawnFlag();
    }

    private void StopEffect()
    {
        targetBottom = null;
        sendDameFisrt = false;

        if (enemyCtrl != null && enemyCtrl.TargetSkill != null)
        {
            enemyCtrl.TargetSkill.listSkillCtrl.RemoveAt(0);

            if (enemyCtrl.EnemyAttack.ListObjAttacks.Count <= 0 && enemyCtrl.TargetSkill.listSkillCtrl.Count <= 0)
                enemyCtrl.EnemyAttack.CheckCanAttack = false;
        }
    }
}
