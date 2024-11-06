using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AnimationImpact : SaiMonoBehaviour
{
    [Header("Animation Impart")]
    [SerializeField] protected CircleCollider2D boxCollider2D;
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl { get => playerCtrl; }
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl { get => enemyCtrl; }

    //[Header("Object")]
    //[SerializeField] protected ObjectCtrl objectCtrl;



    public bool damageSent;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
        this.LoadEnemyCtrl();
        this.LoadPlayerCtrl();
        //this.LoadObjectCtrl();
    }
    protected virtual void LoadPlayerCtrl()
    {
        if (this.playerCtrl != null) return;
        this.playerCtrl = transform.parent.parent.GetComponent<PlayerCtrl>();
        Debug.Log(transform.name + ": LoadPlayerCtrl", gameObject);
    }
    protected virtual void LoadEnemyCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = transform.parent.parent.GetComponent<EnemyCtrl>();
        Debug.Log(transform.name + ": LoadEnemyCtrl", gameObject);
    }
    protected virtual void LoadCollider()
    {
        if (this.boxCollider2D != null) return;
        this.boxCollider2D = GetComponent<CircleCollider2D>();
        this.boxCollider2D.isTrigger = true;
        this.boxCollider2D.offset = new Vector2(0.75f, -0.05f);
        this.boxCollider2D.offset = new Vector2(0.5f, 0.15f);
        Debug.Log(transform.name + ": LoadCollider", gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSent) return;

        if (other.name == "CanAttack" || other.name == "ObjMelee") return;


        // Nếu không có FXDamageReceiver, xử lý theo logic khác
        if (playerCtrl != null)
        {
            if (other.transform.parent.CompareTag("Enemy"))
            {
                var otherObjectCtrl = other.transform.parent.GetComponent<ObjectCtrl>();

                if (otherObjectCtrl == null || playerCtrl.ObjLand.LandIndex != otherObjectCtrl.ObjLand.LandIndex)
                {
                    Debug.Log("Animation Impact other enemyCtrl: " + other.name);
                    return;
                }

                if (playerCtrl.ObjMelee != null && playerCtrl.ObjMelee.ListObjAttacks.Count > 0)
                {
                    playerCtrl.DamageSender.Send(playerCtrl.ObjMelee.ListObjAttacks[0]); //close range attack first
                }
                else if (playerCtrl.PlayerAttack.ListObjAttacks.Count > 0)
                {
                    playerCtrl.DamageSender.Send(playerCtrl.PlayerAttack.ListObjAttacks[0]);
                }
                // Đánh dấu đã gửi damage
                damageSent = true;
                gameObject.SetActive(false);
            }
        }
        else if (enemyCtrl != null)
        {
            var otherObjectCtrl = other.transform.parent.GetComponent<ObjectCtrl>();

            if (otherObjectCtrl == null || enemyCtrl.ObjLand.LandIndex != otherObjectCtrl.ObjLand.LandIndex)
            {
                Debug.Log("Animation Impact other enemyCtrl: " + other.name);
                return;
            }



            if (enemyCtrl.TargetSkill.listSkillCtrl.Count > 0)
            {
                enemyCtrl.TargetSkill.listSkillCtrl[0].FXDamageReceiver.DeductHealth(enemyCtrl.DamageSender.Damage, AttackType.Default);
                Debug.Log("Skill Effect");
                damageSent = true;
                gameObject.SetActive(false);
            }
            else if (other.transform.parent.CompareTag("Player") || other.transform.parent.CompareTag("Castle"))
            {
                if (enemyCtrl.ObjMelee != null && enemyCtrl.ObjMelee.ListObjAttacks.Count > 0)
                {
                    enemyCtrl.DamageSender.Send(enemyCtrl.ObjMelee.ListObjAttacks[0]); //close range attack first
                }
                else if (enemyCtrl.EnemyAttack.ListObjAttacks.Count > 0)
                {
                    int targetsToSend = Mathf.Min(enemyCtrl.ObjAttack.MaxAttackTargets, enemyCtrl.EnemyAttack.ListObjAttacks.Count);

                    for (int i = 0; i < targetsToSend; i++)
                    {
                        enemyCtrl.DamageSender.Send(enemyCtrl.EnemyAttack.ListObjAttacks[i]);
                        Debug.Log("Send to Player " + (i + 1) + ": " + enemyCtrl.EnemyAttack.ListObjAttacks[i].name);
                    }

                    Debug.Log("Send " + targetsToSend + " Player(s)");
                }

                damageSent = true;
                gameObject.SetActive(false);

                Debug.Log("Animation Impact");
            }
        }
    }


}
