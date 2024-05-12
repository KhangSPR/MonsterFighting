using System;
using UnityEngine;

public abstract class ShootAbleObjectAbstact : SaiMonoBehaviour
{
    [Header("ShootAbleObject Abstact")]
    [SerializeField] protected ObjectCtrl shootAbleObjectCtrl;
    public ObjectCtrl ShootAbleObjectCtrl => shootAbleObjectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadShootAbleObjectCtrl();
    }
    protected virtual void LoadShootAbleObjectCtrl()
    {
        if (this.shootAbleObjectCtrl != null) return;
        this.shootAbleObjectCtrl = transform.parent.GetComponentInChildren<ObjectCtrl>();
        Debug.Log(gameObject.name + ": loadShootAbleObjectCtrl" + gameObject);
    }
}
