using UnityEngine;

public class ListInfoSelectCtrl : SelectCategoryAbstact
{
    public ButtonClickListener ButtonClickListener;
    public ListInfoSelect listInfoSelect;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadListInfoSelect();
    }
    protected virtual void loadListInfoSelect()
    {
        if (this.listInfoSelect != null) return;
        this.listInfoSelect = transform.GetComponent<ListInfoSelect>();
        Debug.Log(gameObject.name + ": loadListInfoSelect" + gameObject);
    }
    public void OnButtonClickEvent(ButtonClickListener buttonClickListener)
    {
        // Access the ListInfoSelect associated with the clicked button
        ListInfoSelectCtrl listInfoSelect = buttonClickListener.ListInfoSelectCtrl;

        // Access the GameObject associated with the ListInfoSelect
        GameObject clickedObject = listInfoSelect.listInfoSelect.instantiatedObjects[buttonClickListener.objectIndex];

        // Get the desired component from the clickedObject
        ButtonClickListener = clickedObject.GetComponent<ButtonClickListener>();

        // Do something with yourComponent...
    }
}
