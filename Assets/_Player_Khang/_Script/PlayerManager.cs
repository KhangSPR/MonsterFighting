using System;
using System.Collections.Generic;
using TMPro;
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
                currentXP = 0;
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
                UpdateXPUI();
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
    }

    private void Start()
    {
        SetCurrentCardPlayer();
        ActivateDialog();
        maxXP = LoadXPOfLevel();
        SetUIStatsPlayer();
        SetStatsPlayer(cardCurrentPlayer.CharacterStats);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(50);
            Debug.Log("XP Added");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
            IsDiaLog = false;
        }
    }
    private void ResetPlayer()
    {
        lvPlayer = 1;
        currentXP = 0;
    }
    private void AddXP(uint xp)
    {
        CurrentXP += xp;
    }

    private void SetCurrentCardPlayer()
    {
        cardCurrentPlayer = (gendersType == GendersType.Male) ? CardMale : CardFeMale;
    }

    private void ActivateDialog()
    {
        if (!isDiaLog)
        {
            Instantiate(m_DialogPrefab, UIManager.Instance.HouseUI);
            isDiaLog = true;
        }
    }

    private void SetUIStatsPlayer()
    {
        avatar.sprite = cardCurrentPlayer.AvatarPlayer;
        UpdateXPUI();
    }

    private void UpdateXPUI()
    {
        lvText.text = "LV" + lvPlayer;
        slider.SetMaxSlider(maxXP);
        slider.SetCurrentSlider(currentXP);
        xpText.text = $"{currentXP}/{maxXP}";
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
        int startLine = gendersType == GendersType.Male ? 1 : 11;
        int endLine = gendersType == GendersType.Male ? 11 : lines.Length;

        for (int i = startLine; i < endLine; i++)
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 2 && columns[0].Trim() == $"lv{lvPlayer + 1}" && uint.TryParse(columns[1].Trim(), out var xp))
            {
                return xp;
            }
        }
        return 0;
    }

    private void LoadStatsPlayer()
    {
        string[] lines = csvFile.text.Split('\n');
        int startLine = gendersType == GendersType.Male ? 1 : 11;
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
