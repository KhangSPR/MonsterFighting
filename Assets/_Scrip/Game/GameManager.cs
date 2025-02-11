using System;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UIGameDataMap;
using Unity.VisualScripting;
using UnityEngine;
using static QuestInfoSO;

[Flags]
public enum GameStateFlags
{
    None = 0,
    ClickHoverMove = 1 << 0,
    ClickHoverRemove = 1 << 1,
    ClickTile = 1 << 2,
    ClickInventory = 1 << 3,
    StarCondition = 1 << 4,
    ClickSetting = 1 << 5,
}
public class GameManager : SaiMonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    [Space]
    [Space]
    [Space]
    [Header("Game Data")]
    GameDataManager gameDataManager;

    [Header("Card Play")]
    [SerializeField] BaseBtn clickBtn;
    public BaseBtn ClickBtn { get { return clickBtn; } set { clickBtn = value; } }
    [SerializeField]
    ImageRefresh cardRefresh;
    public ImageRefresh CardRefresh => cardRefresh;
    CardButton cardBtn;

    [Space]
    [Space]
    [Space]
    [Header("Game Play")]
    [SerializeField] private bool isGamePaused = false;
    public bool IsGamePaused => isGamePaused;
    [SerializeField] private bool isGameSpeeded = false;
    [SerializeField] private bool readyTimer = false;
    public bool ReadyTimer { get { return readyTimer; } set { readyTimer = value; } }

    [Space]
    [Space]
    [Space]
    [Header("Setting GamePlay")]
    [SerializeField] private SettingUI settingUI;

    #region Setting GamePlay
    public void OnOpenSetting()
    {
        this.settingUI.OpenSettingUI();
        this.TogglePauseGame();


        Hover.Instance.Hide();
        SetFlag(GameStateFlags.ClickSetting, true);

    }
    public void CloseSetting()
    {
        this.settingUI.CloseSettingUI();
        this.TogglePauseGame();

        Hover.Instance.Hide();
        SetFlag(GameStateFlags.ClickSetting, false);

    }
    public void SaveSetting()
    {
        SettingManager.Instance.SaveSetting();
    }
    private void OnDestroy()
    {
        SaveSetting();
    }
    #endregion

    [Space]
    [Space]
    [Space]
    [Header("Cost Play")]
    [SerializeField] private CostManager costManager;
    public CostManager CostManager => costManager;
    [Space]
    [Space]
    [Space]
    [Header("UI Play")]
    [SerializeField] public Map_Ui_Manager Map_UI_Manager;

    [Space]
    [Space]
    [Space]


    //UI HP
    [Space]
    [Space]
    [Space]
    [Header("HP Play")]
    public UnityEngine.UI.Slider slider_maxhp;
    [Min(1)] public int max_hp;
    [ReadOnlyInspector, SerializeField] private int current_hp;

    [Space]
    [Space]
    [Space]
    [Header("StarCondition")]
    public UnityEngine.UI.Slider slider_star;
    [SerializeField] RectTransform fxHolder;
    [SerializeField] UILevelStarConditionCtrl levelStarConditionCtrl;
    private bool FlagHPStar = true;
    private int star;
    public int Star => star;

    //Envent
    public static event Action CastleSetHpMax;
    public static event Action UpdateResources;

    //LevelSettings Game Play
    private LevelSettings currentLevelSettings;
    public LevelSettings CurrentLevelSettings => currentLevelSettings;
    #region Click Hover UI
    [Space]
    [Space]
    [Header("Click Hover UI")]
    private GameStateFlags gameStateFlags = GameStateFlags.None;

    //Action Quest
    public Dictionary<string, int> _temporaryUpdates = new Dictionary<string, int>();

    public bool AreFlagsSet(GameStateFlags flagsToCheck)
    {
        return (gameStateFlags & flagsToCheck) != 0;
    }
    private void ResetAllFlagsWithLogic()
    {
        foreach (GameStateFlags flag in Enum.GetValues(typeof(GameStateFlags)))
        {
            if (flag != GameStateFlags.None)
            {
                SetFlag(flag, false);
            }
        }
    }

    public void SetFlag(GameStateFlags flag, bool value)
    {
        if (value)
        {
            gameStateFlags |= flag;
        }
        else
        {
            gameStateFlags &= ~flag;
        }
    }
    [SerializeField] private TMP_Text removeTxt;
    int currentRemove = 2;

    private GameObject objSwapMove;
    public GameObject ObjSwapMove { get { return objSwapMove; } set { objSwapMove = value; } }


    public int CurrentRemove
    {
        get { return currentRemove; }
        set
        {
            removeTxt.text = "X" + value.ToString();
            currentRemove = value;

        }
    }
    [SerializeField] private TMP_Text moveTxt;

    int currentMove = 2;
    public int CurrentMove
    {
        get { return currentMove; }
        set
        {
            moveTxt.text = "X" + value.ToString();
            currentMove = value;

        }
    }
    #endregion 
    protected override void OnEnable()
    {
        base.OnEnable();
        UIChoosingMapLoader.LevelSettingsChanged += OnSetLevelSettings;
        LevelSettings.HpPercentage += OnUpdateCurrentHpPercentage;

        WaveSpawnManager.AllPortalsSpawned -= GameWin;
        WaveSpawnManager.AllPortalsSpawned += GameWin;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        UIChoosingMapLoader.LevelSettingsChanged -= OnSetLevelSettings;

        LevelSettings.HpPercentage -= OnUpdateCurrentHpPercentage;


    }
    protected override void Start()
    {
        base.Start();

        GuildManager.Instance.GuildAbilitySO.ApplyDefaultStats(gameObject);

        SetHpInGame();


    }
    protected override void Awake()
    {
        base.Awake();
        //if (GameManager.instance != null) Debug.LogError("Onlly 1 GameManager Warning");
        GameManager.instance = this;
    }
    private void OnApplicationQuit()
    {
        // Xử lý tại đây khi game bị tắt
        HandleEscape();
    }
    #region Castle Dead -- Display
    //Castle Is Dead
    [Space]
    [Space]
    [Header("Castle Dead Event")]
    [SerializeField]
    private bool isCastleDead = false;
    public bool IsCastleDead { get { return isCastleDead; } set { isCastleDead = value; } }
    [SerializeField] protected SpriteRenderer wallCity;
    public void SetLayerWallCity()
    {
        this.wallCity.sortingLayerName = "fx"; //Repair
    }
    private List<Transform> allModleSpawn = new List<Transform>();
    public List<Transform> AllModleSpawn { get { return allModleSpawn; } set { allModleSpawn = value; } }
    private Transform currentModleCall;
    public Transform CurrentModleCall { get { return currentModleCall; } set { currentModleCall = value; } }
    public void SetAnimatorEnabled()
    {
        foreach (Transform t in allModleSpawn)
        {
            if (!t.gameObject.activeSelf || t == currentModleCall) continue;

            t.GetComponentInChildren<Animator>().enabled = false;
        }
    }
    #endregion

    #region Level Settings
    void OnSetLevelSettings(LevelSettings levelSettings)
    {
        currentLevelSettings = levelSettings;

        Debug.Log("Set 1 lan");
    }
    #endregion

    #region Guild Defaut and StarCondition HP

    //Guild ------------------------------------------------------------------------------
    // max hp value and UI
    public void SetHpInGame()
    {

        Debug.Log("Set Hp In Game");
        //slider_maxhp = transform.root.Find("---------UI-----------").Find("UI").Find("UITopCenter").GetChild(0).GetComponent<Slider>();
        current_hp = max_hp;
        slider_maxhp.maxValue = max_hp; UpdateCurrentHpUI();

        CastleSetHpMax?.Invoke();
    }
    public void UpdateCurrentHpUI()
    {
        slider_maxhp.value = current_hp;

    }
    public void Castle_On_Damage(int damage)
    {
        current_hp -= damage;

        //StarCondition
        currentLevelSettings.CheckStarCondition();

        UpdateCurrentHpUI();
    }

    void OnUpdateCurrentHpPercentage()
    {
        if (FlagHPStar)
        {
            slider_star.maxValue = 100;
            FlagHPStar = false;
        }

        float hpPercentage = GetCurrentHpPercentage();

        slider_star.value = hpPercentage;

        if (!fxHolder.gameObject.activeSelf)
        {
            fxHolder.gameObject.SetActive(true);
        }
        //FX Holder Slider
        fxHolder.localPosition = CalculateUIPosition(hpPercentage);

        Debug.Log("OnUpdateCurrentHpPercentage: " + slider_star.value);

        UpdateEmptyStarCondition(hpPercentage);

        //Effect Screen Damage
        EffectsScreenManager.Instance.ScreenDamageEffect(hpPercentage / 100f);
        EffectsScreenManager.Instance.object_ShakeTransfrom.ShakeAndRecover();

    }
    // Update Empty Star Condition
    private void UpdateEmptyStarCondition(float percent)
    {
        float[] percentStars = GetCurrentHpPercentageArrays();
        for (int i = 2; i >= 0; i--)
        {
            if (percentStars[i] > percent)
            {
                TargetStar targetStar = this.levelStarConditionCtrl.StarsShow[i].GetComponent<TargetStar>();

                if (targetStar != null)
                {
                    if (targetStar.activeArray)
                    {
                        targetStar.SetActiveImage(this.levelStarConditionCtrl.EmptySpriteRenderer);

                        targetStar.activeArray = false;
                    }
                }

                Debug.Log("UpdateEmptyStarCondition: " + percentStars[i] + "---" + percent);
            }
        }
    }


    private Vector3 CalculateUIPosition(float percentage)
    {
        float minX = -385 / 2f;
        float maxX = 385 / 2f;

        float positionXTarget = Mathf.Lerp(minX, maxX, percentage / 100f);

        Debug.Log("Position FX: " + positionXTarget);

        return new Vector3(positionXTarget, fxHolder.localPosition.y, fxHolder.localPosition.z);
    }

    private float GetCurrentHpPercentage()
    {
        if (max_hp <= 0)
        {
            return 0;
        }

        float currentHpPercentage = ((float)current_hp / max_hp) * 100f;

        return currentHpPercentage;
    }
    //HP Condition Percent
    public float[] GetCurrentHpPercentageArrays()
    {
        float[] Percentage = new float[3];

        for (int i = 0; i < Percentage.Length; i++)
        {
            if (currentLevelSettings.starConditions[i] is HpPercentageCondition hpCondition)
            {
                Percentage[i] = hpCondition.RequiredHpPercentage;
            }
        }
        return Percentage;
    }
    #endregion

    #region Card Button
    public void PickButton(BaseBtn button, ImageRefresh CardPickup)
    {
        if (button == clickBtn)
        {
            DePickButton(button);
            return; // If the button overlaps with clickBtn, do not perform other operations
        }
        else if (clickBtn != null) // If you are pressing another button
        {
            HandleEscape(); // HandleEscape for the current button
        }

        if (button is MachineBtn && costManager.Currency >= button.Price)
        {
            this.clickBtn = button as MachineBtn;

            Hover.Instance.Activate(button.Sprite);
            SetFlag(GameStateFlags.ClickTile, true);
        }
        else if (button is CardButton cardButton && costManager.Currency >= cardButton.CardCharacter.GetLevel(cardButton.CardCharacter.rarityCard))
        {
            HandleActivation(button);

            cardRefresh = CardPickup;
        }
        //Handle Effect Not Enough
    }

    public void DePickButton(BaseBtn button)
    {
        if (clickBtn == null) return;
        if (button == clickBtn)
        {
            HandleEscape();
            Debug.Log("DePickButton");
        }
    }

    public void BuyCard()
    {
        if(clickBtn is CardButton cardButton)
        {
            this.SubtractCurrency(cardButton.CardCharacter.GetLevel(cardButton.CardCharacter.rarityCard));
        }
        else
        {
            this.SubtractCurrency(clickBtn.Price);
        }
        this.HandleEscape();

    }

    //--
    void SubtractCurrency(int amount)
    {
        Debug.Log(amount);

        costManager.Currency -= amount;
    }
    public void HandleEscape()
    {
        SetFlag(GameStateFlags.ClickTile, false);
        Hover.Instance.Deactivate();
        this.clickBtn = null;
        if (cardBtn != null)
        {
            cardBtn.SelectButton.SetActive(false);
            cardBtn = null;
        }
    }
    public void HandleActivation(BaseBtn button)
    {
        if (button is CardButton)
        {
            SetFlag(GameStateFlags.ClickTile, true);
            Hover.Instance.Activate(button.Sprite);

            this.clickBtn = button as CardButton;

            cardBtn = (CardButton)clickBtn;

            cardBtn.SelectButton.SetActive(true);
        }
    }
    #endregion

    #region Pause Game
    // Pause Game-----------------------------------------------------------------------------------
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void ToggleSpeedGame()
    {
        isGameSpeeded = !isGameSpeeded;

        if (isGameSpeeded)
        {
            SpeedX2();
        }
        else
        {
            SpeedNormal();
        }
    }

    private void SpeedX2()
    {
        Time.timeScale = 2f;
    }

    private void SpeedNormal()
    {
        // Kiểm tra nếu đang tạm dừng thì không thay đổi timeScale
        if (!isGamePaused)
        {
            Time.timeScale = 1f;
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Dừng thời gian trong trò chơi
    }

    private void ResumeGame()
    {
        // Kiểm tra nếu isGameSpeeded đang bật thì giữ Time.timeScale là 2
        if (isGameSpeeded)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    #endregion

    #region Event Win Game
    public bool CheckBtnDifficult()
    {
        Difficult difficult = (Difficult)((int)MapManager.Instance.Difficult + 1);

        // Kiểm tra nếu difficult vượt quá giá trị enum cuối cùng
        if ((int)difficult >= Enum.GetValues(typeof(Difficult)).Length)
        {
            return false;
        }

        return true;
    }


    #endregion

    #region Game ReSult
    //Game Result--------------------------------------------------------------------------------
    private bool isDefeadted = false;
    public bool IsDefeadted => isDefeadted;
    private bool isGameWin = false;
    public bool IsGameWin => isGameWin;

    private void GameWin()
    {
        Debug.Log("GameWin ->>>>>>>");

        if (isGameWin) return;
        isGameWin = true;

        if (isGameSpeeded) Time.timeScale = 1;

        // Set Star Type
        SetStarConditionTypeMap();

        var dropItemHolder = EnemyDropSpawner.Instance?.holder; // Kiểm tra null
        if (dropItemHolder != null)
        {
            foreach (Transform item in dropItemHolder)
            {
                if (item != null && item.gameObject.activeSelf)
                {
                    var itemDropCtrl = item.GetComponent<ItemDropCtrl>();
                    if (itemDropCtrl != null && !itemDropCtrl.IsAnimationProcess)
                    {
                        itemDropCtrl.ItemPickupAnimation();
                    }
                }
            }
        }

        // UI Win
        if (Map_UI_Manager?.UIWin != null)
        {
            Map_UI_Manager.UIWin.gameObject.SetActive(true);
        }

        Debug.Log(gameDataManager);
        if (gameDataManager != null)
        {
            OnReceiverItemData();
        }

        UpdateResources?.Invoke();

        // Star Condition
        if (MapManager.Instance != null)
        {
            MapManager.Instance.UnLockNextMap();
            MapManager.Instance.SetStarDifficult(star);
            MapManager.Instance.SetReward();
        }

        // Consume Energy
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.ConsumeEnergy();
        }

        QuestComplete();
    }

    private void QuestComplete()
    {
        // Cập nhật từ biến tạm
        foreach (var update in _temporaryUpdates)
        {
            string enemyID = update.Key;
            int count = update.Value;

            foreach (QuestInfoSO quest in QuestManager.Instance.QuestInfos)
            {
                foreach (Objective objective in quest.objectives)
                {
                    if (objective.ID == enemyID && objective.type == Objective.Type.kill)
                    {
                        objective.UpdateObjectives(Objective.Type.kill, enemyID, count);
                    }
                    if(objective.ID == enemyID && objective.type == Objective.Type.collect)
                    {
                        objective.UpdateObjectives(Objective.Type.collect, enemyID, count);
                    }
                }
            }
        }
    }
    public void GameLoss()
    {
        if (isGameSpeeded) Time.timeScale = 1;

        isDefeadted = true;

        Debug.Log("You Lose !!");

        var dropItemHolder = EnemyDropSpawner.Instance.holder;

        foreach (Transform item in dropItemHolder)
        {
            Destroy(item.gameObject);
        }
        Map_UI_Manager.GetComponent<Map_Ui_Manager>().UILose.gameObject.SetActive(true);

    }
    private void SetStarConditionTypeMap()
    {
        if (currentLevelSettings.starConditions[0] is HpPercentageCondition hpCondition) //Repair
        {
            star = SetStartHPPercentageCondition();
        }
    }
    private int SetStartHPPercentageCondition()
    {
        int star = 3;

        float currentHpPercentage = ((float)current_hp / max_hp) * 100f;


        float[] PercentHP = GetCurrentHpPercentageArrays();

        for (int i = PercentHP.Length - 1; i >= 0; i--)
        {
            if (star == 0) return 0;

            if (PercentHP[i] > currentHpPercentage)
            {
                star--;
            }
        }
        return star;
    }
    private void OnReceiverItemData()
    {
        gameDataManager.GameData.StoneEnemy += (uint)costManager.StoneEnemyCurrency;
        gameDataManager.GameData.StoneBoss += (uint)costManager.StoneBossCurrency;

        foreach (var item in costManager.ListInventoryItem)
        {
            Item Item = new Item(item.itemObject);

            InventoryManager.Instance.inventory.AddItem(Item, item.count);
        }
    }
    #endregion

    #region Test
    //TEST_---------------------------------------------------------------
    //[ContextMenu("Castle_On_Damage")]
    //public void Castle_On_Damage()
    //{
    //    current_hp -= 1;
    //    UpdateCurrentHpUI();
    //    GameLoss(current_hp);
    //}
    #endregion
}
