using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


// presenter/controller for the ShopScreen
public class ShopController : MonoBehaviour
{
    // notify ShopScreen (pass ShopItem data + screen pos of buy button)
    public static event Action<ShopItemSO, Vector2> ShopItemPurchasing;
    public static event Action<ShopItemCardComponent,ShopItemCardSO, Vector2> ShopItemCardPurchasing;
    public static event Action<List<ShopItemSO>> ShopItemsTabRefilled;
    public static event Action<List<ShopItemCardSO>> ShopItemCardsTabRefilled;

    [Tooltip("Path within the Resources folders for MailMessage ScriptableObjects.")]
    [SerializeField] string m_ResourcePathItems = "GameData/ShopItems";
    [SerializeField] string m_ResourcePathCards = "GameData/ShopItemCards";

    // ScriptableObject game data from Resources
    [Header("Shop Item ALL")]
    [SerializeField] List<ShopItemSO> m_ShopItems = new List<ShopItemSO>();
    [Header("Shop Item Card")]
    [SerializeField] List<ShopItemCardSO> m_ShopItemCards = new List<ShopItemCardSO>();
    // game data filtered in categories
    [Header("Shop Item Card")]
    [SerializeField] List<ShopItemCardSO> m_CardCharacters = new List<ShopItemCardSO>();
    [SerializeField] List<ShopItemCardSO> m_CardMachines= new List<ShopItemCardSO>();
    [SerializeField] List<ShopItemCardSO> m_CardGuards = new List<ShopItemCardSO>();
    [Header("Shop Item")]
    [SerializeField] List<ShopItemSO> m_XpShopItems = new List<ShopItemSO>();
    [SerializeField] List<ShopItemSO> m_EnemyStoneShopItems = new List<ShopItemSO>();
    [SerializeField] List<ShopItemSO> m_RubyShopItems = new List<ShopItemSO>();

    void OnEnable()
    {
        ShopItemComponent.ShopItemClicked += OnTryBuyItem;
        ShopItemCardComponent.ShopItemClicked += OnTryBuyItemCard;

    }

    void OnDisable()
    {
        ShopItemComponent.ShopItemClicked -= OnTryBuyItem;
        ShopItemCardComponent.ShopItemClicked -= OnTryBuyItemCard;
    }

    void Start()
    {
        LoadShopDataItem();
        LoadShopDataCard();
        UpdateViewItems();
        UpdateViewItemCards();
    }

    // fill the ShopScreen with data
    void LoadShopDataItem()
    {
        // load the ScriptableObjects from the Resources directory (default = Resources/GameData/MailMessages)
        m_ShopItems.AddRange(Resources.LoadAll<ShopItemSO>(m_ResourcePathItems));

        m_XpShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.XpLv1 || c.contentType == ShopItemType.XpLv2 || c.contentType == ShopItemType.XpLv3 || c.contentType == ShopItemType.XpLv4).ToList();
        m_EnemyStoneShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Gold).ToList();
        m_RubyShopItems = m_ShopItems.Where(c => c.contentType == ShopItemType.Ruby).ToList();

        m_XpShopItems = SortShopItems(m_XpShopItems);
        m_EnemyStoneShopItems = SortShopItems(m_EnemyStoneShopItems);
        m_RubyShopItems = SortShopItems(m_RubyShopItems);
    }
    void LoadShopDataCard()
    {
        // load the ScriptableObjects from the Resources directory (default = Resources/GameData/MailMessages)
        m_ShopItemCards.AddRange(Resources.LoadAll<ShopItemCardSO>(m_ResourcePathCards));

        // sort them by type
        m_CardCharacters = m_ShopItemCards.Where(c => c.contentType == ShopItemType.CardCharacter).ToList();
        m_CardMachines = m_ShopItemCards.Where(c => c.contentType == ShopItemType.CardMachine).ToList();
        m_CardGuards = m_ShopItemCards.Where(c => c.contentType == ShopItemType.CardMachine).ToList();


        m_CardCharacters = SortShopItemCards(m_CardCharacters);
        m_CardMachines = SortShopItemCards(m_CardMachines);
        m_CardGuards = SortShopItemCards(m_CardGuards);

        m_CardCharacters = FilterShopItemsWithoutBuy(m_CardCharacters);
        m_CardMachines = FilterShopItemsWithoutBuy(m_CardMachines);
        m_CardGuards = FilterShopItemsWithoutBuy(m_CardGuards);
    }
    List<ShopItemSO> SortShopItems(List<ShopItemSO> originalList)
    {
        return originalList.OrderBy(x => x.cost).ToList();
    }
    List<ShopItemCardSO> SortShopItemCards(List<ShopItemCardSO> originalList)
    {
        return originalList.OrderBy(x => x.cost).ToList();
    }
    List<ShopItemCardSO> FilterShopItemsWithoutBuy(List<ShopItemCardSO> originalList)
    {
        List<ShopItemCardSO> filteredList = originalList.Where(item => !item.hasBuy).ToList();
        return filteredList;
    }
    public void UpdateViewItems()
    {
        ShopItemsTabRefilled?.Invoke(m_XpShopItems);
        ShopItemsTabRefilled?.Invoke(m_EnemyStoneShopItems);
        ShopItemsTabRefilled?.Invoke(m_RubyShopItems);
    }
    public void UpdateViewItemCards()
    {
        ShopItemCardsTabRefilled?.Invoke(m_CardCharacters);
        ShopItemCardsTabRefilled?.Invoke(m_CardMachines);
        ShopItemCardsTabRefilled?.Invoke(m_CardGuards);
    }

    // try to buy the item, pass the screen coordinate of the buy button
    public void OnTryBuyItem(ShopItemSO shopItemData, Vector2 screenPos)
    {
        if (shopItemData == null)
            return;
        Debug.Log("OnTryBuyItem");
        // notify other objects we are trying to buy an item
        ShopItemPurchasing?.Invoke(shopItemData, screenPos);
    }
    public void OnTryBuyItemCard(ShopItemCardComponent shopItemCardComponent, ShopItemCardSO shopItemData, Vector2 screenPos)
    {
        if (shopItemData == null)
            return;
        Debug.Log("OnTryBuyItemCard");
        // notify other objects we are trying to buy an item
        ShopItemCardPurchasing?.Invoke(shopItemCardComponent,shopItemData, screenPos);
    }
}
