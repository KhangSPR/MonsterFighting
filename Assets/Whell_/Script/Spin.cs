using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    // Number of gifts on the spin wheel
    public int numberOfGift = 9;

    // Rotation settings
    public float timeRotate = 3f; // Total rotation time
    public float numberCircleRotate = 3f; // Number of full circles before stopping

    private const float CIRCLE = 360.0f; // Degrees in a full circle
    private float angleOfOneGift; // Angle of one gift section

    public Transform parent; // Parent Transform containing gift objects
    public AnimationCurve curve; // Animation curve for rotation speed

    // UI elements
    [SerializeField] private GameObject itemGiftPrefab;
    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private int rewardResetHours;
    [SerializeField] private int rewardResetMinutes;
    [SerializeField] private Button btnSpinWatch;
    [SerializeField] private Button btnSpinX1;
    [SerializeField] private Button btnSpinX10;

    // Spin data
    [SerializeField] private SpinLevelItemSO[] spinLevelItem; // Configuration for spin levels
    [SerializeField] private PanelItemReward panelItemReward;

    private SpinTimerHandler timerHandler;
    private bool isRotating = false;
    private int[] itemRewardSpins;
    private bool hasRewardItems = false;

    public static Action<int> OnSpinData;

    private void OnEnable()
    {
        GameDataManager.OnSpin += RotateSpin;
    }

    private void OnDisable()
    {
        GameDataManager.OnSpin -= RotateSpin;
    }

    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        UpdateTimerWatch();

        if (Input.GetKeyDown(KeyCode.S))
        {
            timerHandler.ResetTimer();
        }
    }
    private void UpdateTimerWatch()
    {
        timerHandler.UpdateTimer();
        hasRewardItems = (timerHandler.TimerDelta.TotalMilliseconds < 0);
        UpdateTimerUI();
        btnSpinWatch.enabled = hasRewardItems;
    }
    /// <summary>
    /// Initializes spin settings and UI.
    /// </summary>
    private void Initialize()
    {
        // Calculate the angle of one gift section
        angleOfOneGift = CIRCLE / numberOfGift;

        // Set up timer handler
        timerHandler = new SpinTimerHandler();

        // Set up gift positions and data
        CreateGiftData();
        SetGiftPositions();
        UpdateSpinButtonState();

        // Assign button listeners
        btnSpinWatch.onClick.AddListener(RotateWithWatch);
        btnSpinX1.onClick.AddListener(() => StartSpin(1));
        btnSpinX10.onClick.AddListener(() => StartSpin(10));
    }

    /// <summary>
    /// Updates the state of the spin button based on timer.
    /// </summary>
    private void UpdateSpinButtonState()
    {
        hasRewardItems = timerHandler.TimerDelta.TotalMilliseconds < 0;
        btnSpinWatch.enabled = hasRewardItems;
        UpdateTimerUI();
    }

    /// <summary>
    /// Displays the timer or "Watch" when eligible.
    /// </summary>
    private void UpdateTimerUI()
    {
        textTimer.text = hasRewardItems ? "Watch" : timerHandler.GetTimerString();
    }

    /// <summary>
    /// Handles spinning with a specific count.
    /// </summary>
    /// <param name="count">Number of spins</param>
    private void RotateSpin(int count)
    {
        if (isRotating) return;

        itemRewardSpins = new int[count];
        for (int i = 0; i < count; i++)
        {
            itemRewardSpins[i] = GenerateRandomRewardIndex();
        }

        StartCoroutine(RotateWheel(itemRewardSpins[0]));
        isRotating = true;
    }

    /// <summary>
    /// Starts a single spin.
    /// </summary>
    private void RotateWithWatch()
    {
        timerHandler.SetNextDay000(rewardResetHours, rewardResetMinutes);
        timerHandler.SaveTimer();
        Rewarded.Instance.ShowRewardedAd();
        StartSpin(1);
    }

    /// <summary>
    /// Begins the spin process for a specified count.
    /// </summary>
    /// <param name="count">Number of spins</param>
    private void StartSpin(int count)
    {
        if (isRotating) return;
        OnSpinData?.Invoke(count);
    }

    /// <summary>
    /// Generates a random reward index based on rarity probability.
    /// </summary>
    /// <returns>Index of the reward</returns>
    private int GenerateRandomRewardIndex()
    {
        int randomValue = UnityEngine.Random.Range(0, 100);

        if (randomValue < 1) // 1%
        {
            return 0;
        }
        else if (randomValue < 75) // 74%
        {
            return UnityEngine.Random.Range(1, 4);
        }
        else // 25%
        {
            return UnityEngine.Random.Range(5, 7);
        }
    }

    /// <summary>
    /// Rotates the wheel to the specified reward index.
    /// </summary>
    /// <param name="indexGiftRandom">Reward index</param>
    private IEnumerator RotateWheel(int indexGiftRandom)
    {
        float startAngle = transform.eulerAngles.z;
        float currentTime = 0;

        float totalAngle = (numberCircleRotate * CIRCLE) + angleOfOneGift * indexGiftRandom - startAngle;

        while (currentTime < timeRotate)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float currentAngle = totalAngle * curve.Evaluate(currentTime / timeRotate);
            transform.eulerAngles = new Vector3(0, 0, startAngle + currentAngle);
        }

        transform.eulerAngles = new Vector3(0, 0, startAngle + totalAngle);
        DisplayRewards();
        isRotating = false;
    }

    /// <summary>
    /// Displays the rewards on the UI.
    /// </summary>
    private void DisplayRewards()
    {
        if (panelItemReward == null) return;

        panelItemReward.ShowRewards(itemRewardSpins, spinLevelItem);
        itemRewardSpins = null;
    }

    /// <summary>
    /// Creates and initializes gift data.
    /// </summary>
    private void CreateGiftData()
    {
        foreach (Transform obj in parent)
        {
            Destroy(obj.gameObject);
        }

        foreach (SpinLevelItemSO spinLevel in spinLevelItem)
        {
            foreach (UIGameDataMap.Resources resources in spinLevel.ResourceItems)
            {
                CreateGift(resources.item, resources.Count);
            }
            foreach (InventoryItem inventoryItem in spinLevel.InventoryItems)
            {
                CreateGift(inventoryItem.itemObject, inventoryItem.count);
            }
        }
    }

    /// <summary>
    /// Creates a single gift item on the spin wheel.
    /// </summary>
    private void CreateGift(object item, int count)
    {
        GameObject newGift = Instantiate(itemGiftPrefab, parent);
        ItemTooltipGift tooltip = newGift.GetComponent<ItemTooltipGift>();

        if (item is ItemObject itemObject)
        {
            tooltip.SetUIITemGift(itemObject, null, count);
        }
        else if (item is ItemReward itemReward)
        {
            tooltip.SetUIITemGift(null, itemReward, count);
        }
        else
        {
            Debug.LogError("Invalid item type passed to CreateGift");
        }
    }
    /// <summary>
    /// Positions gifts around the wheel.
    /// </summary>
    private void SetGiftPositions()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            float angle = -CIRCLE / numberOfGift * i;
            parent.GetChild(i).eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
