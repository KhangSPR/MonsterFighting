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

    [SerializeField] protected Object shootAbleObjectSO;
    public Object ShootAbleObjectSO => shootAbleObjectSO;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadDamageSender();
        this.loadBulletDespawn();
        this.loadShootAbleObjectSO();
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
    protected virtual void loadShootAbleObjectSO() // ScriptableObject
    {
        if (this.shootAbleObjectSO != null) return;
        string resPath = "ShootAbleObject/" + this.GetObjectTypeString() + "/" + transform.name;
        this.shootAbleObjectSO = Resources.Load<Object>(resPath); //Ph?i t?o Folder là Resources
        Debug.LogWarning(transform.name + ": LoadShootAbleObjectSO" + resPath, gameObject);
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
