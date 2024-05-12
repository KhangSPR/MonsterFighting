using UnityEngine;

public class TagClickListener : CategoryAbstract
{
    public int ClickIndex;
    private string _customName;

    public string CustomName
    {
        get { return _customName; }
        set
        {
            if (_customName != value)
            {
                _customName = value;
                OnCustomNameChanged?.Invoke(_customName);
            }
        }
    }

    public event System.Action<string> OnCustomNameChanged;

    private void NotifyListCategoryCtrl()
    {
        if (categoryCtrl != null)
        {
            categoryCtrl.OnButtonClickEvent(this);
        }
    }
    protected override void Start()
    {
        base.Start();
        CustomName = GetCustomName();
    }

    public void OnButtonClick()
    {
        NotifyListCategoryCtrl();

        CustomName = GetCustomName();

        categoryCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.listInfoSelect.OnTagClickListenerCustomNameChanged(CustomName);

        GetCustomName();

    }

    public string GetCustomName()
    {
        return gameObject.name;
    }

}
