using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<ItemTooltip, InventorySlot> itemDisplayed = new Dictionary<ItemTooltip, InventorySlot>();
    public InventoryType displayType;
    [SerializeField] ChoosingManager choosingManager;
    [Space]
    [Space]
    [SerializeField] Transform holderInventory;
    [SerializeField] CraftUI craftUI;
    [SerializeField]
    List<ItemTooltipInventory> itemTooltips = new List<ItemTooltipInventory>();
    private void OnEnable()
    {
        CreateDisplay();

        CraftUI.OnCraft += UpdateSlots;
    }
    private void OnDisable()
    {
        CraftUI.OnCraft -= UpdateSlots;
    }
    private void UpdateSlots()
    {
        bool shouldRecreateDisplay = false; // Chỉ gọi CreateDisplay() nếu cần

        foreach (KeyValuePair<ItemTooltip, InventorySlot> _slot in itemDisplayed)
        {
            ItemTooltip itemTooltip = _slot.Key;
            InventorySlot slot = _slot.Value;

            if (slot != null && slot.ID >= 0)
            {
                // Chỉ cập nhật nếu số lượng thay đổi
                string newText = slot.amount.ToString();
                if (itemTooltip.CountTxt.text != newText)
                {
                    itemTooltip.CountTxt.text = newText;
                }
            }
            else
            {
                // Đánh dấu cần làm mới giao diện
                shouldRecreateDisplay = true;
            }
        }

        // Chỉ gọi CreateDisplay() nếu cần
        if (shouldRecreateDisplay)
        {
            if(displayType == InventoryType.ItemCraft)
            {
                CreateDisplayByType();

                return;
            }
            CreateDisplay();
        }
    }


    public void ResetDisplayItem()
    {
        foreach (var item in itemTooltips)
        {
            if (item.ItemObject.IsUsed)
            {
                item.LabelImg.color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                item.LabelTxt.text = "CANCEL";
                item.LabelImg.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                item.LabelImg.color = Color.green;
                item.LabelTxt.text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
            }
        }
    }
    [ContextMenu("CreateDisplay")]
    public void CreateDisplay()
    {
        this.itemTooltips.Clear();

        //displayType = InventoryType.ALL; //SET ENUM DISPLAY

        foreach (Transform child in holderInventory)
        {
            Destroy(child.gameObject);
        }
        itemDisplayed = new Dictionary<ItemTooltip, InventorySlot>();

        // Danh sách các loại mục theo thứ tự ưu tiên
        List<InventoryType> itemTypeOrder = new List<InventoryType> { InventoryType.ItemCraft, InventoryType.Medicine, InventoryType.Skill };

        // Lọc và sắp xếp các mục trong inventory theo danh sách itemTypeOrder

        var sortedItems = inventory?.Container?.Items?
            .Where(slot => slot != null && slot.item != null && itemTypeOrder.Contains(slot.item.type))
            .OrderBy(slot => itemTypeOrder.IndexOf(slot.item.type))
            .ToArray();

        // Kiểm tra nếu `sortedItems` là null hoặc rỗng để tránh lỗi tiếp theo
        if (sortedItems == null || sortedItems.Length == 0)
        {
            Debug.LogWarning("No valid items found in inventory.");
        }


        for (int i = 0; i < sortedItems.Length; i++)
        {
            InventorySlot slot = sortedItems[i];

            var obj = Instantiate(RewardClaimManager.Instance.ItemObject, holderInventory);

            ItemTooltipInventory itemTooltip = obj.GetComponent<ItemTooltipInventory>();


            if (slot.ID >= 0)
            {
                ItemObject itemObject = inventory.database.GetItem[slot.item.Id];

                itemTooltip.ItemObject = itemObject;
                itemTooltip.Avatar.sprite = itemObject.Sprite;
                //itemTooltip.Avatar.sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemObject.itemRarity);

                itemTooltip.CountTxt.text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                //Craft, Medicine, Skill
                itemTooltip.DisplayInventory = this;

                if (itemObject is not ItemCraftObject)
                {
                    //Add List Lable Item Tool Tip
                    this.itemTooltips.Add(itemTooltip);

                    if (itemObject.IsUsed)
                    {
                        itemTooltip.LabelImg.color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                        itemTooltip.LabelTxt.text = "CANCEL";
                        itemTooltip.LabelTxt.color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        itemTooltip.LabelImg.color = Color.green;
                        itemTooltip.LabelTxt.text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
                    }

                }
                else
                {
                    itemTooltip.LabelImg.color = new Color(178 / 255f, 112 / 255f, 0f, 225 / 255f);
                    itemTooltip.LabelTxt.text = "CRAFT";
                    itemTooltip.LabelTxt.color = new Color(1f, 165 / 255f, 0f, 1f);

                }
            }
            else
            {
                itemTooltip.LabelBtn.enabled = false;
                itemTooltip.Avatar.color = new Color(1, 1, 1, 0);
                itemTooltip.RawrRarity.color = new Color(1, 1, 1, 0);
                itemTooltip.CountTxt.text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                //Empty Item
            }

            itemDisplayed.Add(itemTooltip, slot);
        }
        Debug.Log("CreateDisplay");

        choosingManager.ActivateAll();
    }

    [ContextMenu("CreateDisplayByType")]
    public void CreateDisplayByType()
    {
        CreateDisplayByType(displayType);
        this.itemTooltips.Clear();
    }

    public void CreateDisplayByType(InventoryType type)
    {
        displayType = type; //SET ENUM DISPLAY

        foreach (Transform child in holderInventory)
        {
            Destroy(child.gameObject);
        }
        itemDisplayed = new Dictionary<ItemTooltip, InventorySlot>();

        InventorySlot[] Items = new InventorySlot[40];

        List<InventorySlot> filteredSlots = inventory?.Container?.Items?
                .Where(slot => slot != null && slot.item != null && slot.item.type == type && slot.item.Id >= 0 /*&& slot.amount > 0*/)
                .ToList();

        for (int i = 0; i < filteredSlots.Count; i++)
        {
            Items[i] = filteredSlots[i];
        }

        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = new InventorySlot();
            }
        }

        for (int i = 0; i < Items.Length; i++)
        {
            InventorySlot slot = Items[i];

            var obj = Instantiate(RewardClaimManager.Instance.ItemObject, holderInventory);

            ItemTooltipInventory itemTooltip = obj.GetComponent<ItemTooltipInventory>();


            if (slot.ID >= 0)
            {
                ItemObject itemObject = inventory.database.GetItem[slot.item.Id];

                itemTooltip.ItemObject = itemObject;
                itemTooltip.Avatar.sprite = itemObject.Sprite;
                //itemTooltip.Avatar.sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemObject.itemRarity);

                itemTooltip.CountTxt.text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                //Craft, Medicine, Skill
                itemTooltip.DisplayInventory = this;

                if (itemObject is not ItemCraftObject)
                {
                    //Add List Lable Item Tool Tip
                    this.itemTooltips.Add(itemTooltip);

                    if (itemObject.IsUsed)
                    {
                        itemTooltip.LabelImg.color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                        itemTooltip.LabelTxt.text = "CANCEL";
                        itemTooltip.LabelTxt.color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        itemTooltip.LabelImg.color = Color.green;
                        itemTooltip.LabelTxt.text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
                    }

                }
                else
                {
                    itemTooltip.LabelImg.color = new Color(178 / 255f, 112 / 255f, 0f, 225 / 255f);
                    itemTooltip.LabelTxt.text = "CRAFT";
                    itemTooltip.LabelTxt.color = new Color(1f, 165 / 255f, 0f, 1f);

                }
            }
            else
            {
                itemTooltip.LabelBtn.enabled = false;
                itemTooltip.Avatar.color = new Color(1, 1, 1, 0);
                itemTooltip.RawrRarity.color = new Color(1, 1, 1, 0);
                itemTooltip.CountTxt.text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                //Empty Item
            }

            itemDisplayed.Add(itemTooltip, slot);
        }
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

    public void OnClickItemCraft(string ID)
    {
        this.SetCraftUI(ID);

        // Di chuyển transform1 từ vị trí hiện tại tới -300 từ phải qua trái
        transform1.DOAnchorPosX(-380, duration).SetEase(Ease.InOutQuad);

        transform2.gameObject.SetActive(true);

        // Di chuyển transform2 từ vị trí hiện tại tới 500 từ trái qua phải
        transform2.DOAnchorPosX(580, duration).SetEase(Ease.InOutQuad);

        canvas1.DOFade(1, 0.5f).SetEase(Ease.InOutQuad);
    }
    private void SetCraftUI(string ID)
    {
        if(craftUI == null)
        {
            Debug.LogError("Craft UI == Null");
        }
        Debug.Log("ID: "+ ID);
        craftUI.SetItemObject(InventoryManager.Instance.inventory.GetItemObjectCanCraftByID(ID));
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