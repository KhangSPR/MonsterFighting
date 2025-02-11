using System;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;

    [Header("UI Elements")]
    [SerializeField] private Image avatar;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text lvText;

    [Space]
    [SerializeField] private CardPlayer CardMale;
    [SerializeField] private CardPlayer CardFeMale;
    [Space]
    [Header("UI CardFrame Update Rarity B")]
    [SerializeField] private Sprite m_FrameAvatarB;
    [SerializeField] private Sprite m_FrameNameB;
    [Header("UI CardFrame Update Rarity SS")]
    [SerializeField] private Sprite m_FrameAvatarSS;
    [SerializeField] private Sprite m_FrameNameSS;
    private CardPlayer cardCurrentPlayer;
    public CardPlayer CardCurrentPlayer => cardCurrentPlayer;

    public static Action<CardPlayer> OnSkillLvBefore; //Before update(1)
    public static Action<CardPlayer> OnSkillLvAfter;  //After update(0)
    public static Action OnQuestUpdate;
    public static Action OnQuestUIDisplayUpdate;
    public static Action OnQuestPVP;

    [SerializeField] private uint lvPlayer;
    public uint LvPlayer
    {
        set
        {
            if (lvPlayer != value)
            {
                lvPlayer = value;
                currentXP = currentXP - maxXP; //retain energy
                maxXP = LoadXPOfLevel();
                UpdateXPUI();
                SaveData();

                // Gọi OnSkillLvBefore trước khi cập nhật thông tin
                OnSkillLvBefore?.Invoke(cardCurrentPlayer);


                SetStatsPlayer(cardCurrentPlayer.CharacterStats);


                // Gọi OnSkillLvAfter sau khi stats được thiết lập xong
                OnSkillLvAfter?.Invoke(cardCurrentPlayer);


                //Quest Update
                OnQuestUpdate?.Invoke();
                //Quest UI Display
                OnQuestUIDisplayUpdate?.Invoke();

                //Quest UI PVP
                if(lvPlayer == 5)
                {
                    OnQuestPVP?.Invoke();
                }
            }
        }
        get => lvPlayer;
    }


    private uint currentXP;
    public uint CurrentXP
    {
        set
        {
            if (currentXP != value)
            {
                currentXP = value;
                if (currentXP >= maxXP)
                {
                    LvPlayer++;
                }
                if (LvPlayer >= 10)
                {
                    UpdateLevelMax();
                }
                else
                {
                    UpdateXPUI();
                }
                SetCardRarity(cardCurrentPlayer);//Update Rarity
                CardUIPanelManager.Instance.UpdateRarityCardPlayer(cardCurrentPlayer);//Update UIPanel
                SaveData();
            }
        }
        get => currentXP;
    }

    private uint maxXP;
    public uint MaxXP => maxXP;

    [SerializeField] private bool isDiaLog;
    public bool IsDiaLog
    {
        set
        {
            if (isDiaLog != value)
            {
                isDiaLog = value;
                SaveData();
            }
        }
        get => isDiaLog;
    }

    [SerializeField] private GendersType gendersType;
    public GendersType GendersType
    {
        set
        {
            if (gendersType != value)
            {
                gendersType = value;
                SaveData();
            }
        }
        get => gendersType;
    }

    [SerializeField] private DialogUI m_DialogPrefab;
    [SerializeField] private TextAsset csvFile;

    private List<string> statStrings = new List<string>();
    private void OnApplicationFocus(bool hasFocus) //APly Android
    {
        if (!hasFocus) // Mất tiêu điểm
        {
            SaveData();
        }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only 1 PlayerManager instance allowed!");
            return;
        }
        instance = this;
        LoadData();
        LoadStatsPlayer();
        maxXP = LoadXPOfLevel();
        SetCurrentCardPlayer();
        SetCardRarity(cardCurrentPlayer);
        SetUIStatsPlayer();
        SetStatsPlayer(cardCurrentPlayer.CharacterStats);
    }

    private void Start()
    {
        if (!isDiaLog)
        {
            ActivateDialog();
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(200);
            Debug.Log("XP Added");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
            IsDiaLog = false;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ResetPlayer();
        }
    }
    public void SetOnDisableDialog()
    {
        IsDiaLog = true;
        ResetPlayer();
        SetCurrentCardPlayer();
        maxXP = LoadXPOfLevel();
        SetUIStatsPlayer();
        SetStatsPlayer(cardCurrentPlayer.CharacterStats);
    }
    private void ResetPlayer()
    {
        lvPlayer = 1;
        currentXP = 0;
    }
    public void AddXP(uint xp)
    {
        if(lvPlayer >= 10)
        {
            Debug.LogWarning("XP MAX PLAYER!");
            return;
        }
        CurrentXP += xp;
    }

    private void SetCurrentCardPlayer()
    {
        cardCurrentPlayer = (gendersType == GendersType.Male) ? CardMale : CardFeMale;
    }
    private void SetCardRarity(CardPlayer cardPlayer)
    {
        if(cardPlayer == null)
        {
            Debug.LogError("CardPlayer == null");
        }
        if(lvPlayer == 5)
        {
            cardPlayer.frame = m_FrameAvatarB;
            cardPlayer._frameCardName = m_FrameNameB;
            cardPlayer.rarityCard = RarityCard.B;
        }
        else if(lvPlayer == 10)
        {
            cardPlayer.frame = m_FrameAvatarSS;
            cardPlayer._frameCardName = m_FrameNameSS;
            cardPlayer.rarityCard = RarityCard.SS;
        }
        if(lvPlayer >=5)
        {
            cardPlayer.skill2.skillUnlock = true;
        }
    }
    private void ActivateDialog()
    {
        Instantiate(m_DialogPrefab, UIManager.Instance.HouseUI);
    }

    private void SetUIStatsPlayer()
    {
        avatar.sprite = cardCurrentPlayer.AvatarPlayer;
        if(LvPlayer < 10)
        {
            UpdateXPUI();
        }
        else
        {
            UpdateLevelMax();
        }
    }

    private void UpdateXPUI()
    {
        lvText.text = "LV" + lvPlayer;
        slider.SetMaxSlider(maxXP);
        slider.SetCurrentSlider(currentXP);
        xpText.text = $"{currentXP}/{maxXP}";
    }
    private void UpdateLevelMax()
    {
        lvText.text = "LV" + lvPlayer;
        xpText.text = $"Max";
        slider.SetMaxSlider(10);
        slider.SetCurrentSlider(10);
    }
    private void SetStatsPlayer(Stats stats)
    {
        if (lvPlayer < statStrings.Count)
        {
            string[] values = statStrings[(int)lvPlayer].Split(',');
            if (values.Length >= 6)
            {
                stats.Life = ParseInt(values[0]);
                stats.Attack = ParseInt(values[1]);
                stats.Deff = ParseInt(values[2]);
                stats.AttackSpeed = ParseFloat(values[3]);
                stats.Mana = ParseInt(values[4]);
                stats.RecoverMana = ParseInt(values[5]);
            }
        }
    }

    private int ParseInt(string value) => int.TryParse(value, out var result) ? result : 0;
    private float ParseFloat(string value) => float.TryParse(value, out var result) ? result : 0;

    private uint LoadXPOfLevel()
    {
        string[] lines = csvFile.text.Split('\n');
        int startLine = gendersType == GendersType.Male ? 1 : 12;
        int endLine = gendersType == GendersType.Male ? 11 : lines.Length;

        for (int i = startLine; i < endLine; i++)
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 2 && columns[0].Trim() == $"lv{lvPlayer + 1}" && uint.TryParse(columns[1].Trim(), out var xp))
            {
                Debug.Log($"XP lấy được tại dòng {i}: {xp}");
                return xp;
            }
        }
        return 0;
    }


    private void LoadStatsPlayer()
    {
        string[] lines = csvFile.text.Split('\n');
        int startLine = gendersType == GendersType.Male ? 0 : 12;
        int endLine = gendersType == GendersType.Male ? 11 : lines.Length;

        for (int i = startLine; i < endLine; i++)
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 8)
            {
                statStrings.Add($"{columns[2]},{columns[3]},{columns[4]},{columns[5]},{columns[6]},{columns[7]}");
            }
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("GendersType", (int)gendersType);
        PlayerPrefs.SetInt("LvPlayer", (int)lvPlayer);
        PlayerPrefs.SetInt("CurrentXP", (int)currentXP);
        PlayerPrefs.SetInt("IsDiaLog", isDiaLog ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        gendersType = (GendersType)PlayerPrefs.GetInt("GendersType", 0);
        lvPlayer = (uint)PlayerPrefs.GetInt("LvPlayer", 1);
        currentXP = (uint)PlayerPrefs.GetInt("CurrentXP", 0);
        isDiaLog = PlayerPrefs.GetInt("IsDiaLog", 0) == 1;
    }
}
