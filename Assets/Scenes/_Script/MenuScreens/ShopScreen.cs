using System;
using System.Collections.Generic;
using UnityEngine;
using UIGameDataManager;

public class ShopScreen: MonoBehaviour
{
    // string IDs
    const string k_CraftsScrollview = "Consumables/ViewPort/CraftContent";
    const string k_MedicinesScrollview = "Consumables/ViewPort/MedicineContent";
    const string k_SkillsScrollview = "Consumables/ViewPort/SkillContent";

    const string k_ItemsScrollview = "Items/ViewPort/ItemContent";
    const string k_RubysScrollview = "Rubys/ViewPort/RubyContent";
    [Header("Shop Item")]
    [Tooltip("ShopItem Element Asset to instantiate ")]
    [SerializeField] GameObject m_ShopItemPrefab;
    [SerializeField] GameIconsSO m_GameIconsData;

    // visual elements
    // each tab/parent element for each category of ShopItem
    GameObject m__CraftsScrollview;
    GameObject m_MedicinesScrollview;
    GameObject m_SkillsScrollview;

    GameObject m_ItemsScrollview;
    GameObject m_RubysScrollview;

    [SerializeField] GameObject m_Root;

    void OnEnable()
    {
        ShopController.ShopItemsTabRefilled += RefillShopTabItem;
    }

    void OnDisable()
    {
        ShopController.ShopItemsTabRefilled -= RefillShopTabItem;
    }

    protected  void Awake()
    {

        SetVisualElements();
    }

    //public void ShowScreen()
    //{
    //    m_TabbedMenu?.SelectFirstTab();
    //}

    protected  void SetVisualElements()
    {
        m__CraftsScrollview = m_Root.transform.Find(k_CraftsScrollview).gameObject;
        m_MedicinesScrollview = m_Root.transform.Find(k_MedicinesScrollview).gameObject;
        m_SkillsScrollview = m_Root.transform.Find(k_SkillsScrollview).gameObject;

        m_ItemsScrollview = m_Root.transform.Find(k_ItemsScrollview).gameObject;
        m_RubysScrollview = m_Root.transform.Find(k_RubysScrollview).gameObject;

    }

    // fill a tab with content
    public void RefillShopTabItem(List<ShopItemSO> shopItems)
    {
        if (shopItems == null || shopItems.Count == 0)
            return;

        Debug.Log("ShopItem: " + shopItems[0].contentType);

        // generate items 
        GameObject parentTab = null;
        switch (shopItems[0].contentType)
        {
            //Buy by Stone
            case ShopItemType.Craft:
                parentTab = m__CraftsScrollview;
                break;
            case ShopItemType.Medicine:
                parentTab = m_MedicinesScrollview;
                break;
            case ShopItemType.Skill:
                parentTab = m_SkillsScrollview;
                break;
            //Buy by Ruby
            case ShopItemType.Item:
                parentTab = m_ItemsScrollview;
                break;
            case ShopItemType.Watch:
                parentTab = m_RubysScrollview;
                break;
            case ShopItemType.Ruby:
                parentTab = m_RubysScrollview;
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
    void CreateShopItemElement(ShopItemSO shopItemData, GameObject parentElement)
    {
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
            shopItemController.UpdateBuyButton(shopItemData.contentValue >0);
        } 
        else
        {
            Debug.LogError("Shop item prefab is missing ShopItemComponent!");
        }

        //Is Watch
        if (shopItemData.contentType == ShopItemType.Watch)
        {
            shopItemController.CalculateWatchStatus();
        }
    }
}
