using System.Collections;
using UnityEngine;
using static QuestInfoSO;

public enum AttackType
{
    Default,      // Đánh thường
    Burn,         // Do đốt cháy
    Skill         // Do kỹ năng
}
public class DamageSender : AbstractCtrl
{
    [SerializeField]
    int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    [SerializeField] protected SkillType skillType;
    public SkillType SkillType => skillType;  // Sửa lỗi ở đây
    [SerializeField] protected AttackType attackType;
    public AttackType AttackType => attackType;

    protected override void Start()
    {
        base.Start();

        this.damage = playerCtrl?.CharacterStatsFake.Attack ?? enemyCtrl?.EnemySO.basePointsAttack ?? this.damage;
    }
    public virtual void AddPower(int amount)
    {
        if (playerCtrl.ObjectDamageReceiver.IsDead) return;
        damage += amount;
    }
    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) return;
        this.Send(damageReceiver);

        if (obj.name == "Castle")
        {
            this.EnemyMoveGameEnd(damageReceiver);
            return;
        }
        Transform targetPosition = obj.GetComponent<ObjectCtrl>().TargetPosition;

        FXSpawner.Instance.SendFXText(damage, skillType, targetPosition, Quaternion.identity);

        Debug.Log("Default Spawner Sender");
    }
    public virtual void SendFXImpact(DamageReceiver damageReceiver, ObjectCtrl objectCtrl)
    {
        //DamageReceiver damageReceiver = obj.GetComponent<DamageReceiver>();

        this.Send(damageReceiver);

        FXSpawner.Instance.SendFXText(damage, skillType, objectCtrl.TargetPosition, Quaternion.identity);

        Debug.Log("FX Spawner Sender");

    }
    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.DeductHealth(this.damage, attackType);

        this.QuestAction(damageReceiver);

        Debug.Log("Send" + this.damage);
    }
    private void QuestAction(DamageReceiver damageReceiver)
    {
        if (!damageReceiver.IsDead) return;

        ObjectCtrl obj = damageReceiver.ObjectCtrl;

        if(obj == null) return; 

        if (obj.ObjectCtrlType != ObjectCtrlType.Enemy) return;

        EnemyCtrl enemyCtrl = (EnemyCtrl)obj;

        // Lưu vào biến tạm
        string enemyID = enemyCtrl.EnemySO.ID;
        if (GameManager.Instance._temporaryUpdates.ContainsKey(enemyID))
        {
            GameManager.Instance._temporaryUpdates[enemyID]++;
        }
        else
        {
            GameManager.Instance._temporaryUpdates[enemyID] = 1;
        }
    }
    private void EnemyMoveGameEnd(DamageReceiver damageReceiver)
    {
        if (damageReceiver.IsDead && !GameManager.Instance.IsCastleDead)
        {
            if (this is BulletDameSender bullet)
            {

                EnemyCtrl enemyCtrl = (EnemyCtrl)bullet.bulletCtrl.ObjectCtrl;

                enemyCtrl.ObjMoveIntheCity.OnDeadBulletEnemy();

                enemyCtrl.EnemyAttack.OnDeadCastle(false);
            }
            if (this.enemyCtrl != null)
            {
                this.enemyCtrl.EnemyAttack.OnDeadCastle(true);
            }

            Debug.Log("Enemy Attack: " + transform.parent.name);

            GameManager.Instance.IsCastleDead = true; // Call One

            GameManager.Instance.CurrentModleCall = transform.parent;

            GameManager.Instance.SetAnimatorEnabled();

            Debug.Log("Castle: " + damageReceiver.IsDead);

            if (damageReceiver is CastleDamageReceiver castleDamageReceiver)
            {
                castleDamageReceiver.BoxCollider.enabled = false;
            }
        }
    }
}

