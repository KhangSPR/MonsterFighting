using System;
using UnityEngine;
using UnityEngine.UI;
using UIGameDataManager;
using TMPro;
using System.Threading;


// represents one item in the shop
public class ShopItemComponent : MonoBehaviour
{
    // notify the ShopScreenController to buy product (passes the ShopItem data + UI element screen position)
    public static event Action<ShopItemSO, ShopItemComponent, Vector2> ShopItemClicked;

    // ScriptableObject pairing icons with currency/shop item types
    GameIconsSO m_GameIconsData;
    ShopItemSO m_ShopItemData;

    // visual elements
    //Text
    [Header("Text")]
    [SerializeField] TMP_Text m_Description;
    [SerializeField] TMP_Text m_ContentValue;
    [SerializeField] TMP_Text m_CostPrice;
    [SerializeField] TMP_Text m_CostUSD;
    [SerializeField] TMP_Text m_QuantityContent;
    //Text m_DiscountLabel;
    //Text m_DiscountCost;

    //Image
    [Header("Image")]
    [SerializeField] Image m_ProductImage;
    [SerializeField] Image m_CostIcon;
    [SerializeField] Image m_DiscountIcon;
    [SerializeField] Image m_ContentIcon; //Icon Count

    //GameObject
    [Header("GameObject")]
    [SerializeField] GameObject m_SizeContainer;
    //GameObject m_DiscountBadge;
    //GameObject m_DiscountSlash;
    //GameObject m_DiscountGroup;
    [SerializeField] GameObject m_ContentGroup;
    //[SerializeField] GameObject m_ContentIconGroup;
    [SerializeField] GameObject m_CostIconGroup;
    [SerializeField] GameObject m_CostGroup;
    [SerializeField] GameObject m_ProductItem;
    [SerializeField] GameObject m_CostUSDGroup;
    [SerializeField] GameObject m_Watch;

    [Header("Button")]
    [SerializeField] Button m_BuyButton;
    [SerializeField] CanvasGroup CanvasGroup;
    public void UpdateBuyButton(bool active)
    {
        if (active)
        {
            CanvasGroup.alpha = 1;
        }
        else
        {
            CanvasGroup.alpha = 120 / 255f;
        }
    }

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
        //m_ContentIcon.sprite = m_GameIconsData.GetCurrencyIcon(m_ShopItemData.CostInCurrencyType);
        m_ContentValue.text = "Quantity: " + m_ShopItemData.contentValue;
        m_QuantityContent.text = "x" + m_ShopItemData.quantityContent;
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
    void MoveObject_Cost(float movement)
    {
        // Lấy RectTransform của m_CostIconGroup
        m_CostIconGroup.GetComponent<RectTransform>().localPosition = new Vector2(movement, m_CostIconGroup.GetComponent<RectTransform>().localPosition.y);
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
            if (digitCount_Content > 1)
            {
                //DigitCount_Content(digitCount_Content);
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
            //m_CostGroup.SetActive(false);
            ////m_DiscountGroup.SetActive(false);
            //m_CostPrice.text = "Free";
            if (m_ShopItemData.contentType == ShopItemType.Watch)
            {
                m_CostIconGroup.SetActive(false);

                m_Watch.SetActive(true);

                Debug.Log("Watch True");
            }
        }
        // disable/enabled, depending whether the item is discounted
        //m_DiscountBadge.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountLabel.text = m_ShopItemData.discount + "%";
        //m_DiscountSlash.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountGroup.SetActive(IsDiscounted(m_ShopItemData));
        //m_DiscountCost.text = currencyPrefix + (((100 - m_ShopItemData.discount) / 100f) * m_ShopItemData.cost).ToString(decimalPlaces);
    }
    public void SetContentValue(int value)
    {
        m_ContentValue.text = "Quantity: " + value;
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

        ShopItemClicked?.Invoke(m_ShopItemData, this, screenPos); //Has Buy

        // Play a button click sound
        //AudioManager.PlayDefaultButtonSound();
    }
    private WatchTimerHandler timerHandler;
    [SerializeField] private bool isWatch = false;
    public bool IsWatch => isWatch;
    private bool hasWatchItems = false;
    private void Update()
    {
        //Watch
        if (isWatch)
        {
            timerHandler.UpdateTimer();
            hasWatchItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);
            ShowTimerUIWatch(); //Update Timer
            HandleItemWatch(); //Handler Item
            return;
        }
        if (m_ShopItemData?.contentValue > 0)
        {
            return;
        }
        DateTime timerNow = DateTime.Now;
        TimeSpan remainingTime = TimeSpan.FromHours(24) - timerNow.TimeOfDay;
        ShowTimerUI(remainingTime);
    }
    private void ShowTimerUIWatch()
    {
        m_ContentValue.text = hasWatchItems ? "" : timerHandler.GetTimerWatch();
    }
    private void OnApplicationQuit() //Repair Android
    {
        if (m_ShopItemData.contentType == ShopItemType.Watch)
        {
            PlayerPrefs.SetInt("WatchItem", isWatch ? 1 : 0);
            PlayerPrefs.Save();  
        }

    }
    private void HandleItemWatch()
    {
        if (hasWatchItems)
        {
            UpdateBuyButton(true);
            m_ContentValue.text = "Quantity: " + m_ShopItemData.contentValue.ToString();
            isWatch = false;
        }
    }
    public void CalculateWatchStatus()
    {
        timerHandler = new WatchTimerHandler();
        int IsCalculate = PlayerPrefs.GetInt("WatchItem");
        if (IsCalculate == 0) return;

        if (timerHandler.TimerDelta.TotalSeconds < 0)
        {
            isWatch = false;
        }
        else
        {
            UpdateBuyButton(false);
            isWatch = true;
        }
    }
    public void StartWatch()
    {
        if (m_ShopItemData.contentValue <= 0)
            isWatch = false; //Hasn't enough ContentValue
        else
            isWatch = true; //Has enough ContentValue
        timerHandler.SetWatch(m_ShopItemData.RestoreInterval);
        timerHandler.SaveTimer();
        UpdateBuyButton(false);
    }
    private void ShowTimerUI(TimeSpan remainingTime)
    {
        // Định dạng chuỗi để hiển thị thời gian còn lại
        m_ContentValue.text = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
    }
}



