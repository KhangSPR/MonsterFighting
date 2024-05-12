using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickListener : ListInfoSelectAbstract
{
    public int objectIndex;
    [SerializeField] bool isActivated;
    public GameObject newObject;
    [SerializeField] TMP_Text _Text;

    //test
    protected override void Start()
    {
        this.LoadTNP_Text();
        // Thêm component Button vào GameObject nếu chưa có
        Button button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        // Thêm hàm OnButtonClick() vào sự kiện click của Button
        button.onClick.AddListener(OnButtonClick);

    }
    protected void LoadTNP_Text()
    {
        this._Text = transform.GetComponentInChildren<TMP_Text>();
    }
    private void NotifyListInfoSelectCtrl()
    {
        if (ListInfoSelectCtrl != null)
        {
            ListInfoSelectCtrl.OnButtonClickEvent(this);
        }
    }
    public void SetButtonText(string buttonText)
    {
        if (_Text != null)
        {
            _Text.text = buttonText;
        }
    }
    public Transform GetTranFromFade()
    {
        return ListInfoSelectCtrl.SelectCategoryCtrl.CardInventoryUICtrl.DropDownCtrl.FadeImage;
    }
    public void SetPrefabToInstantiateNull()
    {
        newObject = null;
    }
    //public Transform GetTranListSelect()
    //{
    //    return ListInfoSelectCtrl.SelectCategoryCtrl.dropDownCtrl.ListSelect;
    //}
    // Hàm này để gán giá trị index
    public void SetObjectIndex(int index)
    {
        objectIndex = index;
    }
    public void OnButtonClick()
    {
        // Check if the button is not activated
        if (!isActivated)
        {
            // Bật/tắt fadeGamobject
            if (GetTranFromFade() != null)
            {
                GameObject fadeGamobject = GetTranFromFade().gameObject;
                if (fadeGamobject != null)
                {
                    ToggleGameObject(fadeGamobject);
                }
            }

            // Đường dẫn đến prefab trong thư mục Resources
            string prefabPath = "Prefab/DropDown/ListSelect"; // Thay "YourPrefabName" bằng tên của prefab bạn muốn sử dụng

            // Load prefab từ Resources
            GameObject prefabToInstantiate = Resources.Load<GameObject>(prefabPath);

            if (prefabToInstantiate != null)
            {
                newObject = Instantiate(prefabToInstantiate, ListInfoSelectCtrl.listInfoSelect.DropDown);

                // Bạn có thể thực hiện các bước xử lý khác cho đối tượng mới ở đây

                Debug.Log("Da tim thay");

                SetNewObjectPosition(newObject);

                NotifyListInfoSelectCtrl();
            }
            NotifyListInfoSelectCtrl();
        }
    }
    private void SetNewObjectPosition(GameObject newPosition)
    {
        RectTransform newObjectRectTransform = newPosition.GetComponent<RectTransform>();

        if (newObjectRectTransform != null)
        {
            Vector2 newPositionValue;

            switch (objectIndex)
            {
                case 0:
                    newPositionValue = new Vector2(215, 110);
                    break;
                case 1:
                    newPositionValue = new Vector2(215, 15);
                    break;
                case 2:
                    newPositionValue = new Vector2(215, -70);
                    break;
                case 3:
                    newPositionValue = new Vector2(215, -155);
                    break;
                default:
                    newPositionValue = Vector2.zero;
                    break;
            }

            newObjectRectTransform.anchoredPosition = newPositionValue;
        }
        else
        {
            Debug.LogError("RectTransform component not found on the new object.");
        }
    }


    private void ToggleGameObject(GameObject gameObjectToToggle)
    {
        // Sử dụng property Toggle của GameObject để đảo ngược trạng thái
        gameObjectToToggle.SetActive(!gameObjectToToggle.activeSelf);
    }

}
