using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListSelect : DropDownAbstract
{
    private string ObjectName;
    private string ObjectLoading;

    [SerializeField] GameObject[] selectObjects;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    protected override void Start()
    {
        SetNameOBJ();
        SetNameOBJLoading();
        // Load các đối tượng từ thư mục "Prefab/Select/ParentObjectName"
        selectObjects = LoadSelectObjects("Prefab/MenuSelect/" + ObjectLoading + "/" + ObjectName);

        InstantiateAndSetParent();

    }
    protected List<string> GetObjectLoading(int index)
    {
        return dropDownCtrl.CardInventoryUICtrl.CategoryCtrl.ListCategory.instantiatedObjectNames;
    }
    protected List<string> GetNameObjInfo(int index)
    {
        return dropDownCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.listInfoSelect.instantiatedObjectNames;
    }
    public int GetIndexObjectLoading()
    {
        return dropDownCtrl.CardInventoryUICtrl.CategoryCtrl.TagClickListener.ClickIndex;
    }
    public int GetIndexObjectName()
    {
        return dropDownCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.ButtonClickListener.objectIndex;
    }
    protected void SetNameOBJ()
    {
        List<string> objectNames = GetNameObjInfo(GetIndexObjectName());

        // Kiểm tra index có hợp lệ và danh sách không rỗng
        if (GetIndexObjectName() >= 0 && GetIndexObjectName() < objectNames.Count)
        {
            ObjectName = objectNames[GetIndexObjectName()];
        }
    }
    protected void SetNameOBJLoading()
    {
        List<string> objectNames = GetObjectLoading(GetIndexObjectLoading());

        // Kiểm tra index có hợp lệ và danh sách không rỗng
        if (GetIndexObjectLoading() >= 0 && GetIndexObjectLoading() < objectNames.Count)
        {
            ObjectLoading = objectNames[GetIndexObjectLoading()];
        }
    }
    protected void SetNameText(string text)
    {
        dropDownCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.ButtonClickListener.SetButtonText(text);
    }

    private GameObject[] LoadSelectObjects(string path)
    {
        // Load tất cả các đối tượng từ thư mục cụ thể
        return Resources.LoadAll<GameObject>(path);
    }
    private void InstantiateAndSetParent()
    {
        for (int i = 0; i < selectObjects.Count(); i++)
        {
            int currentIndex = i; // Tạo biến cục bộ

            // Instantiate GameObject và gán cha là GameObject chứa script
            GameObject instantiatedObject = Instantiate(selectObjects[i], transform.position, Quaternion.identity, transform);

            // Thêm component Button nếu chưa tồn tại
            Button button = instantiatedObject.GetComponent<Button>();
            if (button == null)
            {
                button = instantiatedObject.AddComponent<Button>();
            }

            // Thêm sự kiện cho nút, sử dụng biến cục bộ currentIndex
            button.onClick.AddListener(() => OnButtonClick(currentIndex));


            // Add instantiatedObject to the list
            instantiatedObjects.Add(instantiatedObject);
        }
    }
    private void OnButtonClick(int clickedIndex)
    {
        //Swap text
        Text buttonText = instantiatedObjects[clickedIndex].GetComponentInChildren<Text>();
        string buttonTextValue = buttonText.text;

        SetNameText(buttonTextValue);

        // Biến chứa hàm cần gọi dựa trên objectIndex
        UnityAction<int, int> selectedAction = null;

        int indexObjectLoading = GetIndexObjectLoading();

        // Nested switch case
        switch (indexObjectLoading)
        {
            case 0:
                switch (GetIndexObjectName())
                {
                    case 0:
                        selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyAttackType;
                        break;
                    case 1:
                        selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyCharacterClass;
                        break;
                    case 2:
                        selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyRare;
                        break;
                    case 3:
                        selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickedStar;
                        break;
                        // Thêm các case khác nếu có
                }
                break;
            case 1:
                switch (GetIndexObjectName())
                {
                    case 0:
                        selectedAction = CardUIPanelManager.Instance.OnMachineButtonClickbyClass;
                        break;
                    case 1:
                        selectedAction = CardUIPanelManager.Instance.OnMachineButtonClickbyRare;
                        break;
                        // Thêm các case khác nếu có
                }
                break;
                // Thêm các case khác nếu có
        }

        // Gọi hàm đã chọn
        selectedAction?.Invoke(indexObjectLoading, clickedIndex);

        // Tắt và xóa các đối tượng
        dropDownCtrl.GetOBJFade().gameObject.SetActive(false);
        dropDownCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.ButtonClickListener.SetPrefabToInstantiateNull();
        Destroy(gameObject);
    }

}
