using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<GameObject, InventorySlot> itemDisplayed = new Dictionary<GameObject, InventorySlot>();
    public ItemType displayType;
    [SerializeField] ChoosingManager choosingManager;
    [Space]
    [Space]
    [SerializeField] Transform holderInventory;
    [SerializeField] GameObject prefabItem;

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        //UpdateSlots();
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

    [ContextMenu("CreateDisplay")]
    public void CreateDisplay()
    {
        foreach (Transform child in holderInventory)
        {
            Destroy(child.gameObject);
        }
        itemDisplayed = new Dictionary<GameObject, InventorySlot>();

        // Danh sách các loại mục theo thứ tự ưu tiên
        List<ItemType> itemTypeOrder = new List<ItemType> { ItemType.Stone, ItemType.Poison, ItemType.Skill };

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
                obj.GetComponent<ItemTooltip>().ItemObject = inventory.database.GetItem[slot.item.Id];
                obj.transform.Find("Icon").GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
            }
            else
            {
                obj.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
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

    public void CreateDisplayByType(ItemType type)
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
        ItemType itemType = CheckItemtype(type);
        CreateDisplayByType(itemType);


        choosingManager.ActivateChoosingObject(itemType);
    }

    ItemType CheckItemtype(int number)
    {
        switch (number)
        {
            case 0:
                return ItemType.Stone;
            case 1:
                return ItemType.Poison;
            case 2:
                return ItemType.Skill;
            default:
                return ItemType.Skill;
        }
    }
}
