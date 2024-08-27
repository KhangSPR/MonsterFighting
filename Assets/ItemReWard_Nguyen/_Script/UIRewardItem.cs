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
    public List<InventoryItemReward> inventoryItems; // Item - Count


    [Space]
    [Space]
    [Header("UI")]
    public TMP_Text textTimer;
    public Image btnDone;
    public Transform Panel;
    // Prefab 
    public Transform ItemPrefab;
    //public GameObject UIRewardItemGO;
    // Time reset
    public int RewardResetHours;
    public int RewardResetMinute;
    private void Awake()
    {
        TimerNow = DateTime.Now;
        TimerNextDay = TimerNow.AddDays(-1);
        TimerNextDay = new DateTime(TimerNow.Year, TimerNow.Month, TimerNow.Day, RewardResetHours, RewardResetMinute, 0, 0);

    }

    public DateTime TimerNow;
    public DateTime TimerNextDay;
    public TimeSpan TimerDelta;
    public string TimerNowStr;
    public string TimerNextDayStr;
    public string TimerDeltaStr;

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

        if(btnDone.enabled)
        {
            btnDone.gameObject.SetActive(true);
        }
        else
        {
            btnDone.gameObject.SetActive(false);
        }
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

        SetNextDay();
        ShowPanelUI();
    }
    public void ShowPanelUI()
    {
        Panel.gameObject.SetActive(true);
        foreach (Transform o in Panel.Find("List Item Reward"))
        {
            Destroy(o.gameObject);
        }
        foreach (var resource in resoureceItems)
        {
            GameObject objNew = Instantiate(ItemPrefab, Panel.Find("List Item Reward")).gameObject;
            objNew.transform.Find("Img").GetComponent<Image>().sprite = resource.item.Image;
            objNew.transform.Find("Count").GetComponent<Text>().text = $"x{resource.Count}";
            objNew.transform.GetComponent<ItemTooltip>().ItemReward = resource.item;


            //ADD Item
            GameDataManager.Instance.OnReceiverRewardResources(resource);

        }
        foreach(var item in inventoryItems)
        {
            Item Item = new Item(item.itemObject);

            GameObject objNew = Instantiate(ItemPrefab, Panel.Find("List Item Reward")).gameObject;


            objNew.transform.GetComponent<ItemTooltip>().ItemObject = item.itemObject;
            objNew.transform.Find("Img").GetComponent<Image>().sprite = item.itemObject.Sprite;
            objNew.transform.Find("Count").GetComponent<Text>().text = $"x{item.count}";

            InventoryManager.Instance.inventory.AddItem(Item, item.count);
        }
        Panel.GetComponent<Button>().onClick.AddListener(() =>
        {
            Panel.gameObject.SetActive(false);
        });
    }
    private void SetNextDay()
    {
        TimerNextDay = new DateTime(TimerNow.Year, TimerNow.Month, TimerNow.Day + 1, RewardResetHours, RewardResetMinute, 0, 0);
    }

    public string ShowTimerString()
    {
        TimerDeltaStr = $"{TimerDelta.Hours}H:{TimerDelta.Minutes}M:{TimerDelta.Seconds}S";
        return TimerDeltaStr;
    }
}

