using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCtrl : SaiMonoBehaviour
{
    [Header("Abtract Ctrl")]
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl { get => enemyCtrl; }
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl { get => playerCtrl; }
    [SerializeField] protected GuardCtrl guardCtrl;
    public GuardCtrl GuardCtrl { get => guardCtrl; }
    [SerializeField] protected DeFenSeCtrl deFenSeCtrl;
    public DeFenSeCtrl DeFenSeCtrl { get => deFenSeCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
        this.LoadPlayerCtrl();
        this.LoadGuardCtrl();
        this.LoadDeFenSeCtrl();
    }
    protected virtual void LoadPlayerCtrl()
    {
        if (this.playerCtrl != null) return;
        this.playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
        Debug.Log(transform.name + ": LoadPlayerCtrl", gameObject);
    }
    protected virtual void LoadGuardCtrl()
    {
        if (this.guardCtrl != null) return;
        this.guardCtrl = transform.parent.GetComponent<GuardCtrl>();
        Debug.Log(transform.name + ": LoadGuardCtrl", gameObject);
    }
    protected virtual void LoadEnemyCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = transform.parent.GetComponent<EnemyCtrl>();
        Debug.Log(transform.name + ": LoadEnemyCtrl", gameObject);
    }
    protected virtual void LoadDeFenSeCtrl()
    {
        if (this.deFenSeCtrl != null) return;
        this.deFenSeCtrl = transform.parent.GetComponent<DeFenSeCtrl>();
        Debug.Log(transform.name + ": LoadDeFenSeCtrl", gameObject);
    }
}
