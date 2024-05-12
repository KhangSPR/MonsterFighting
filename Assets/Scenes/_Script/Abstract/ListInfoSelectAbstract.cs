using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListInfoSelectAbstract : SaiMonoBehaviour
{
    public ListInfoSelectCtrl ListInfoSelectCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadListInfoSelect();
    }
    protected virtual void loadListInfoSelect()
    {
        if (this.ListInfoSelectCtrl != null) return;
        this.ListInfoSelectCtrl = transform.parent.GetComponent<ListInfoSelectCtrl>();
        Debug.Log(gameObject.name + ": loadDropDownCtrl" + gameObject);
    }
}
