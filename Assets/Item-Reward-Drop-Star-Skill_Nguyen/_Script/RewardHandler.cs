using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIGameDataManager;

public class RewardHandler : MonoBehaviour
{
    public List<UIGameDataMap.Resources> resourceItems;
    public List<InventoryItem> inventoryItems;

    public TMP_Text textTimer;
    public Button btnDone;
    public Transform panel;
    public Transform vfxReward;

    private Animator animator;

    [SerializeField] private int rewardResetHours;
    [SerializeField] private int rewardResetMinutes;
    [SerializeField] private PanelItemReward paneltemReward;


    private RewardTimerHandler timerHandler;
    [SerializeField] private bool hasRewardItems = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        timerHandler = new RewardTimerHandler();
    }

    private void Start()
    {
        ActiveRewardStart();
        btnDone.onClick.AddListener(RewardItem);
    }
    private void ActiveRewardStart()
    {
        hasRewardItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);
        SetActiveReward(hasRewardItems);
    }
    private void Update()
    {
        timerHandler.UpdateTimer();
        hasRewardItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);
        ShowTimerUI();

        if (hasRewardItems)
        {
            SetActiveReward(hasRewardItems);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            timerHandler.ResetTimer();
        }
    }
    bool isHasTrue = false;
    private void SetActiveReward(bool active)
    {
        if (isHasTrue) return;
        vfxReward.gameObject.SetActive(active);
        animator.Rebind();      
        animator.enabled = active;
        btnDone.gameObject.SetActive(active);
        if(active)
            isHasTrue = true;
        Debug.Log("Set SetActiveReward");
    }

    private void ShowTimerUI()
    {
        textTimer.text = hasRewardItems ? "" : timerHandler.GetTimerString();
    }
    public void RewardItem()
    {
        isHasTrue = false;
        timerHandler.SetNextDay000(rewardResetHours, rewardResetMinutes);
        timerHandler.SaveTimer();
        ShowPanelUI();
        SetActiveReward(false);
    }

    private void ShowPanelUI()
    {
        panel.gameObject.SetActive(true);
        ClearRewardList();

        var createdItems = new List<GameObject>();

        CreateRewardItems(resourceItems, createdItems, CreateResourceItem);
        CreateRewardItems(inventoryItems, createdItems, CreateInventoryItem);

        RewardClaimManager.Instance.PlayItemsWithAnimation(createdItems, 0.65f);

        paneltemReward.SetFade(120);
        paneltemReward.SetPanels(createdItems);

    }

    private void ClearRewardList()
    {
        foreach (Transform child in panel.Find("ListItemReward"))
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateRewardItems<T>(List<T> items, List<GameObject> createdItems, Func<T, GameObject> createItemCallback)
    {
        foreach (var item in items)
        {
            var objNew = createItemCallback(item);
            createdItems.Add(objNew);
        }
    }

    private GameObject CreateResourceItem(UIGameDataMap.Resources resource)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemReward, panel.Find("ListItemReward")).gameObject;
        objNew.SetActive(false);

        var itemTooltip = objNew.GetComponent<ItemTooltipReward>();
        itemTooltip.Avatar.sprite = resource.item.Image;
        itemTooltip.CountTxt.text = $"x{resource.Count}";
        itemTooltip.ItemReward = resource.item;
        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resource.item.itemRarity);

        GameDataManager.Instance.OnReceiverRewardResources(resource);

        return objNew;
    }

    private GameObject CreateInventoryItem(InventoryItem item)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemObject, panel.Find("ListItemReward")).gameObject;
        objNew.SetActive(false);

        var itemTooltip = objNew.GetComponent<ItemTooltipInventory>();
        itemTooltip.Avatar.sprite = item.itemObject.Sprite;
        itemTooltip.CountTxt.text = $"x{item.count}";
        itemTooltip.ItemObject = item.itemObject;
        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(item.itemObject.itemRarity);

        InventoryManager.Instance.inventory.AddItem(new Item(item.itemObject), item.count);

        return objNew;
    }
}
