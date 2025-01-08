using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;


public class UIRewardItems : MonoBehaviour
{
    // Logic

    public string TimerStr;
    public bool hasRewardItems;
    public List<UIGameDataMap.Resources> resoureceItems; // Item - Count
    public List<InventoryItem> inventoryItems; // Item - Count


    [Space]
    [Space]
    [Header("UI")]
    public TMP_Text textTimer;
    public Image btnDone;
    public Transform Panel;
    public Transform VFXReward;
    // Prefab 
    public Transform ItemPrefab;
    //public GameObject UIRewardItemGO;
    // Time reset
    int RewardResetHours;
    int RewardResetMinute;
    private void Awake()
    {
        TimerNow = DateTime.Now;
        TimerNextDay = TimerNow.AddDays(-1);
        TimerNextDay = new DateTime(TimerNow.Year, TimerNow.Month, TimerNow.Day, RewardResetHours, RewardResetMinute, 0, 0);

    }

    public DateTime TimerNow;
    public DateTime TimerNextDay;
    public TimeSpan TimerDelta;
    public string TimerDeltaStr;
    [SerializeField] PaneltemReward paneltemReward;

    Animator Animator;
    private void Start()
    {
        Animator = transform.GetComponent<Animator>();
        SetActiveReward(false);
    }

    private void OnEnable()
    {
        LoadTimerReward();

    }
    private void OnDisable()
    {
        SaveTimerRewarded();
    }
    private void OnApplicationQuit()
    {
        SaveTimerRewarded();
    }
    void LoadTimerReward()
    {
        if (PlayerPrefs.HasKey("RewardTimerSaved"))
        {
            var RewardTimerSaved = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString("RewardTimerSaved")));
            TimerNextDay = RewardTimerSaved;
        }
    }
    private void SetActiveReward(bool active)
    {
        VFXReward.gameObject.SetActive(active);
        Animator.enabled = active;

    }
    void SaveTimerRewarded()
    {
        PlayerPrefs.SetString("RewardTimerSaved", TimerNextDay.ToBinary().ToString());
        PlayerPrefs.Save();
    }
    private void Update()
    {
        TimerNow = DateTime.Now;
        TimerDelta = TimerNextDay - TimerNow;
        hasRewardItems = (TimerDelta.TotalMilliseconds < 0);
    }
    private void LateUpdate()
    {
        ShowTimerUI();
    }
    public void ShowTimerUI()
    {
        textTimer.text = "";
        btnDone.enabled = false;
        //isDone.gameObject.SetActive(true);


        textTimer.text = hasRewardItems ? "" : ShowTimerString();
        btnDone.enabled = hasRewardItems;

        SetActiveReward(btnDone.enabled);


        if (GetComponent<Button>() != null) GetComponent<Button>().interactable = hasRewardItems;

    }
    [ContextMenu("RemoveRewarddTimerKey")]
    public void RemoveRewarddTimerKey()
    {
        PlayerPrefs.DeleteKey("RewardTimerSaved");
    }
    [ContextMenu("Reward")]
    public void RewardItem()
    {
        // Set the next day's timer immediately after receiving the reward
        SetNextDay();

        // Show the reward UI
        ShowPanelUI();

        // Save the new timer value
        SaveTimerRewarded();

        // Update the timer UI
        ShowTimerUI();
    }
    public void ShowPanelUI()
    {
        Panel.gameObject.SetActive(true);

        // Xóa các item cũ trong danh sách
        foreach (Transform o in Panel.Find("ListItemReward"))
        {
            Destroy(o.gameObject);
        }

        // Danh sách các item sẽ được tạo
        var createdItems = new List<GameObject>();

        // Hiển thị các phần thưởng
        foreach (var resource in resoureceItems)
        {
            GameObject objNew = Instantiate(RewardClaimManager.Instance.ItemReward, Panel.Find("ListItemReward")).gameObject;

            objNew.SetActive(false);
            ItemTooltipReward itemTooltip = objNew.GetComponent<ItemTooltipReward>();
            itemTooltip.Avatar.sprite = resource.item.Image;
            itemTooltip.CountTxt.text = $"x{resource.Count}";
            itemTooltip.ItemReward = resource.item;

            // Rairity Material
            itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resource.item.itemRarity);

            // ADD Item
            GameDataManager.Instance.OnReceiverRewardResources(resource);

            // Thêm vào danh sách để xử lý animation
            createdItems.Add(objNew);
        }

        // Hiển thị các item trong inventory
        foreach (var item in inventoryItems)
        {
            Item Item = new Item(item.itemObject);

            GameObject objNew = Instantiate(RewardClaimManager.Instance.ItemObject, Panel.Find("ListItemReward")).gameObject;

            objNew.SetActive(false);
            ItemTooltipInventory itemTooltip = objNew.GetComponent<ItemTooltipInventory>();
            itemTooltip.Avatar.sprite = item.itemObject.Sprite;
            itemTooltip.CountTxt.text = $"x{item.count}";
            itemTooltip.ItemObject = item.itemObject;

            // Rairity Material
            itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(item.itemObject.itemRarity);

            InventoryManager.Instance.inventory.AddItem(Item, item.count);

            // Thêm vào danh sách để xử lý animation
            createdItems.Add(objNew);
        }

        // Chạy animation cho các item đã tạo
        RewardClaimManager.Instance.PlayItemsWithAnimation(createdItems, 1.3f / 2);

        paneltemReward.SetFade(120);
        paneltemReward.SetPanels(createdItems);

        // Gán sự kiện cho nút hoàn thành
        btnDone.GetComponent<Button>().onClick.AddListener(() =>
        {
            Panel.gameObject.SetActive(false); // Đóng panel khi hoàn thành
        });
    }



    public string ShowTimerString()
    {
        TimerDeltaStr = $"{TimerDelta.Hours}:{TimerDelta.Minutes}:{TimerDelta.Seconds}s";
        return TimerDeltaStr;
    }
    private void SetNextDay()
    {
        // Set the next day to the same time tomorrow
        TimerNextDay = TimerNow.AddDays(1).Date.AddHours(RewardResetHours).AddMinutes(RewardResetMinute);
    }
}

