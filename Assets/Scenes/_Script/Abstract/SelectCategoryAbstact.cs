using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectCategoryAbstact : SaiMonoBehaviour
{
    public SelectCategoryCtrl SelectCategoryCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSelectCategoryCtrl();
    }
    protected void LoadSelectCategoryCtrl()
    {
        if (this.SelectCategoryCtrl != null) return;
        this.SelectCategoryCtrl = transform.parent.GetComponent<SelectCategoryCtrl>();
        Debug.Log(gameObject.name + ": LoadSelectCategoryCtrl" + gameObject);
    }
}
