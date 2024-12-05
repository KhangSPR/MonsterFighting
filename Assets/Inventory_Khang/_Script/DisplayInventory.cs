using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.Rendering;
using DG.Tweening;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<GameObject, InventorySlot> itemDisplayed = new Dictionary<GameObject, InventorySlot>();
    public InventoryType displayType;
    [SerializeField] ChoosingManager choosingManager;
    [Space]
    [Space]
    [SerializeField] Transform holderInventory;
    [SerializeField] GameObject prefabItem;
    [SerializeField] List<ItemTooltip> itemTooltips = new List<ItemTooltip>();
    private void OnEnable()
    {
        CreateDisplay();
    }
    private void Update()
    {
        //UpdateSlots();
    }
    private void OnDisable()
    {
        this.itemTooltips.Clear();
    }
    //public void UpdateSlots()
    //{
    //    foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemDisplayed)
    //    {
    //        if (_slot.Value.ID >= 0)
    //        {
    //            // Update slot with item
    //            _slot.Key.transform.Find("Count").GetComponent<TMP_Text>().text = _slot.Value.amount == 0 ? "" : "x" + _slot.Value.amount.ToString();
    //        }
    //        else
    //        {
    //            // Clear slot
    //            _slot.Key.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    //            _slot.Key.transform.Find("Count").GetComponent<TMP_Text>().text = "";
    //        }
    //    }
    //}

    public void ResetDisplayItem()
    {
        foreach (var item in itemTooltips)
        {
            if (item.ItemObject.IsUsed)
            {
                item.LableItem.GetComponent<Image>().color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                item.LableItem.GetComponentInChildren<TMP_Text>().text = "CANCEL";
                item.LableItem.GetComponentInChildren<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                item.LableItem.GetComponent<Image>().color = Color.green;
                item.LableItem.GetComponentInChildren<TMP_Text>().text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
            }
        }
    }
    [ContextMenu("CreateDisplay")]
    public void CreateDisplay()
    {
        foreach (Transform child in holderInventory)
        {
            Destroy(child.gameObject);
        }
        itemDisplayed = new Dictionary<GameObject, InventorySlot>();

        // Danh sách các loại mục theo thứ tự ưu tiên
        List<InventoryType> itemTypeOrder = new List<InventoryType> { InventoryType.ItemCraft, InventoryType.Medicine, InventoryType.Skill };

        // Lọc và sắp xếp các mục trong inventory theo danh sách itemTypeOrder
        var sortedItems = inventory.Container.Items
            .Where(slot => slot.item != null && itemTypeOrder.Contains(slot.item.type))
            .OrderBy(slot => itemTypeOrder.IndexOf(slot.item.type))
            .ToArray();

        for (int i = 0; i < sortedItems.Length; i++)
        {
            InventorySlot slot = sortedItems[i];

            var obj = Instantiate(prefabItem, holderInventory);

            if (slot.ID >= 0)
            {
                ItemObject itemObject = inventory.database.GetItem[slot.item.Id];

                ItemTooltip itemTooltip = obj.GetComponent<ItemTooltip>();

                itemTooltip.ItemObject = itemObject;
                obj.transform.Find("Icon").GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                //Craft, Medicine, Skill
                itemTooltip.DisplayInventory = this;

                if (itemObject is not ItemCraftObject)
                {
                    //Add List Lable Item Tool Tip
                    this.itemTooltips.Add(itemTooltip);

                    if (itemObject.IsUsed)
                    {
                        obj.transform.Find("Lable").GetComponent<Image>().color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                        obj.transform.Find("Lable/Text").GetComponent<TMP_Text>().text = "CANCEL";
                        obj.transform.Find("Lable/Text").GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        obj.transform.Find("Lable").GetComponent<Image>().color = Color.green;
                        obj.transform.Find("Lable/Text").GetComponent<TMP_Text>().text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
                    }

                }
                else
                {
                    obj.transform.Find("Lable").GetComponent<Image>().color = new Color(178 / 255f, 112 / 255f, 0f, 225 / 255f);
                    obj.transform.Find("Lable/Text").GetComponent<TMP_Text>().text = "CRAFT";
                    obj.transform.Find("Lable/Text").GetComponent<TMP_Text>().color = new Color(1f, 165 / 255f, 0f, 1f);

                }
            }
            else
            {
                obj.GetComponent<Button>().enabled = false;
                obj.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();

                //Empty Item
            }

            itemDisplayed.Add(obj, slot);
        }
        Debug.Log("CreateDisplay");

        choosingManager.ActivateAll();
    }

    [ContextMenu("CreateDisplayByType")]
    public void CreateDisplayByType()
    {
        CreateDisplayByType(displayType);
    }

    public void CreateDisplayByType(InventoryType type)
    {
        foreach (Transform child in holderInventory)
        {
            Destroy(child.gameObject);
        }
        itemDisplayed = new Dictionary<GameObject, InventorySlot>();

        // Lọc các mục theo loại đã chọn
        List<InventorySlot> filteredSlots = inventory.Container.Items
            .Where(slot => slot.item != null && slot.item.type == type)
            .ToList();

        // Tạo và hiển thị các mục và ô trống
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            InventorySlot slot = i < filteredSlots.Count ? filteredSlots[i] : null;
            var obj = Instantiate(prefabItem, holderInventory);

            if (slot != null && slot.ID >= 0)
            {
                obj.GetComponent<ItemTooltip>().ItemObject = inventory.database.GetItem[slot.item.Id];
                obj.transform.Find("Icon").GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
            }
            else
            {
                obj.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = "";
            }

            itemDisplayed.Add(obj, slot);
        }
        Debug.Log("CreateDisplayByType");
    }


    public void OnClickCreateDisplayByType(int type)
    {
        InventoryType itemType = CheckItemtype(type);
        CreateDisplayByType(itemType);


        choosingManager.ActivateChoosingObject(itemType);
    }

    InventoryType CheckItemtype(int number)
    {
        switch (number)
        {
            case 0:
                return InventoryType.ItemCraft;
            case 1:
                return InventoryType.Medicine;
            case 2:
                return InventoryType.Skill;
            default:
                return InventoryType.Skill;
        }
    }
    #region MoveOnButtonClick Craft
    [Space]
    [Space]
    [Header("Move On Click Craft")]
    public RectTransform transform1; // Đối tượng đầu tiên
    public RectTransform transform2; // Đối tượng thứ hai
    public CanvasGroup canvas1;


    public float duration = 0.4f;    // Thời gian di chuyển

    public void OnButtonClick()
    {
        // Di chuyển transform1 từ vị trí hiện tại tới -300 từ phải qua trái
        transform1.DOAnchorPosX(-300, duration).SetEase(Ease.InOutQuad);

        transform2.gameObject.SetActive(true);

        // Di chuyển transform2 từ vị trí hiện tại tới 500 từ trái qua phải
        transform2.DOAnchorPosX(500, duration).SetEase(Ease.InOutQuad);

        canvas1.DOFade(1, 0.5f).SetEase(Ease.InOutQuad);

    }
    public void OnClickComPact()
    {
        transform2.DOAnchorPosX(0, duration)
            .SetEase(Ease.InOutQuad);

        transform1.DOAnchorPosX(0, duration).SetEase(Ease.InOutQuad);

        canvas1.DOFade(0, 0.5f).OnComplete(() => canvas1.gameObject.SetActive(false));
    }
    #endregion
}