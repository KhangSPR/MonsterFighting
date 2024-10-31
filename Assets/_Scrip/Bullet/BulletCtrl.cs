using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : SaiMonoBehaviour
{
    //Event
    public event Action OnSetBullet;


    [SerializeField] protected DamageSender damageSender;
    public DamageSender DamageSender { get => damageSender; }
    [SerializeField] protected BulletDespawn bulletDespawn;
    public BulletDespawn BulletDespawn { get => bulletDespawn; }
    [SerializeField] protected Transform shooter;
    public Transform Shooter => shooter;

    [SerializeField] protected BulletSO bulletSO;
    public BulletSO BulletSO { get => bulletSO; }
    [SerializeField] protected ObjectCtrl objectCtrl;
    public ObjectCtrl ObjectCtrl
    {
        get { return objectCtrl; }
        set { objectCtrl = value; }
    }
    [SerializeField] protected BulletImpact bulletImpact;
    public BulletImpact BulletImpact => bulletImpact;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadDamageSender();
        this.loadBulletDespawn();
        this.loadBulletImpact();
    }
    protected virtual void loadBulletImpact()
    {
        if (this.bulletImpact != null) return;
        this.bulletImpact = transform.GetComponentInChildren<BulletImpact>();
        Debug.Log(gameObject.name + ": loadBulletImpact" + gameObject);
    }
    protected virtual void loadDamageSender()
    {
        if (this.damageSender != null) return;
        this.damageSender = transform.GetComponentInChildren<DamageSender>();
        Debug.Log(gameObject.name + ": loadDamageSender" + gameObject);
    }
    protected virtual void loadBulletDespawn()
    {
        if (this.bulletDespawn != null) return;
        this.bulletDespawn = transform.GetComponentInChildren<BulletDespawn>();
        Debug.Log(gameObject.name + ": loadBulletDespawn" + gameObject);
    }
    public virtual void SetShotter(Transform shooter)
    {
        this.shooter = shooter;
    }
    protected  string GetObjectTypeString()
    {
        return ObjectType.Bullet.ToString();
    }
    public void SetBullet()
    {
        OnSetBullet?.Invoke();

        Debug.Log("setBullet");
    }
}
