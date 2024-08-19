using System;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

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
    CardBtn cardBtn;

    [Space]
    [Space]
    [Space]
    [Header("Game Play")]
    [SerializeField] private bool isGamePaused = false;
    public Transform gameFinish;
    public Transform fade;

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

    //Envent
    public bool OnFinish;

    [Space]
    [Space]
    [Space]
    [Header("Guild Play")]
    public GuildSOManager GuildSOManager;


    //UI HP
    [Space]
    [Space]
    [Space]
    [Header("HP Play")]
    public Slider slider_maxhp;
    [Min(1)] public int max_hp;
    [ReadOnlyInspector, SerializeField] private int current_hp;


    //Envent
    public static event Action CastleSetHpMax;
    public static event Action UpdateResources;

    protected override void OnEnable()
    {
        base.OnEnable();
        PortalSpawnManager.AllPortalsSpawned += GameWin;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PortalSpawnManager.AllPortalsSpawned -= GameWin;
    }
    protected override void Start()
    {
        base.Start();
        if (GuildSOManager == null) return;

        GuildSOManager.GuildAbilitySO.ApplyDefaultStats(gameObject);

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
    #region Guild Defaut

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
        UpdateCurrentHpUI(); GameLoss(current_hp);
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
        }
        else if (button is CardBtn && costManager.Currency >= button.Price)
        {
            HandleActivation(button);

            cardRefresh = CardPickup;
        }
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

        this.SubtractCurrency(clickBtn.Price);

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
        if (button is CardBtn)
        {
            Hover.Instance.Activate(button.Sprite);

            this.clickBtn = button as CardBtn;

            cardBtn = (CardBtn)clickBtn;

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

    private void PauseGame()
    {
        Time.timeScale = 0f; // Dừng thời gian trong trò chơi

    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian trong trò chơi       
    }
    #endregion


    #region Game ReSult
    //Game Result--------------------------------------------------------------------------------
    private void GameWin()
    {
        var dropItemHolder = EnemyDropSpawner.Instance.holder;

        gameDataManager = FindAnyObjectByType<GameDataManager>();
        foreach (Transform item in dropItemHolder)
        {
            if (item.gameObject.activeSelf)
            {
                if (!item.GetComponent<ItemPickupCtrl>().IsAnimationProcess)
                {
                    item.GetComponent<ItemPickupCtrl>().ItemPickupAnimation();
                    //uint itemValue = 1; // biến này sau này sẽ dựa theo scale của đá mà set value
                    //costManager.StoneEnemyCurrency += 1;
                }
            }
        }
        Map_UI_Manager.UIWin.gameObject.SetActive(true);

        Debug.Log(gameDataManager);
        gameDataManager.GameData.enemyStone += (uint)costManager.StoneEnemyCurrency;

        UpdateResources?.Invoke();

        //fade.gameObject.SetActive(true);
    }
    private void GameLoss(int current_hp)
    {
        Debug.Log("Current Hp = " + current_hp);
        if (current_hp <= 0)
        {
            Debug.Log("You Lose !!");
            RectTransform UILost = Map_UI_Manager.GetComponent<Map_Ui_Manager>().UILose;
            Map_UI_Manager.GetComponent<Map_Ui_Manager>().OpenRectransform(UILost);

            var dropItemHolder = EnemyDropSpawner.Instance.holder;

            foreach (Transform item in dropItemHolder)
            {
                Destroy(item.gameObject);
            }
            Map_UI_Manager.GetComponent<Map_Ui_Manager>().UILose.gameObject.SetActive(true);

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
