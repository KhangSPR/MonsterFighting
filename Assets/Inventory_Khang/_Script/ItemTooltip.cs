using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    ItemObject itemObject;
    public ItemObject ItemObject { get { return itemObject; } set { itemObject = value; } }

    ItemReward itemReward;
    public ItemReward ItemReward { get { return itemReward; } set { itemReward = value; } }

    [SerializeField] GameObject _lableItem;
    public GameObject LableItem => _lableItem;
    [SerializeField] Button _lableButton;
    private DisplayInventory _displayInventory;
    public DisplayInventory DisplayInventory { get { return _displayInventory; } set { _displayInventory = value; } }
    public void SetLableItem(bool active)
    {
        _lableItem.SetActive(active);
    }
    void Start()
    {
        _lableButton.onClick.AddListener(HandlerOnPointerClickItem);

        if (itemObject != null)
        {
            Tooltip_Item.AddTooltip(transform, itemObject, null);
        }
        if (itemReward != null)
        {
            Tooltip_Item.AddTooltip(transform, null, itemReward);
        }
        if (_lableItem != null)
            _lableItem.SetActive(false);
    }
    protected void HandlerOnPointerClickItem()
    {
        Debug.Log("Button clicked!");
        

        if (itemObject is not ItemCraftObject)
        {
            if (itemObject.IsUsed)
            {
                InventoryManager.Instance.CurrentQuantityItem -= 1;

                _lableItem.transform.GetComponent<Image>().color = Color.green;
                _lableItem.transform.Find("Text").GetComponent<TMP_Text>().text = "<color=white>USE</color> <color=green>" + InventoryManager.Instance.CurrentQuantityItem + "</color>" + "<color=white>/" + InventoryManager.Instance.LimitedQuantityItem + "</color>";
                itemObject.IsUsed  = !itemObject.IsUsed;

                _displayInventory.ResetDisplayItem();
                Debug.Log("Swap1 ");
            }
            else
            {
                if (InventoryManager.Instance.CurrentQuantityItem >= 5) return;

                InventoryManager.Instance.CurrentQuantityItem += 1;

                itemObject.IsUsed = !itemObject.IsUsed;
                _lableItem.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 225 / 255f); // RGBA (1, 1, 1, 1)
                _lableItem.transform.Find("Text").GetComponent<TMP_Text>().text = "CANCEL";
                _lableItem.transform.Find("Text").GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);

                _displayInventory.ResetDisplayItem();

                Debug.Log("Swap2");

            }
        }
        else
        {
            Debug.Log("Button clicked ItemCraftObject!");
        }
    }
}
