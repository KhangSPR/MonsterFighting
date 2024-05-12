using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRegularCtrl : BulletCtrl
{


    [Header("BulletRegular Ctrl")]
    [SerializeField] protected BulletFly bulletFly;
    public BulletFly BulletFly { get => bulletFly; }
    [SerializeField] protected Vector3 direction;
    public Vector3 Direction => direction;
    [SerializeField] protected ObjLookAtTargetSetter objLookAtTargetSetter;
    public ObjLookAtTargetSetter ObjLookAtTargetSetter => objLookAtTargetSetter;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadBulletFly();
        this.loadObjLookAtTargetSetter();
    }
    protected virtual void loadBulletFly()
    {
        if (this.bulletFly != null) return;
        this.bulletFly = transform.GetComponentInChildren<BulletFly>();
        Debug.Log(gameObject.name + ": loadBulletFly" + gameObject);
    }
    protected virtual void loadObjLookAtTargetSetter()
    {
        if (this.objLookAtTargetSetter != null) return;
        this.objLookAtTargetSetter = transform.GetComponentInChildren<ObjLookAtTargetSetter>();
        Debug.Log(gameObject.name + ": loadObjLookAtTargetSetter" + gameObject);
    }
    public void SetDirection(Vector3 set)
    {
        direction = set;
    }
    public Vector3 GetDirection()
    {
        return direction;
    }
}
