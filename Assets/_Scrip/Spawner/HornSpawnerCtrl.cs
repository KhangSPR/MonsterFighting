using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSpawnerCtrl : AbilityPointAbstract
{
    [SerializeField] protected ObjectCtrl objectCtrl;
    public ObjectCtrl ObjectCtrl => objectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjectCtrl();
    }
    protected void LoadObjectCtrl()
    {
        if (objectCtrl != null) return;
        this.objectCtrl = transform.parent.GetComponent<ObjectCtrl>();
    }
}
