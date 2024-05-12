using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventoryUICtrl : SaiMonoBehaviour
{
    public SelectCategoryCtrl SelectCategoryCtrl;
    public CategoryCtrl CategoryCtrl;
    public DropDownCtrl DropDownCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSelectCategoryCtrl();
        this.LoadCategoryCtrl();
        this.LoadDropDownCtrl();    
    }
    protected void LoadSelectCategoryCtrl()
    {
        if (this.SelectCategoryCtrl != null) return;
        this.SelectCategoryCtrl = transform.GetComponentInChildren<SelectCategoryCtrl>();
        Debug.Log(gameObject.name + ": LoadSelectCategoryCtrl" + gameObject);
    }
    protected void LoadDropDownCtrl()
    {
        if (this.DropDownCtrl != null) return;
        this.DropDownCtrl = transform.GetComponentInChildren<DropDownCtrl>();
        Debug.Log(gameObject.name + ": LoadDropDownCtrl" + gameObject);
    }
    protected void LoadCategoryCtrl()
    {
        if (this.CategoryCtrl != null) return;
        this.CategoryCtrl = transform.GetComponentInChildren<CategoryCtrl>();
        Debug.Log(gameObject.name + ": LoadCategoryCtrl" + gameObject);
    }

}
