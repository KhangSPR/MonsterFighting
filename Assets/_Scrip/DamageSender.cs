using System.Collections;
using UnityEngine;

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

        if(obj.name=="Castle")
        {
            if(damageReceiver.IsDead && !GameManager.Instance.IsCastleDead)
            {
                if (this is BulletDameSender bullet)
                {


                    EnemyCtrl enemyCtrl = (EnemyCtrl)bullet.bulletCtrl.ObjectCtrl;

                    enemyCtrl.ObjMoveIntheCity.OnDeadBulletEnemy();

                    enemyCtrl.EnemyAttack.OnDeadCastle(false);
                }
                if (this.enemyCtrl!= null)
                {
                    this.enemyCtrl.EnemyAttack.OnDeadCastle(true);
                }

                Debug.Log("Enemy Attack: " + transform.parent.name);

                GameManager.Instance.IsCastleDead = true; // Call One

                GameManager.Instance.CurrentModleCall = transform.parent;

                GameManager.Instance.SetAnimatorEnabled();

                Debug.Log("Castle: "+ damageReceiver.IsDead);

                if(damageReceiver is CastleDamageReceiver castleDamageReceiver)
                {
                    castleDamageReceiver.BoxCollider.enabled = false;
                }
            }

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

        Debug.Log("Send" + this.damage);
    }
    #region Comment
    //public void SendDamageOverTime(Transform obj, bool isColliding)
    //{
    //    DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
    //    if (damageReceiver == null) return;

    //    if (isColliding)
    //    {
    //        // Đặt lại damageInterval về ban đầu
    //        damageInterval = 3f;
    //        StartCoroutine(SendDamageOverTimeCoroutine(damageReceiver));
    //    }
    //    else
    //    {
    //        // Nếu không còn va chạm, bắt đầu gửi dame theo thời gian
    //        StartCoroutine(SendDamageOverTimeCoroutine(damageReceiver));
    //    }
    //}

    //private IEnumerator SendDamageOverTimeCoroutine(DamageReceiver damageReceiver)
    //{
    //    while (damageInterval > 0f)
    //    {
    //        yield return new WaitForSeconds(1f); // Chờ 0.3s trước khi gửi tiếp
    //        this.Send(damageReceiver); // Gửi dame
    //        damageInterval -= 0.5f; // Giảm thời gian đợi

    //    }

    //    // Đảm bảo damageInterval không nhỏ hơn 0
    //    damageInterval = Mathf.Max(damageInterval, 0f);
    //}
}









//protected virtual void createImpactFX()
//{
//    string fxName = this.getImpactFX();
//    Vector3 hitPos = transform.position;
//    Quaternion hitRot = transform.rotation;
//    Transform fxImpact = FXSpawner.Instance.Spawn(fxName, hitPos, hitRot);
//    fxImpact.gameObject.SetActive(true);
//}
//protected virtual string getImpactFX()
//{
//    return FXSpawner.ImpactOne;
//}
#endregion

