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
    [SerializeField] protected ParticleCtrl particleCtrl;
    public ParticleCtrl ParticleCtrl { get => particleCtrl; }
    [SerializeField] protected ObjectCtrl objCtrl;
    public ObjectCtrl ObjectCtrl { get => objCtrl; }

    [SerializeField] protected SkillCtrl skillCtrl;
    public SkillCtrl SkillCtrl { get => skillCtrl; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadInParentComponent(ref enemyCtrl);
        LoadInParentComponent(ref playerCtrl);
        LoadInParentComponent(ref guardCtrl);
        LoadInParentComponent(ref deFenSeCtrl);
        LoadInParentComponent(ref  bulletCtrl);
        LoadInParentComponent(ref particleCtrl);
        LoadInParentComponent(ref objCtrl);
        LoadInParentComponent(ref skillCtrl);


    }

    protected void LoadInParentComponent<T>(ref T component) where T : Component
    {
        if (component != null) return;
        component = transform.parent.GetComponent<T>();
        Debug.Log($"{transform.name}: Load {typeof(T).Name}", gameObject);
    }
}
