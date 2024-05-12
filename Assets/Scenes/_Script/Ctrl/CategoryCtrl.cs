using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryCtrl : CardInventoryUIAbstract
{
    public TagClickListener TagClickListener;
    public ListCategory ListCategory;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadListCategory();
        AutoListCategory();
    }
    protected virtual void LoadListCategory()
    {
        if (ListCategory != null) return;
        ListCategory = transform.GetComponent<ListCategory>();
        Debug.Log(gameObject.name + ": LoadListCategory " + gameObject);
    }

    protected virtual void AutoListCategory()
    {
        // Lấy GameObject tại vị trí thứ 0
        GameObject clickedObject = ListCategory.instantiatedObjects[0];
        // Lấy hoặc thiết lập TagClickListener
        TagClickListener = clickedObject.GetComponent<TagClickListener>();
    }

    public void OnButtonClickEvent(TagClickListener tagClickListener)
    {
        // Truy cập ListInfoSelect liên quan đến nút được nhấp
        CategoryCtrl categoryCtrl = tagClickListener.categoryCtrl;

        // Truy cập GameObject liên quan đến ListInfoSelect
        GameObject clickedObject = categoryCtrl.ListCategory.instantiatedObjects[tagClickListener.ClickIndex];

        //// Lấy thành phần mong muốn từ clickedObject
        TagClickListener = clickedObject.GetComponent<TagClickListener>();

        // Làm điều gì đó với yourComponent...
    }
}
