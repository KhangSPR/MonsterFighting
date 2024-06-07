using System;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class RewardItem
{
    [SerializeField] private Item item; public Item Item => item;
    [SerializeField] private int count; public int Count => count;
}

public class UIRewardItems : MonoBehaviour
{
    // Logic
    
    public string TimerStr;
    public bool hasRewardItems;
    public List<RewardItem> rewardItems; // Item - Count
                                         // UI
    public Text textTimer;
    public Image isDone;
    public Transform Panel;
    // Prefab 
    public Transform ItemPrefab;
    public GameObject UIRewardItemGO;
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
        isDone.enabled = false;

        textTimer.text = hasRewardItems ? "" : ShowTimerString();
        isDone.enabled = hasRewardItems;
        if(GetComponent<Button>()!=null) GetComponent<Button>().interactable = hasRewardItems;

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
        foreach(Transform o in Panel.Find("List Item Reward"))
        {
            Destroy(o.gameObject);
        }
        foreach(var slot in rewardItems)
        {
            GameObject o = Instantiate(ItemPrefab, Panel.Find("List Item Reward")).gameObject;
            o.transform.Find("Img").GetComponent<Image>().sprite = slot.Item.Image;
            o.transform.Find("Count").GetComponent<Text>().text = $"x{slot.Count}";

            GameDataManager.Instance.inventory.AddOrChangeQuantity(slot.Item, slot.Count);
        }
        Panel.GetComponent<Button>().onClick.AddListener(() =>
        {
            Panel.gameObject.SetActive(false);
        });
    }
    private void SetNextDay()
    {
        TimerNextDay = new DateTime(TimerNow.Year, TimerNow.Month, TimerNow.Day +1, RewardResetHours/**/, RewardResetMinute/*RewardResetMinute+1*/, 0, 0);
    }

    public string ShowTimerString()
    {
        TimerNowStr = $"{TimerNow.Year}Y:{TimerNow.Month}M,{TimerNow.Day}D:{TimerNow.Hour}H:{TimerNow.Minute}M:{TimerNow.Second}S:{TimerNow.Millisecond}MS";
        TimerNextDayStr = $"{TimerNextDay.Year}Y:{TimerNextDay.Month}M,{TimerNextDay.Day}D:{TimerNextDay.Hour}H:{TimerNextDay.Minute}M:{TimerNextDay.Second}S:{TimerNextDay.Millisecond}MS";
        TimerDeltaStr = $"";
        TimerDeltaStr = $"{TimerDelta.Hours}H:{TimerDelta.Minutes}M:{TimerDelta.Seconds}S";

        return TimerDeltaStr;
    }
}
    
