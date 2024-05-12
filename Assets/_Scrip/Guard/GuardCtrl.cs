using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCtrl : SaiMonoBehaviour
{
    [Header("Guard Ctrl")]
    [SerializeField] protected ObjLookAtDistance objLookAtEnemy;
    public ObjLookAtDistance ObjLookAtEnemy => objLookAtEnemy;
    [SerializeField] protected GuardShooter guardShooter;
    public GuardShooter GuardShooter => guardShooter;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjLookAtEnemy();
        this.LoadShootByDistance();
    }
    protected virtual void LoadObjLookAtEnemy()
    {
        if (this.objLookAtEnemy != null) return;
        this.objLookAtEnemy = transform.GetComponentInChildren<ObjLookAtDistance>();
        Debug.Log(gameObject.name + ": loadObjLookAtEnemy" + gameObject);
    }
    protected virtual void LoadShootByDistance()
    {
        if (this.guardShooter != null) return;
        this.guardShooter = transform.GetComponentInChildren<GuardShooter>();
        Debug.Log(gameObject.name + ": loadShootByDistance" + gameObject);
    }
}
