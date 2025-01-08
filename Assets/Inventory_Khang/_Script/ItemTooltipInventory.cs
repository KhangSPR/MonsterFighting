using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipInventory : ItemTooltip
{
    [Space(2)]
    [Header("UI Inventory")]
    [SerializeField] Image labelImg;
    public Image LabelImg => labelImg;
    [SerializeField] TMP_Text labelTxt;
    public TMP_Text LabelTxt => labelTxt;
    [SerializeField] Button labelBtn;
    public Button LabelBtn => labelBtn;
    [Space]
    [SerializeField]
    ItemObject itemObject;
    public ItemObject ItemObject { get { return itemObject; } set { itemObject = value; } }

    [SerializeField]
    private DisplayInventory _displayInventory;
    public DisplayInventory DisplayInventory { get { return _displayInventory; } set { _displayInventory = value; } }

    private void Start()
    {
        if(itemObject != null)
        {
            Tooltip_Item.AddTooltip(transform, itemObject, null);

            labelBtn.onClick.AddListener(HandlerOnPointerClickItem);

        }

        labelImg.gameObject.SetActive(false);
    }
    private void HandlerOnPointerClickItem()
    {
        if (_displayInventory == null) return;

        if (itemObject is not ItemCraftObject)
        {
            if (itemObject.IsUsed)
            {
                InventoryManager.Instance.CurrentQuantityItem -= 1;

                labelImg.color = Color.green;
                labelTxt.text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
                itemObject.IsUsed = !itemObject.IsUsed;

                _displayInventory.ResetDisplayItem();
            }
            else
            {
                if (InventoryManager.Instance.CurrentQuantityItem >= 5) return;

                InventoryManager.Instance.CurrentQuantityItem += 1;

                itemObject.IsUsed = !itemObject.IsUsed;
                labelImg.color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                labelTxt.text = "CANCEL";
                labelTxt.color = new Color(1f, 1f, 1f, 1f);

                _displayInventory.ResetDisplayItem();

            }
        }
        else
        {
            _displayInventory.OnClickItemCraft(itemObject.ID);
        }
    }
    public void SetLableItem(bool active)
    {
        labelImg.gameObject.SetActive(active);
    }
}
