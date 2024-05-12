using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AnimationImpact : SaiMonoBehaviour
{
    [Header("Animation Impart")]
    [SerializeField] protected BoxCollider2D boxCollider2D;
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl { get => playerCtrl; }
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl { get => enemyCtrl; }

    public bool damageSent;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCollider();
        this.LoadEnemyCtrl();
        this.LoadPlayerCtrl();
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
        this.boxCollider2D = GetComponent<BoxCollider2D>();
        this.boxCollider2D.isTrigger = true;
        this.boxCollider2D.offset = new Vector2(0.75f, -0.05f);
        this.boxCollider2D.size = new Vector2(0.5f, 0.15f);
        Debug.Log(transform.name + ": LoadCollider", gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSent) return;

        if (playerCtrl != null /*&& playerCtrl.transform.CompareTag("Player")*/)
        {
            if (other.transform.parent.CompareTag("Enemy"))
            {
                playerCtrl.DamageSender.Send(other.transform.parent);

                Debug.Log("damageSent");

                //Finish DamageSent
                damageSent = true;
                gameObject.SetActive(false);

                return;
            }



        }
        else if (enemyCtrl != null /*&& enemyCtrl.transform.CompareTag("Enemy")*/)
        {
            if (other.transform.parent.CompareTag("Player") || other.transform.parent.CompareTag("Castle"))
            {
                enemyCtrl.DamageSender.Send(other.transform.parent);

                Debug.Log("damageSent");

                damageSent = true;
                gameObject.SetActive(false);

                return;
            }

        }
        //this.CreateImpactFX(other);
    }
}
