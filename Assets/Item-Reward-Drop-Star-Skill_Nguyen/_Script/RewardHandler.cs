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

    private TimerHandler timerHandler;
    private Animator animator;

    [SerializeField] private int rewardResetHours;
    [SerializeField] private int rewardResetMinutes;
    [SerializeField] private PaneltemReward paneltemReward;
    [SerializeField]
    private bool hasRewardItems = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        timerHandler = new TimerHandler(rewardResetHours, rewardResetMinutes);
        SetActiveReward(false);
    }

    private void Start()
    {
        ActiveRewardStart();
        btnDone.onClick.AddListener(RewardItem);
    }
    private void ActiveRewardStart()
    {
        timerHandler.LoadTimer();

        hasRewardItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);

        SetActiveReward(hasRewardItems);
    }
    private void Update()
    {
        timerHandler.UpdateTimer();
        ShowTimerUI();
        hasRewardItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);


        if (Input.GetKeyDown(KeyCode.J))
        {
            timerHandler.RemoveTimerKey();
            timerHandler.ResetTimer();
            SetActiveReward(true);
        }
    }

    private void SetActiveReward(bool active)
    {
        vfxReward.gameObject.SetActive(active);
        animator.Rebind();      
        animator.enabled = active;
        btnDone.gameObject.SetActive(active);
    }

    private void ShowTimerUI()
    {
        textTimer.text = hasRewardItems ? "" : timerHandler.GetTimerString();

        //Debug.Log("Timer Handerler: "+ hasRewardItems);
    }
    public void RewardItem()
    {
        timerHandler.SetNextDay(rewardResetHours, rewardResetMinutes);
        ShowPanelUI();
        timerHandler.SaveTimer();
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
