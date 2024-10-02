using System.Collections;
using UnityEngine;

public class MagicVortexCtrl : SkillCtrl
{
    public float timeLockAttack;
    public float speedMove;

    public Transform targetBottom;
    private Vector3 pos;

    private bool sendDameFisrt = false;

    protected override void Update()
    {
        FollowTargetSkill(); // Gọi hàm để theo dõi mục tiêu
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        StopEffect();
    }
    private void FollowTargetSkill()
    {
        // Kiểm tra xem targetBottom có giá trị không
        if (targetBottom != null && targetBottom != null)
        {
            pos = targetBottom.position;
            transform.position = Vector3.MoveTowards(transform.position, pos, speedMove * Time.deltaTime);
        }
    }
    public override void SkillAction()
    {
        StartCoroutine(DelaySkillAction());


        Debug.Log("SkillAction");
    }

    private IEnumerator DelaySkillAction()
    {
        // wait 1.5 second
        yield return new WaitForSeconds(0.7f);

        EnemyCtrl enemyCtrl = (EnemyCtrl)objectCtrl;

        if (enemyCtrl == null) yield break;

        enemyCtrl.EnemyAttack.canAttack = true;
    }



    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        if (sendDameFisrt) return;

        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }

        if (damageReceiver.IsDead) return;


        //Add Skill
        this.DamageSender.SendFXImpact(damageReceiver);
        sendDameFisrt = true;


        StartCoroutine(StopAction());


        Debug.Log("Call Skill Colider");

    }

    private IEnumerator StopAction()
    {
        // wait 1.5 second
        yield return new WaitForSeconds(timeLockAttack);

        EnemyCtrl enemyCtrl = (EnemyCtrl)objectCtrl;

        if (enemyCtrl == null) yield break;

        enemyCtrl.TargetSkill.IsSkill = false;
        
        if(enemyCtrl.EnemyAttack.CanAtacck.Count <=0)
            enemyCtrl.EnemyAttack.canAttack = false;

        //DeSpawn = true
        fxDespawn.ResetCanDespawnFlag();
    }
    private void StopEffect()
    {
        targetBottom = null;
        sendDameFisrt = false;
    }
        
}
