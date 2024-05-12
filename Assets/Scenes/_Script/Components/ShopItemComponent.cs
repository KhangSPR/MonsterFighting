using System;
using UnityEngine;
using UnityEngine.UI;
using UIGameDataManager;
using TMPro;


// represents one item in the shop
public class ShopItemComponent: MonoBehaviour
{
    // notify the ShopScreenController to buy product (passes the ShopItem data + UI element screen position)
    public static event Action<ShopItemSO, Vector2> ShopItemClicked;

    // ScriptableObject pairing icons with currency/shop item types
    GameIconsSO m_GameIconsData;
    ShopItemSO m_ShopItemData;

    // string IDs
    //const string k_ParentContainer = "SizeContainer";
    //const string k_Description = "Description";
    //const string k_ProductImage = "ProductImage";

    ////const string k_Banner = "shop-item__banner";
    ////const string k_BannerLabel = "shop-item__banner-label";

    //const string k_DiscountBadge = "shop-item__discount-badge";
    //const string k_DiscountLabel = "shop-item__badge-text";

    ////const string k_ContentCurrency = "shop-item__content-currency";
    //const string k_ContentValue = "ContentValue";
    //const string k_ContentIcon = "ContentIcon";
    //const string k_ContentGroup = "ContentGroup";

    //const string k_CostPrice = "CostPrice";
    //const string k_CostIcon = "CostIcon";
    //const string k_CostGroup = "CostGroup";

    //const string k_DiscountPrice = "shop-item__discount-price";
    //const string k_DiscountIcon = "shop-item__discount-icon";
    //const string k_DiscountSlash = "shop-item__discount-slash";
    //const string k_DiscountGroup = "shop-item__discount-group";

    //const string k_BuyButton = "CostGroup";


    // visual elements
    //Text
    [Header("Text")]
    [SerializeField] TMP_Text m_Description;
    [SerializeField] TMP_Text m_ContentValue;
    [SerializeField] TMP_Text m_CostPrice;
    [SerializeField] TMP_Text m_CostUSD;
    //Text m_DiscountLabel;
    //Text m_DiscountCost;

    //Image
    [Header("Image")]
    [SerializeField] Image m_ProductImage;
    [SerializeField] Image m_CostIcon;
    [SerializeField] Image m_DiscountIcon;
    [SerializeField] Image m_ContentIcon;

    //GameObject
    [Header("GameObject")]
    [SerializeField] GameObject m_SizeContainer;
    //GameObject m_DiscountBadge;
    //GameObject m_DiscountSlash;
    //GameObject m_DiscountGroup;
    [SerializeField] GameObject m_ContentGroup;
    [SerializeField] GameObject m_ContentIconGroup;
    [SerializeField] GameObject m_CostIconGroup;
    [SerializeField] GameObject m_CostGroup;
    [SerializeField] GameObject m_ProductItem;
    [SerializeField] GameObject m_CostUSDGroup;

    [Header("Button")]
    [SerializeField] Button m_BuyButton;

    float wideSize = 200f;
    float normalSize = 100f;
    public ShopItemComponent(GameIconsSO gameIconsData, ShopItemSO shopItemData)
    {
        m_GameIconsData = gameIconsData;
        m_ShopItemData = shopItemData;
    }
    public void Initialize(GameIconsSO gameIconsData, ShopItemSO shopItemData)
    {
        m_GameIconsData = gameIconsData;
        m_ShopItemData = shopItemData;

    }
    //public void SetUI(GameObject shopItemElement)
    //{
    //    // query the parts of the ShopItemElement
    //    m_SizeContainer = shopItemElement.transform.Find(k_ParentContainer).gameObject;
    //    m_Description = shopItemElement.transform.Find(k_Description).GetComponent<Text>();
    //    m_ProductImage = shopItemElement.transform.Find(k_ProductImage).GetComponent<Image>();

    //    //m_DiscountBadge = shopItemElement.transform.Find(k_DiscountBadge).gameObject;
    //    //m_DiscountLabel = shopItemElement.transform.Find(k_DiscountLabel).GetComponent<Text>();

    //    m_ContentGroup = shopItemElement.transform.Find(k_ContentGroup).gameObject;
    //    m_ContentIcon = shopItemElement.transform.Find(k_ContentIcon).GetComponent<Image>();
    //    m_ContentValue = shopItemElement.transform.Find(k_ContentValue).GetComponent<Text>();

    //    //m_DiscountSlash = shopItemElement.transform.Find(k_DiscountSlash).gameObject;
    //    //m_DiscountIcon = shopItemElement.transform.Find(k_DiscountIcon).GetComponent<Image>();
    //    //m_DiscountGroup = shopItemElement.transform.Find(k_DiscountGroup).gameObject;
    //    //m_DiscountCost = shopItemElement.transform.Find(k_DiscountPrice).GetComponent<Text>();

    //    m_CostIcon = shopItemElement.transform.Find(k_CostIcon).GetComponent<Image>();
    //    m_CostPrice = shopItemElement.transform.Find(k_CostPrice).GetComponent<Text>();
    //    m_CostGroup = shopItemElement.transform.Find(k_CostGroup).gameObject;

