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

    //protected virtual void LoadObjectCtrl()
    //{
    //    if (objectCtrl != null) return;
    //    objectCtrl = transform.parent.GetComponent<ObjectCtrl>();
    //    Debug.Log(gameObject.name + ": Loaded ObjectCtrl for " + gameObject.name);
    //}
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSent) return;

        // Nếu không có FXDamageReceiver, xử lý theo logic khác
        if (playerCtrl != null)
        {
            if (other.transform.parent.CompareTag("Enemy"))
            {
                playerCtrl.DamageSender.Send(other.transform.parent);
                Debug.Log("damageSent (player vs enemy)");

                // Đánh dấu đã gửi damage
                damageSent = true;
                gameObject.SetActive(false);
                return;
            }
        }
        else if (enemyCtrl != null)
        {
            if(enemyCtrl.TargetSkill.listSkillCtrl.Count >0)
            {
                enemyCtrl.TargetSkill.listSkillCtrl[0].FXDamageReceiver.DeductHealth(enemyCtrl.DamageSender.Damage);

                Debug.Log("Skill Effect");
                damageSent = true;
                gameObject.SetActive(false);
                return;
            }
            if (other.transform.parent.CompareTag("Player") || other.transform.parent.CompareTag("Castle"))
            {
                if (enemyCtrl.EnemyAttack.CanAtacck.Count > 0)
                {
                    enemyCtrl.DamageSender.Send(enemyCtrl.EnemyAttack.CanAtacck[0]);
                }
                damageSent = true;
                gameObject.SetActive(false);

                Debug.Log("Animation Impact");

                return;
            }
        }
    }

}
