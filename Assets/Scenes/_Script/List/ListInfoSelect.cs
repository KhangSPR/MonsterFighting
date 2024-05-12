using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ListInfoSelect : SelectCategoryAbstact
{
    private string listNameFolder;
    [SerializeField] private GameObject[] SelectInfo;
    public List<string> instantiatedObjectNames = new List<string>();
    public List<GameObject> instantiatedObjects = new List<GameObject>();
    public Transform DropDown;

    protected override void Start()
    {
        base.Start();

        // Đăng ký sự kiện OnCustomNameChanged từ TagClickListener
        if (SelectCategoryCtrl.CardInventoryUICtrl.CategoryCtrl.TagClickListener != null)
        {
            SelectCategoryCtrl.CardInventoryUICtrl.CategoryCtrl.TagClickListener.OnCustomNameChanged += OnTagClickListenerCustomNameChanged;
            Debug.Log("Reload");
        }
    }

    public void OnTagClickListenerCustomNameChanged(string newCustomName)
    {
        // Khi customName thay đổi, gọi lại hàm HandleObjectNameChange để load lại thông tin
        LoadSelectInfo();
    }
    public string SetlistNameFolder()
    {
        return SelectCategoryCtrl.CardInventoryUICtrl.CategoryCtrl.TagClickListener.CustomName;
    }

    private GameObject[] LoadGameScene(string path)
    {
        return Resources.LoadAll<GameObject>(path);
    }

    private void LoadSelectInfo()
    {
        // Kiểm tra xem có thông tin nào đang tồn tại không
        if (SelectInfo != null)
        {
            // Xóa tất cả các đối tượng trong danh sách
            foreach (var obj in instantiatedObjects)
            {
                Destroy(obj);
            }

            // Xóa danh sách các đối tượng và tên đối tượng
            instantiatedObjects.Clear();
            instantiatedObjectNames.Clear();
        }

        // Set giá trị listNameFolder tại đây
        listNameFolder = SetlistNameFolder();

        // Load mới các GameObject từ thư mục "Prefab/Select"
        SelectInfo = LoadGameScene("Prefab/Select/" + listNameFolder);

        // Instantiate lại và thiết lập parent
        InstantiateAndSetParent();
    }

    private void InstantiateAndSetParent()
    {
        // Duyệt qua mảng SelectInfo và instantiate từng GameObject
        int length = SelectInfo.Length;
        for (int i = 0; i < length; i++)
        {
            GameObject obj = SelectInfo[i];

            // Instantiate GameObject và gán cha là GameObject chứa script
            GameObject instantiatedObject = Instantiate(obj, transform.position, Quaternion.identity, transform);

            // Thêm vào danh sách
            instantiatedObjects.Add(instantiatedObject);

            // Lấy tên của đối tượng và thêm vào mảng
            string objectName = obj.name;
            instantiatedObjectNames.Add(objectName);

            // Thêm component ButtonClickListener vào GameObject và lấy reference
            ButtonClickListener btnClick = instantiatedObject.AddComponent<ButtonClickListener>();

            // Gọi hàm SetObjectIndexAndListInfoSelect và truyền giá trị index và ListInfoSelect reference
            btnClick.SetObjectIndex(i);
        }
    }

    private void OnApplicationQuit()
    {
        ClearSelectInfo();
    }

    private void ClearSelectInfo()
    {
        // Release the reference to the array
        SelectInfo = null;
        instantiatedObjectNames.Clear();
        instantiatedObjects.Clear();
    }
}
