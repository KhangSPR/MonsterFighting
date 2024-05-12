using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CategoryAbstract : SaiMonoBehaviour
{
    public CategoryCtrl categoryCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadCategoryCtrl();
    }
    protected virtual void loadCategoryCtrl()
    {
        if (this.categoryCtrl != null) return;
        this.categoryCtrl = transform.parent.GetComponent<CategoryCtrl>();
        Debug.Log(gameObject.name + ": loadCategoryCtrl" + gameObject);
    }
}
