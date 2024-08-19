using UnityEngine;

public class ChoosingManager : MonoBehaviour
{
    public GameObject[] choosingObjects;

    private GameObject currentActiveObject;

    public void ActivateChoosingObject(InventoryType itemType)
    {
        // Tắt đối tượng hiện đang được bật (nếu có)
        if (currentActiveObject != null)
        {
            currentActiveObject.SetActive(false);
        }

        // Bật đối tượng có tên tương ứng với lựa chọn
        foreach (var obj in choosingObjects)
        {
            if (obj.name == "Choosing" + itemType.ToString())
            {
                obj.SetActive(true);
                currentActiveObject = obj;
                break;
            }
        }
    }
    public void ActivateAll()
    {
        foreach (var obj in choosingObjects)
        {
            obj.SetActive(false);
        }
    }
}
