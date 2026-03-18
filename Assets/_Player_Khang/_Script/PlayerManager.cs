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
    [Header("UI CardFrame Update Rarity B")]
    [SerializeField] private Sprite m_FrameAvatarB;
    [SerializeField] private Sprite m_FrameNameB;
    [Header("UI CardFrame Update Rarity SS")]
    [SerializeField] private Sprite m_FrameAvatarSS;
    [SerializeField] private Sprite m_FrameNameSS;


    public static Action OnQuestUpdate;
    public static Action OnQuestUIDisplayUpdate;
    public static Action OnQuestPVP;
    public static Action OnCardPlayerAwake;

    [SerializeField] private uint lvPlayer;
    //public uint LvPlayer
    //{
    //    set
    //    {
    //        if (lvPlayer != value)
    //        {
    //            lvPlayer = value;
    //            currentXP = currentXP - maxXP; //retain energy
    //            maxXP = LoadXPOfLevel();
    //            UpdateXPUI();
    //            SaveData();

    //            // Gọi OnSkillLvBefore trước khi cập nhật thông tin
    //            OnSkillLvBefore?.Invoke(cardCurrentPlayer);


    //            SetStatsPlayer(cardCurrentPlayer.CharacterStats);


    //            // Gọi OnSkillLvAfter sau khi stats được thiết lập xong
    //            OnSkillLvAfter?.Invoke(cardCurrentPlayer);


    //            //Quest Update
    //            OnQuestUpdate?.Invoke();
    //            //Quest UI Display
    //            OnQuestUIDisplayUpdate?.Invoke();

    //            //Quest UI PVP
    //            if(lvPlayer == 5)
    //            {
    //                OnQuestPVP?.Invoke();
    //            }
    //        }
    //    }
    //    get => lvPlayer;
    //}

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsDiaLog = false;
        }
    }
    public void SetOnDisableDialog()
    {
        IsDiaLog = true;
    }
    private void ActivateDialog()
    {
        Instantiate(m_DialogPrefab, UIManager.Instance.HouseUI);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("IsDiaLog", isDiaLog ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        isDiaLog = PlayerPrefs.GetInt("IsDiaLog", 0) == 1;
    }
}
