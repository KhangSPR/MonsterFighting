using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIGameDataManager;

public class ShopScreen: MonoBehaviour
{
    // string IDs
    const string k_CardCharacterScrollview = "Cards/PanelCard/Character/ViewPort/CharacterContent";
    const string k_CardMachineScrollview = "Cards/PanelCard/Machine/ViewPort/MachineContent";
    const string k_CardGuardScrollview = "Cards/PanelCard/Guard/ViewPort/GuardContent";
    const string k_XpScrollview = "Xps/ViewPort/XpContent";
    const string k_EnemyStoneScrollview = "EnemyStones/ViewPort/EnemyContent";
    const string k_RubyScrollview = "Rubys/ViewPort/RubyContent";
    const string k_ResourcePath = "GameData/GameIcons";

    [Header("Shop Item")]
    [Tooltip("ShopItem Element Asset to instantiate ")]
    [SerializeField] GameObject m_ShopItemPrefab;
    [SerializeField] GameObject m_ShopItemCardPrefab;
    [SerializeField] GameIconsSO m_GameIconsData;

    // visual elements
    // each tab/parent element for each category of ShopItem
    GameObject m_CardCharacterScrollview;
    GameObject m_CardMachineScrollview;
    GameObject m_CardGuardScrollview;
    GameObject m_XpScrollview;
    GameObject m_EnemyStoneScrollview;
    GameObject m_RubyScrollview;

    [SerializeField] GameObject m_Root;

    void OnEnable()
    {
        ShopController.ShopItemsTabRefilled += RefillShopTabItem;
        ShopController.ShopItemCardsTabRefilled += RefillShopTabItemCard;
    }

    void OnDisable()
    {
        ShopController.ShopItemsTabRefilled -= RefillShopTabItem;
        ShopController.ShopItemCardsTabRefilled -= RefillShopTabItemCard;
    }

    protected  void Awake()
    {
        // this ScriptableObject pairs data types (ShopItems, Skills, Rarity, Classes, etc.) with specific icons 
        // (default path = Resources/GameData/GameIcons)
        m_GameIconsData = Resources.Load<GameIconsSO>(k_ResourcePath);


        SetVisualElements();
    }

    //public void ShowScreen()
    //{
    //    m_TabbedMenu?.SelectFirstTab();
    //}

    protected  void SetVisualElements()
    {
        m_CardCharacterScrollview = m_Root.transform.Find(k_CardCharacterScrollview).gameObject;
        m_CardMachineScrollview = m_Root.transform.Find(k_CardMachineScrollview).gameObject;
        m_XpScrollview = m_Root.transform.Find(k_XpScrollview).gameObject;
        m_EnemyStoneScrollview = m_Root.transform.Find(k_EnemyStoneScrollview).gameObject;
        m_RubyScrollview = m_Root.transform.Find(k_RubyScrollview).gameObject;
    }

    // fill a tab with content
    public void RefillShopTabItem(List<ShopItemSO> shopItems)
    {
        if (shopItems == null || shopItems.Count == 0)
            return;

        Debug.Log("Refill");

        // generate items for each tab (gold, gems, potions)
        GameObject parentTab = null;
        switch (shopItems[0].contentType)
        {
            case ShopItemType.XpLv1:
                parentTab = m_XpScrollview;
                break;
            case ShopItemType.XpLv2:
                parentTab = m_XpScrollview;
                break;
            case ShopItemType.XpLv3:
                parentTab = m_XpScrollview;
                break;
            case ShopItemType.XpLv4:
                parentTab = m_XpScrollview;
                break;
            case ShopItemType.Gold:
                parentTab = m_EnemyStoneScrollview;
                break;
            case ShopItemType.Ruby:
                parentTab = m_RubyScrollview;
                break;
            default:
                parentTab = m_CardCharacterScrollview;
                break;
        }

        foreach (Transform child in parentTab.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItemSO shopItem in shopItems)
        {
            CreateShopItemElement(shopItem, parentTab);
        }
    }
    public void RefillShopTabItemCard(List<ShopItemCardSO> shopItems)
    {
        if (shopItems == null || shopItems.Count == 0)
            return;

        Debug.Log("Refill");

        // generate items for each tab (gold, gems, potions)
        GameObject parentTab = null;
        switch (shopItems[0].contentType)
        {
            case ShopItemType.CardCharacter:
                parentTab = m_CardCharacterScrollview;
                break;
            case ShopItemType.CardMachine:
                parentTab = m_CardMachineScrollview;
                Debug.Log(shopItems[0].contentType);
                break;
            case ShopItemType.CardGuard:
                parentTab = m_CardGuardScrollview;
                break;
            default:
                parentTab = m_CardCharacterScrollview;
                break;
        }

        foreach (Transform child in parentTab.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItemCardSO shopItem in shopItems)
        {
            CreateShopItemCardElement(shopItem, parentTab);
        }
    }
    void CreateShopItemElement(ShopItemSO shopItemData, GameObject parentElement)
    {
        Debug.Log("CreateShop");

        if (parentElement == null || shopItemData == null || m_ShopItemPrefab == null)
            return;

        // instantiate a new ShopItem prefab
        GameObject shopItemObj = Instantiate(m_ShopItemPrefab, parentElement.transform);

        shopItemObj.SetActive(true);

        // sets up the components and game data per ShopItem
        ShopItemComponent shopItemController = shopItemObj.GetComponent<ShopItemComponent>();
        if (shopItemController != null)
        {
            shopItemController.Initialize(m_GameIconsData, shopItemData);
            shopItemController.SetGameData(shopItemObj);
            shopItemController.RegisterCallbacks();

        }
        else
        {
            Debug.LogError("Shop item prefab is missing ShopItemComponent!");
        }
    }
    void CreateShopItemCardElement(ShopItemCardSO shopItemData, GameObject parentElement)
    {
        Debug.Log("CreateShop");

        if (parentElement == null || shopItemData == null || m_ShopItemPrefab == null)
            return;

        // instantiate a new ShopItem prefab
        GameObject shopItemObj = Instantiate(m_ShopItemCardPrefab, parentElement.transform);

        shopItemObj.SetActive(true);

        // sets up the components and game data per ShopItem
        ShopItemCardComponent shopItemController = shopItemObj.GetComponent<ShopItemCardComponent>();
        if (shopItemController != null)
        {
            shopItemController.Initialize(m_GameIconsData, shopItemData, shopItemData.cardComponent);
            shopItemController.SetGameData(shopItemObj);
            shopItemController.RegisterCallbacks();

        }
        else
        {
            Debug.LogError("Shop item prefab is missing ShopItemComponent!");
        }
    }
}
