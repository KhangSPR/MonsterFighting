using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;


// presenter/controller for the ShopScreen
public class ShopController : MonoBehaviour
{
    // notify ShopScreen (pass ShopItem data + screen pos of buy button)
    public static event Action<ShopItemSO, ShopItemComponent, Vector2> ShopItemPurchasing;
    public static event Action<List<ShopItemSO>> ShopItemsTabRefilled;

    [Tooltip("Path within the Resources folders for MailMessage ScriptableObjects.")]
    [SerializeField] string m_ResourcePathItems = "GameData/ShopItems";

    // ScriptableObject game data from Resources
    [Header("Shop Item ALL")]
    List<ShopItemSO> m_ShopItems = new List<ShopItemSO>();    
    [Header("Shop Item")]
    List<ShopItemSO> m_CraftShopItems = new List<ShopItemSO>();
    List<ShopItemSO> m_MedicineShopItems = new List<ShopItemSO>();
    List<ShopItemSO> m_SkillShopItems = new List<ShopItemSO>();



    List<ShopItemSO> m_ItemShopItems = new List<ShopItemSO>();
    List<ShopItemSO> m_RubyShopItems = new List<ShopItemSO>();

    private ShopTimerHandler timerHandler;
    [SerializeField] private int rewardResetHours;
    [SerializeField] private int rewardResetMinutes;
    void Start()
    {
        LoadShopDataItem();
        UpdateViewItems();
    }
    void OnEnable()
    {
        ShopItemComponent.ShopItemClicked += OnTryBuyItem;
    }

    void OnDisable()
    {
        ShopItemComponent.ShopItemClicked -= OnTryBuyItem;
    }
    private void OnApplicationFocus(bool hasFocus) //APly Android
    {
        if (!hasFocus) // Mất tiêu điểm
        {
            Save(); 
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    #region ShopTimerHandler

    private void Awake()
    {
        timerHandler = new ShopTimerHandler();
        LoadResourcesItem();
        Load(); //Save Data
    }

    private void Update()
    {
        timerHandler.UpdateTimer();

        if (timerHandler.TimerDelta.TotalSeconds <= 0)
        {
            UpdateItemShopPurchase();
            UpdateViewItems();
            timerHandler.SetNextDay000(0, 0);
            timerHandler.SaveTimer();

            Debug.Log("Update ShopTimerHandle");
        }
    }
    void UpdateItemShopPurchase()
    {
        foreach (var shopItem in m_ShopItems)
        {
            shopItem.contentValue = shopItem.maxValue;

            if (shopItem.contentType == ShopItemType.Watch)
            {
                int IsCalculate = PlayerPrefs.GetInt("WatchItem");
                if (IsCalculate == 1)
                {
                    PlayerPrefs.SetInt(shopItem.itemName, 0);
                }
            }
        }
    }
    #endregion
    #region LoadItemShopUI
    private void LoadResourcesItem()
    {
        // load the ScriptableObjects from the Resources directory (default = Resources/GameData/MailMessages)
        m_ShopItems.AddRange(Resources.LoadAll<ShopItemSO>(m_ResourcePathItems));
    }

    // fill the ShopScreen with data
    void LoadShopDataItem()
    {
        m_CraftShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Craft).ToList();
        m_MedicineShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Medicine).ToList();
        m_SkillShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Skill).ToList();


        m_ItemShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Item).ToList();
        m_RubyShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Ruby || c.contentType == ShopItemType.Watch).ToList();


        m_CraftShopItems = SortShopItems(m_CraftShopItems);
        m_MedicineShopItems = SortShopItems(m_MedicineShopItems);
        m_SkillShopItems = SortShopItems(m_SkillShopItems);

        m_ItemShopItems = SortShopItems(m_ItemShopItems);
        m_RubyShopItems = SortShopItems(m_RubyShopItems);
    }
    List<ShopItemSO> SortShopItems(List<ShopItemSO> originalList)
    {
        return originalList.OrderBy(x => x.cost).ToList();
    }
    public void UpdateViewItems()
    {
        ShopItemsTabRefilled?.Invoke(m_CraftShopItems);
        ShopItemsTabRefilled?.Invoke(m_MedicineShopItems);
        ShopItemsTabRefilled?.Invoke(m_SkillShopItems);
        ShopItemsTabRefilled?.Invoke(m_ItemShopItems);
        ShopItemsTabRefilled?.Invoke(m_RubyShopItems);
    }
    // try to buy the item, pass the screen coordinate of the buy button
    public void OnTryBuyItem(ShopItemSO shopItemData, ShopItemComponent shopItemComponent, Vector2 screenPos)
    {
        if (shopItemData == null)
            return;
        Debug.Log("OnTryBuyItem");
        // notify other objects we are trying to buy an item
        ShopItemPurchasing?.Invoke(shopItemData, shopItemComponent, screenPos);
    }
    #endregion
    #region Load - Save Resources
    private void Save()
    {
        foreach (var shopItem in m_ShopItems)
        {
            PlayerPrefs.SetInt(shopItem.ID, (int)shopItem.contentValue);
        }
        PlayerPrefs.Save();
    }

    private void Load()
    {
        foreach (var shopItem in m_ShopItems)
        {
            if (PlayerPrefs.HasKey(shopItem.ID))
            {
                int contentValue = PlayerPrefs.GetInt(shopItem.ID);

                if (contentValue > 0)
                {
                    if (contentValue > shopItem.maxValue)
                    {
                        contentValue = (int)shopItem.maxValue;
                    }
                    shopItem.contentValue = (uint)contentValue;
                }
                else
                {
                    shopItem.contentValue = 5; //Repair
                    //Reload when day passes
                    //shopItem.contentValue = (uint)shopItem.maxValue;
                }
            }
        }
    }
    #endregion
}
