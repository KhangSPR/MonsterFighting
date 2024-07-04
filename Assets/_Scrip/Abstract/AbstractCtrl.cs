using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCtrl : SaiMonoBehaviour
{
    [Header("Abstract Ctrl")]
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl { get => enemyCtrl; }
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl { get => playerCtrl; }
    [SerializeField] protected GuardCtrl guardCtrl;
    public GuardCtrl GuardCtrl { get => guardCtrl; }
    [SerializeField] protected DeFenSeCtrl deFenSeCtrl;
    public DeFenSeCtrl DeFenSeCtrl { get => deFenSeCtrl; }
    [SerializeField] protected BulletCtrl bulletCtrl;
    public BulletCtrl BulletCtrl { get => bulletCtrl; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadComponent(ref enemyCtrl);
        LoadComponent(ref playerCtrl);
        LoadComponent(ref guardCtrl);
        LoadComponent(ref deFenSeCtrl);
        LoadComponent(ref  bulletCtrl);
    }

    protected void LoadComponent<T>(ref T component) where T : Component
    {
        if (component != null) return;
        component = transform.parent.GetComponent<T>();
        Debug.Log($"{transform.name}: Load {typeof(T).Name}", gameObject);
    }
}
