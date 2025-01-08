using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CostManager : SaiMonoBehaviour
{
    const float k_LerpTime = 0.5f;
    const float autoIncreaseDuration = 30f; // Thời gian tự động tăng vàng trong giây

    private static CostManager instance;
    public static CostManager Instance => instance;

    [SerializeField] private TMP_Text currencyTxt;
    [SerializeField] private TMP_Text stoneEnemyCurrencyTxt;
    [SerializeField] private TMP_Text stoneBossCurrencyTxt;

    [SerializeField] private int currency;
    [SerializeField] private int stoneEnemyCurrency;
    [SerializeField] private int stoneBossCurrency;

    [SerializeField] private int autoIncreaseAmount = 0; //Số vàng tăng mỗi giây
    private Coroutine autoIncreaseCoroutine;

    [SerializeField] private List<InventoryItem> listInventoryItem;
    public List<InventoryItem> ListInventoryItem => listInventoryItem;
    public void SetAutoIncreaseAmount(int amount)
    {
        autoIncreaseAmount = amount;
    }
    public int Currency
    {
        get { return currency; }
        set
        {
            StartCoroutine(LerpRoutine(currencyTxt, (uint)currency, (uint)value, k_LerpTime));
            currencyTxt.text = value.ToString();
            currency = value;

        }
    }

    public int StoneEnemyCurrency
    {
        get { return stoneEnemyCurrency; }
        set
        {

            StartCoroutine(LerpRoutine(stoneEnemyCurrencyTxt, (uint)stoneEnemyCurrency, (uint)value, k_LerpTime));
            stoneEnemyCurrencyTxt.text = value.ToString();
            stoneEnemyCurrency = value;

        }
    }

    public int StoneBossCurrency
    {
        get { return stoneBossCurrency; }
        set
        {

            //StartCoroutine(LerpRoutine(stoneBossCurrencyTxt, (uint)stoneBossCurrency, (uint)value, k_LerpTime));
            stoneBossCurrencyTxt.text = value.ToString();
            stoneBossCurrency = value;

        }
    }
    protected override void Awake()
    {
        base.Awake();
        //if (GameManager.instance != null) Debug.LogError("Onlly 1 GameManager Warning");
        CostManager.instance = this;
    }
    protected override void Start()
    {
        base.Start();
        //StartAutoIncreaseCurrency();
    }

    // Coroutine để tự động tăng vàng liên tục
    private IEnumerator AutoIncreaseCurrencyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoIncreaseDuration);
            Currency += autoIncreaseAmount;
        }
    }

    // Phương thức công khai để bắt đầu quá trình tự động tăng vàng
    public void StartAutoIncreaseCurrency()
    {
        if (autoIncreaseCoroutine == null)
        {
            autoIncreaseCoroutine = StartCoroutine(AutoIncreaseCurrencyRoutine());
        }
    }

    // animated Label counter
    IEnumerator LerpRoutine(TMP_Text label, uint startValue, uint endValue, float duration)
    {
        float lerpValue = (float)startValue;
        float t = 0f;
        label.text = string.Empty;

        while (Mathf.Abs((float)endValue - lerpValue) > 0.05f)
        {
            t += Time.deltaTime / k_LerpTime;

            lerpValue = Mathf.Lerp(startValue, endValue, t);
            label.text = lerpValue.ToString("0");
            yield return null;
        }
        label.text = endValue.ToString();
    }
    //Receiver Item
    public void ReceiverItemDisplay(ItemDropType itemDropType, int count)
    {
        switch (itemDropType)
        {
            case ItemDropType.MagicalCrystal:
                StoneEnemyCurrency += count;
                break;

            case ItemDropType.Crystalline:
                stoneBossCurrency += count;
                break;
            default:
                Debug.Log("Item Error");
                break;
        }
    }
    public void ReceiverItemInventory(InventoryItem inventoryItem)
    {
        if (listInventoryItem == null)
        {
            listInventoryItem = new List<InventoryItem>();
        }


        if (CheckForDuplicateType(inventoryItem))
            return;

        listInventoryItem.Add(inventoryItem);
    }    
    bool CheckForDuplicateType(InventoryItem inventoryItem)
    {
        foreach (InventoryItem item in listInventoryItem)
        {
            if(inventoryItem.itemObject.IdDatabase == item.itemObject.IdDatabase)
            {
                item.count += inventoryItem.count;

                return true;
            }
        }
        return false;
    }
}
