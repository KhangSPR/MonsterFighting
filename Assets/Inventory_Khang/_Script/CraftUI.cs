using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using Unity.VisualScripting;
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
    //Dictionary ITEM REQUIRED 
    Dictionary<ItemTooltipInventory, int> itemObjectDictionary = new Dictionary<ItemTooltipInventory, int>();
    Dictionary<ItemTooltipReward, int> itemRewardDictionary = new Dictionary<ItemTooltipReward, int>();
    //Event
    public static Action OnCraft;

    private void ClearAndUpdate()
    {
        itemObjectDictionary.Clear();
        itemRewardDictionary.Clear();
    }
    ItemObject currentItemCraft
    {
        get { return itemObjects[currentItemObject]; }
    }

    int currentItemObject = 0;

    bool activeBtn = false;
    private void Start()
    {
        nextBtn.onClick.AddListener(OnButtonNext);
        previousBtn.onClick.AddListener(OnButtonPrevious);
        craftBtn.onClick.AddListener(() =>
        {
            OnCraftBtn();
        });
    }
    private void Update()
    {
        // Kiểm tra nếu lastClickTime > 0 thì giảm dần theo thời gian
        if (lastClickTime > 0)
        {
            lastClickTime -= Time.deltaTime;

            // Nếu lastClickTime <= 0, reset combo về 1
            if (lastClickTime <= 0)
            {
                AddItem();
                lastClickTime = 0; // Đảm bảo không giảm thêm
                number = 0;
            }
        }
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
    #region Display UI
    protected void SetItemObjets()
    {
        if (this.itemObjects == null)
        {
            Debug.LogError("Item objects lenght: " + itemObjects.Length);
        }
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (i == currentItemObject)
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

        activeBtn = true;

        SetButtonCraft(true);

        ClearAndUpdate();

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
            //currentItemObjects.Add(itemObj); //ADD LIST ITEM OBJECT
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
        int current = GameDataManager.Instance.GetCountItemRewardById(itemRequiredCraft.ID);
        int required = itemRequiredCraft.quantityRequired;

        itemRewardDictionary.Add(itemTooltipReward, required);//Dictionary
        SetCountText(itemTooltipReward.CountTxt, required, current);
        itemTooltipReward.ItemReward = itemReward;
        itemTooltipReward.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemReward.itemRarity);
    }
    // Thêm item tồn tại trong inventory vào UI
    private void AddExistingItemToUI(ItemObject itemObj, ItemRequiredCraft itemRequiredCraft)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemObject, holderCraft);
        var itemTooltip = objNew.GetComponent<ItemTooltipInventory>();

        itemTooltip.Avatar.sprite = itemObj.Sprite;
        int current = InventoryManager.Instance.inventory.GetCountById(itemObj.IdDatabase);
        int required = itemRequiredCraft.quantityRequired;

        itemObjectDictionary.Add(itemTooltip, required); //Dictionary
        SetCountText(itemTooltip.CountTxt, required, current);
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

            // Chỉ gọi SetButtonCraft khi phát hiện không đủ điều kiện
            SetButtonCraft(false);

            activeBtn = false;
        }
        else
        {
            countTxt.text = $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{current}</color>/{required}";
        }
    }
    #endregion
    private void OnCraftBtn()
    {
        // Animation - VFX - Number
        Bonus();
        fx.Image.sprite = itemObjects[currentItemObject].Sprite;
        fx.CraftEffect();

        Debug.Log("On Button");

        //REMOVE ITEM
        foreach (KeyValuePair<ItemTooltipInventory, int> kvp in itemObjectDictionary)
        {


            ItemTooltipInventory ItemTooltipInventory = kvp.Key;

            ItemObject itemObject = ItemTooltipInventory.ItemObject;

            Item ItemObjectKey = new Item(itemObject);

            int quantity = kvp.Value;

            InventoryManager.Instance.inventory.RemoveItem(ItemObjectKey, quantity); 

            int current = InventoryManager.Instance.inventory.GetCountById(itemObject.IdDatabase);

            SetCountText(ItemTooltipInventory.CountTxt, quantity, current);
        }
        foreach (KeyValuePair<ItemTooltipReward, int> kvp in itemRewardDictionary)
        {
            ItemTooltipReward itemTooltipReward = kvp.Key;

            ItemReward itemReward = itemTooltipReward.ItemReward;

            int quantity = kvp.Value;
            UIGameDataMap.Resources resource = new UIGameDataMap.Resources(itemReward, quantity);


            GameDataManager.Instance.OnReceiverRewardResources(resource);


            int current = GameDataManager.Instance.GetCountItemRewardById(itemReward.ID);

            SetCountText(itemTooltipReward.CountTxt, quantity, current);
        }
    }


    private void AddItem()
    {
        //ADD ITEM
        Item Item = new Item(currentItemCraft);
        InventoryManager.Instance.inventory.AddItem(Item, Number);

        Debug.Log("ADD ITEM NUMNER: " + Number);

        OnCraft?.Invoke();

    }
    #region VFX Craft
    [SerializeField] VFXCraft fx;

    [SerializeField] float clickInterval = 1.3f; // Thời gian cố định là 1.3 giây

    float lastClickTime = 0;
    public float LastClickTime => lastClickTime;
    [SerializeField]
    int number = 0;
    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            fx.CountText.text = "x" + Number.ToString();
            fx.BonusEffect();
        }
    }

    private void Bonus()
    {
        // Khi nhấn, đặt lastClickTime về clickInterval
        lastClickTime = clickInterval;

        // Tăng combo nếu vẫn trong khoảng thời gian clickInterval
        Number++;
    }
    #endregion

    private void OnButtonNext()
    {
        if (currentItemObject < itemObjects.Length - 1)
        {
            currentItemObject++;
        }
        else
        {
            currentItemObject = 0;

        }
        SetItemObjets();
    }
    private void OnButtonPrevious()
    {
        if (currentItemObject == 0)
        {
            currentItemObject = itemObjects.Length - 1;
        }
        else
        {
            currentItemObject--;
        }
        SetItemObjets();
    }
    private void SetButtonCraft(bool active)
    {
        if (!activeBtn) return;

        craftBtn.enabled = active;
        if (active)
        {
            craftBtn.GetComponent<Image>().color = new Color(178 / 255f, 112 / 255f, 0f, 1f);
            craftBtn.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            craftBtn.GetComponent<Image>().color = new Color(178 / 255f, 112 / 255f, 0f, 120 / 255f);
            craftBtn.GetComponentInChildren<TMP_Text>().color = new Color(1, 1, 1, 120 / 255f);
        }
    }
}
