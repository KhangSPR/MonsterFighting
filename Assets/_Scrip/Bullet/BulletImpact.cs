using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BulletImpact : BulletAbstract
{
    [Header("Bullet Impart")]
    [SerializeField] protected CircleCollider2D circleCollider2D;
    [SerializeField] protected Rigidbody2D _rigidbody;

    // Thêm biến để kiểm tra đã gửi sát thương hay chưa
    public bool hasDealtDamage = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
        this.LoadRigibody();
    }

    protected virtual void LoadCollider()
    {
        if (this.circleCollider2D != null) return;
        this.circleCollider2D = GetComponent<CircleCollider2D>();
        this.circleCollider2D.isTrigger = true;
        this.circleCollider2D.radius = 0.25f;
        Debug.Log(transform.name + ": LoadCollider", gameObject);
    }

    protected virtual void LoadRigibody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.isKinematic = true;
        Debug.Log(transform.name + ": LoadRigibody", gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đã gây sát thương chưa, nếu rồi thì return luôn
        if (hasDealtDamage) return;

        if (other.transform.parent == null)
        {
            return;
        }

        if (other.name == "CanAttack" || other.name== "ObjMelee") return;

        BulletCtrl bulletCtrl = GetBulletCtrl(); // Lấy bulletCtrl từ lớp con
        if (bulletCtrl == null)
        {
            return; // Nếu bulletCtrl bị null, không thực hiện bất kỳ hành động nào
        }

        if (bulletCtrl.Shooter != null)
        {
            if (bulletCtrl.Shooter.CompareTag("Enemy"))
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    if (bulletCtrl.ObjectCtrl is EnemyCtrl enemyCtrl)
                    {
                        if (enemyCtrl.ObjLand.LandIndex != other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.LandIndex && !other.transform.parent.CompareTag("Castle")) return;

                        if (enemyCtrl.EnemyAttack.ListObjAttacks.Count > 0)
                        {
                            bulletCtrl.DamageSender.Send(enemyCtrl.EnemyAttack.ListObjAttacks[0]);
                            hasDealtDamage = true; // Đánh dấu đã gây sát thương
                            Debug.Log("Send Bullet Enemy");

                        }
                    }
                }
                else if (other.transform.parent.CompareTag("Castle"))
                {
                    if (bulletCtrl.ObjectCtrl is EnemyCtrl enemyCtrl)
                    {
                        if (enemyCtrl.EnemyAttack.ListObjAttacks.Count > 0)
                        {
                            bulletCtrl.DamageSender.Send(enemyCtrl.EnemyAttack.ListObjAttacks[0]);
                            hasDealtDamage = true; // Đánh dấu đã gây sát thương
                            Debug.Log("Send Bullet Enemy");
                        }
                    }
                }
                else if(other.transform.parent.CompareTag("Skill"))
                {
                    if (bulletCtrl.ObjectCtrl is EnemyCtrl enemyCtrl)
                    {
                        if (enemyCtrl.TargetSkillScript.listSkillCtrl.Count > 0)
                        {
                            if(bulletCtrl is BulletRegularCtrl regularCtrl)
                            {
                                SkillCtrl skill = regularCtrl.ObjLookAtTargetSetter.target.GetComponent<SkillCtrl>();

                                if(skill is MagicVortexCtrl magicVortex)
                                {
                                    magicVortex.FXDamageReceiver.DeductHealth(enemyCtrl.DamageSender.Damage, AttackType.Default);
                                }
                                else
                                {
                                    enemyCtrl.TargetSkillScript.listSkillCtrl[0].FXDamageReceiver.DeductHealth(enemyCtrl.DamageSender.Damage, AttackType.Default);
                                }
                            }
                            else
                            {
                                //Arrow
                            }


                            bulletCtrl.BulletDespawn.ResetCanDespawnFlag();
                            hasDealtDamage = true; // Đánh dấu đã gây sát thương
                            Debug.Log("Send Bullet Enemy");
                        }
                    }
                }
            }
            else if (bulletCtrl.Shooter.CompareTag("Player"))
            {
                if (other.transform.parent.CompareTag("Enemy"))
                {
                    if (bulletCtrl.ObjectCtrl is PlayerCtrl playerCtrl)
                    {
                        if (playerCtrl.ObjLand.LandIndex != other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.LandIndex) return;


                        if (playerCtrl.PlayerAttack.ListObjAttacks.Count > 0)
                        {
                            bulletCtrl.DamageSender.Send(playerCtrl.PlayerAttack.ListObjAttacks[0]);
                            hasDealtDamage = true; // Đánh dấu đã gây sát thương
                            Debug.Log("Send Bullet Player");
                        }
                    }
                }
            }
            else if (bulletCtrl.Shooter.CompareTag("Castle"))
            {
                if (bulletCtrl.ObjectCtrl is EnemyCtrl enemyCtrl)
                {
                    if (enemyCtrl.EnemyAttack.ListObjAttacks.Count > 0)
                    {
                        bulletCtrl.DamageSender.Send(enemyCtrl.EnemyAttack.ListObjAttacks[0]);
                        hasDealtDamage = true; // Đánh dấu đã gây sát thương
                    }
                }
            }
        }
    }
}