    //    m_BuyButton = shopItemElement.transform.Find(k_BuyButton).GetComponent<Button>();


    //}
    // show the ScriptableObject data
    public void SetGameData(GameObject shopItemElement)
    {
        if (m_GameIconsData == null)
        {
            Debug.LogWarning("ShopItemController SetGameData: missing GameIcons ScriptableObject data");
            return;
        }


        if (shopItemElement == null)
            return;

        // basic description and image
        m_Description.text = m_ShopItemData.itemName;
        m_ProductImage.sprite = m_ShopItemData.sprite;


        // content value
        m_ContentIcon.sprite = m_GameIconsData.GetShopTypeIcon(m_ShopItemData.contentType);
        m_ContentValue.text = " " + m_ShopItemData.contentValue.ToString();


        FormatBuyButton();

        // use the oversize style if discounted
        if (IsDiscounted(m_ShopItemData))
        {
            m_SizeContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(wideSize, m_SizeContainer.GetComponent<RectTransform>().sizeDelta.y);
        }
        else
        {
            m_SizeContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(normalSize, m_SizeContainer.GetComponent<RectTransform>().sizeDelta.y);
        }

    }
    void DigitCount_Cost(int digitCount)
    {
        // Tính toán khoảng cách di chuyển dựa trên số lượng chữ số
        float moveDistance = -5f * digitCount;

        MoveObject_Cost(moveDistance);
    }
    void DigitCount_Content(int digitCount)
    {
        float moveDistance = -5f * digitCount;

        MoveObject_Content(moveDistance);
    }

    void MoveObject_Cost(float movement)
    {
        // Lấy RectTransform của m_CostIconGroup
        m_CostIconGroup.GetComponent<RectTransform>().localPosition = new Vector2(movement, m_CostIconGroup.GetComponent<RectTransform>().localPosition.y);
    }
    void MoveObject_Content(float movement)
    {
        m_ContentIconGroup.GetComponent<RectTransform>().localPosition = new Vector2(movement, m_ContentIconGroup.GetComponent<RectTransform>().localPosition.y);

    }
    // format the cost and cost currency
    void FormatBuyButton()
    {
        string currencyPrefix = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "$" : string.Empty;
        string decimalPlaces = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "0.00" : "0";

        if (m_ShopItemData.cost > 0.00001f)
        {
            if (m_ShopItemData.CostInCurrencyType == CurrencyType.USD)
            {
                m_CostUSD.text = currencyPrefix + m_ShopItemData.cost.ToString(decimalPlaces);
                m_CostUSDGroup.SetActive(m_ShopItemData.CostInCurrencyType == CurrencyType.USD);

            }

            m_CostPrice.text = currencyPrefix + m_ShopItemData.cost.ToString(decimalPlaces);


            int digitCount_Cost = m_ShopItemData.cost.ToString().Length;
            int digitCount_Content = m_ShopItemData.contentValue.ToString().Length;


            if (digitCount_Cost > 2)
            {
                DigitCount_Cost(digitCount_Cost);
            }
            if(digitCount_Content > 1)
            {
                DigitCount_Content(digitCount_Content);
            }

            Sprite currencySprite = m_GameIconsData.GetCurrencyIcon(m_ShopItemData.CostInCurrencyType);

            m_CostIcon.sprite = currencySprite;
            //m_DiscountIcon.sprite = currencySprite;

            m_CostGroup.SetActive(m_ShopItemData.CostInCurrencyType != CurrencyType.USD);
            //m_DiscountGroup.SetActive(m_ShopItemData.CostInCurrencyType != CurrencyType.USD);

        }
        // if the cost is 0, mark the ShopItem as free and hide the cost currency
        else
        {
            m_CostGroup.SetActive(false);
            //m_DiscountGroup.SetActive(false);
            m_CostPrice.text = "Free";
        }

        // disable/enabled, depending whether the item is discounted
        //m_DiscountBadge.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountLabel.text = m_ShopItemData.discount + "%";
        //m_DiscountSlash.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountGroup.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountCost.text = currencyPrefix + (((100 - m_ShopItemData.discount) / 100f) * m_ShopItemData.cost).ToString(decimalPlaces);
    }

    bool IsDiscounted(ShopItemSO shopItem)
    {
        return (shopItem.discount > 0);
    }

    //bool HasBanner(ShopItemSO shopItem)
    //{
    //    return !string.IsNullOrEmpty(shopItem.promoBannerText);
    //}

    public void RegisterCallbacks()
    {
        if (m_BuyButton == null)
            return;
        // store the cost/contents data in each button for later use
        //m_BuyButton.userData = m_ShopItemData;
        m_BuyButton.onClick.AddListener(BuyAction);
    }
    void BuyAction()
    {
        // Notify the ShopController (passes ShopItem data + UI Toolkit screen position)
        Vector2 screenPos = m_BuyButton.transform.position;

        ShopItemClicked?.Invoke(m_ShopItemData, screenPos);

        // Play a button click sound
        //AudioManager.PlayDefaultButtonSound();
    }
}



