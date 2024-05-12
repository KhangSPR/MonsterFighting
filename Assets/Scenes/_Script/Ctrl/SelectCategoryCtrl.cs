using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCategoryCtrl : CardInventoryUIAbstract
{
    public ListInfoSelectCtrl ListInfoSelectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadlistInfoSelect();
    }

    protected virtual void loadlistInfoSelect()
    {
        if (this.ListInfoSelectCtrl != null) return;
        this.ListInfoSelectCtrl = transform.GetComponentInChildren<ListInfoSelectCtrl>();
        Debug.Log(gameObject.name + ": loadlistInfoSelect" + gameObject);
    }
}
