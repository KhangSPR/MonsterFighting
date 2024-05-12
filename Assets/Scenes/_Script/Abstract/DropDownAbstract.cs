using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DropDownAbstract: SaiMonoBehaviour
{
    public DropDownCtrl dropDownCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDropDownCtrl();
    }
    protected void LoadDropDownCtrl()
    {
        if (this.dropDownCtrl != null) return;
        this.dropDownCtrl = transform.parent.GetComponent<DropDownCtrl>();
        Debug.Log("LoadLoadObject");
    }
}
