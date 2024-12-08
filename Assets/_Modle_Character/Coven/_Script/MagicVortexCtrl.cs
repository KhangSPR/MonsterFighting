using System.Collections;
using System.Collections.Generic;
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
        base.Update();

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

        DamageSender.SendFXImpact(damageReceiver, objectCtrl);
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

        if (enemyCtrl != null && enemyCtrl.TargetSkillScript != null)
        {
            RemoveMagicVortexCtrl(enemyCtrl.TargetSkillScript.listSkillCtrl);


            if (enemyCtrl.EnemyAttack.ListObjAttacks.Count <= 0 && enemyCtrl.TargetSkillScript.listSkillCtrl.Count <= 0)
                enemyCtrl.EnemyAttack.CheckCanAttack = false;
        }
    }
    public void RemoveMagicVortexCtrl(List<SkillCtrl> listSkillCtrl)
    {
        // Duyệt qua danh sách để tìm instance của MagicVortexCtrl
        for (int i = 0; i < listSkillCtrl.Count; i++)
        {
            if (listSkillCtrl[i] is MagicVortexCtrl magicVortexCtrl && this == magicVortexCtrl)
            {
                listSkillCtrl.RemoveAt(i); // Xóa phần tử tại vị trí tìm được
                return; // Thoát khỏi hàm sau khi xóa
            }
        }
    }

}
