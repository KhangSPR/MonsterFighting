using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text descriptionTxt;
    [SerializeField] Image avatarImg;
    [SerializeField] Transform holderCraft;
    [SerializeField] Button nextBtn;
    [SerializeField] Button previousBtn;
    [SerializeField] Button craftBtn;
    [SerializeField] ItemObject[] itemObjects;
    int currentItemObject = 0;
    private void Start()
    {
        nextBtn.onClick.AddListener(OnButtonNext);
        previousBtn.onClick.AddListener(OnButtonPrevious);
        craftBtn.onClick.AddListener(OnCraftBtn);
    }
    private void OnEnable()
    {
        SetItemObjets();
    }
    public void SetItemObject(ItemObject[] itemObjects)
    {
        Debug.Log("SetItemObject: " + itemObjects.Length);

        this.itemObjects = itemObjects;
    }
    protected void SetItemObjets()
    {
        if (this.itemObjects == null)
        {
            Debug.LogError("Item objects lenght: " + itemObjects.Length);
        }
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if(i == currentItemObject)
            {
                SetCraftUI(itemObjects[currentItemObject]);
                return;
            }
        }
    }
    private void SetCraftUI(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            Debug.LogError("ItemObject is null.");
            return;
        }

        Debug.Log("ItemObject: " + itemObject.name);

        // Cập nhật thông tin cơ bản
        UpdateUIElements(itemObject);

        // Xóa các item cũ trong giao diện
        ClearHolderCraft();

        // Hiển thị các item cần thiết để craft
        foreach (var itemRequiredCraft in itemObject.itemRequiredCrafts)
        {
            AddRequiredItemToUI(itemRequiredCraft);
        }
    }

    // Cập nhật các UI cơ bản
    private void UpdateUIElements(ItemObject itemObject)
    {
        nameTxt.text = itemObject.Name;
        descriptionTxt.text = itemObject.description;
        avatarImg.sprite = itemObject.Sprite;
    }

    // Xóa các item trong holderCraft
    private void ClearHolderCraft()
    {
        foreach (Transform child in holderCraft)
        {
            Destroy(child.gameObject);
        }
    }

    // Thêm item cần thiết vào UI
    private void AddRequiredItemToUI(ItemRequiredCraft itemRequiredCraft)
    {
        var itemObj = InventoryManager.Instance.inventory.GetItemByID(itemRequiredCraft.ID);

        if (itemObj == null)
        {
            AddMissingItemToUI(itemRequiredCraft);
        }
        else
        {
            AddExistingItemToUI(itemObj, itemRequiredCraft);
        }
    }

    // Thêm item bị thiếu vào UI
    private void AddMissingItemToUI(ItemRequiredCraft itemRequiredCraft)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemReward, holderCraft);
        var itemTooltipReward = objNew.GetComponent<ItemTooltipReward>();

        var itemReward = GameDataManager.Instance.GetItemRewardByID(itemRequiredCraft.ID);

        if (itemReward == null)
        {
            Debug.LogError("ItemReward is null for ID: " + itemRequiredCraft.ID);
            return;
        }

        itemTooltipReward.Avatar.sprite = itemReward.Image;
        SetCountText(itemTooltipReward.CountTxt, itemRequiredCraft.quantityRequired, GameDataManager.Instance.GetCountItemRewardById(itemRequiredCraft.ID));

        itemTooltipReward.ItemReward = itemReward;
        itemTooltipReward.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemReward.itemRarity);
    }

    // Thêm item tồn tại trong inventory vào UI
    private void AddExistingItemToUI(ItemObject itemObj, ItemRequiredCraft itemRequiredCraft)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemObject, holderCraft);
        var itemTooltip = objNew.GetComponent<ItemTooltipInventory>();

        itemTooltip.Avatar.sprite = itemObj.Sprite;
        SetCountText(itemTooltip.CountTxt, itemRequiredCraft.quantityRequired, InventoryManager.Instance.inventory.GetCountById(itemObj.IdDatabase));

        itemTooltip.ItemObject = itemObj;
        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemObj.itemRarity);
    }

    // Cập nhật Count Text với màu sắc tương ứng
    private void SetCountText(TMP_Text countTxt, int required, int current)
    {
        countTxt.alignment = TextAlignmentOptions.Center;

        if (current < required)
        {
            countTxt.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{current}</color>/{required}";
        }
        else
        {
            countTxt.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{current}</color>/{required}";
        }
    }

    private void OnButtonNext()
    {

    }
    private void OnButtonPrevious()
    {

    }
    private void OnCraftBtn()
    {

    }
}
