using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIGameDataManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static MainQuestSO;
using static QuestInfoSO;

public class QuestUIDisPlay : SaiMonoBehaviour
{
    [Header("UI Description Left")]
    [SerializeField] TMP_Text nameQuest;
    [SerializeField] TMP_Text descriptionQuest;
    [SerializeField] TMP_Text requireQuest;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] Transform holderReward;
    [SerializeField] GameObject prefabReward;
    [SerializeField] RectTransform DesTable;
    [SerializeField] Button btnQuest;

    [Space]
    [Header("UI Description Right")]
    [SerializeField] Transform holderMainQuest;
    [SerializeField] GameObject mainQuestPrefab;
    [SerializeField] GameObject lvQuestPrefab;
    [SerializeField] GameObject desQuestPrefab;

    public Camera mainCamera;       // Camera Main
    public GameObject viewPortTarget; // viewPortTarget Check

    [Space(2)]
    //[SerializeField]
    private int previousButtonIndex = 0;
    //[SerializeField]
    private List<Button> questButtons = new List<Button>();
    private Button currentButtonActive;
    private QuestCtrl currentQuestCtrl;
    private List<Button> ALLButtons = new List<Button>();  
    public Button CurrentButtonActive => currentButtonActive;
    private Image currentTickImage;
    public Image CurrentTickImage => currentTickImage;


    [SerializeField]
    private QuestAbstractSO currentQuestSO;

    [SerializeField] QuestBaseUI questBaseUI;
    [SerializeField] PanelItemReward paneltemReward;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (currentQuestSO != null)
        {
            ShowQuestDetails(currentQuestSO);
        }
        PanelItemReward.OnActivePanelItemReward += SetButton;
        PanelItemReward.OnResetCompleteQuest += ResetCompleteQuest;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PanelItemReward.OnActivePanelItemReward -= SetButton;
        PanelItemReward.OnResetCompleteQuest -= ResetCompleteQuest;
    }
    protected override void Start()
    {
        base.Start();
        this.OnCreateDisplayQuest();
        DesTable.gameObject.SetActive(false); //Set False Parent UI
        btnQuest.onClick.AddListener(OnClickClaimQuest);
    }
    public void SetButton(bool active)
    {
        foreach (var button in questButtons)
        {
            button.gameObject.SetActive(active);
        }
        foreach (var button in ALLButtons)
        {
            button.gameObject.SetActive(active);
        }
    }
    private void OnClickClaimQuest()
    {
        if (currentQuestSO == null) return;
        if (paneltemReward == null) return;

        paneltemReward.gameObject.SetActive(true);

        Transform holder = paneltemReward.transform.Find("ListItemReward");

        // Xóa các item cũ trong danh sách
        foreach (Transform o in holder)
        {
            Destroy(o.gameObject);
        }
        var createdItems = new List<GameObject>();

        foreach (var resource in currentQuestSO.questInfoSO.Reward)
        {
            GameObject objNew = Instantiate(RewardClaimManager.Instance.ItemReward, holder).gameObject;

            objNew.SetActive(false);
            ItemTooltipReward itemTooltip = objNew.GetComponent<ItemTooltipReward>();
            itemTooltip.Avatar.sprite = resource.item.Image;
            itemTooltip.CountTxt.text = $"x{resource.Count}";
            itemTooltip.ItemReward = resource.item;

            // Rairity Material
            itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resource.item.itemRarity);

            // ADD Item
            GameDataManager.Instance.OnReceiverRewardResources(resource);

            // Thêm vào danh sách để xử lý animation
            createdItems.Add(objNew);
        }
        RewardClaimManager.Instance.PlayItemsWithAnimation(createdItems, 1.3f / 2);

        SetButton(false);
        paneltemReward.SetFade(80);
        paneltemReward.SetPanels(createdItems);
    }
    private void ResetCompleteQuest()
    {
        //Set
        currentQuestSO.questInfoSO.isFinishQuest = true;

        this.UpdateNameQuest(currentQuestSO, currentQuestCtrl.TMP_Text);

        ShowQuestDetails(currentQuestSO);
    }
    private void OnCreateDisplayQuest()
    {
        // Loop through all possible quest types in the TypeQuestMain enum
        foreach (TypeQuestMain questType in Enum.GetValues(typeof(TypeQuestMain)))
        {   
            GameObject newQuestMain = Instantiate(mainQuestPrefab, holderMainQuest);

            MainQuestCtrl mainQuestCtrl = newQuestMain.GetComponent<MainQuestCtrl>();

            mainQuestCtrl.typeQuestMain = questType;

            ALLButtons.Add(mainQuestCtrl._Button);

            questBaseUI.MainQuestCtrlList.Add(mainQuestCtrl);

            Transform holder = mainQuestCtrl.HolderMain;
            //mainQuestCtrl.TMP_Text.text = "------------*" + StringExtensions.ToFormattedString(questType.ToString()) + "*------------";

            if (holder == null)
            {
                Debug.LogError("Holder Main quest Null!");
            }

            switch (questType)
            {
                case TypeQuestMain.MainQuest:
                    // Call MAin
                    CreateLevelMain(holder);
                    mainQuestCtrl.TMP_Text.text = "------------*Main Quest*-------------";

                    break;

                case TypeQuestMain.ClanQuest:
                    mainQuestCtrl.TMP_Text.text = "------------*Clan Quest*-------------";

                    GuildSO guildSO = GuildManager.Instance.GuildJoined;
                    mainQuestCtrl.CheckClanJoined(GuildManager.Instance.GuildJoined, mainQuestCtrl);

                    if (guildSO == null) break;
                    CreateClanQuest(holder, guildSO);

                    break;

                case TypeQuestMain.PvPQuest:
                    mainQuestCtrl.TMP_Text.text = "------------*PVP Quest*--------------";

                    bool checkPVP = mainQuestCtrl.CheckPVPQuest(mainQuestCtrl);

                    if (checkPVP == false) break;

                    CreatePVPQuest(holder, mainQuestCtrl);

                    break;

                case TypeQuestMain.EventQuest:
                    //Not Function
                    mainQuestCtrl.TMP_Text.text = "------------*Event Quest*------------";
                    CheckEventQuest(mainQuestCtrl);

                    break;

                default:
                    Debug.LogWarning($"Unhandled quest type: {questType}");
                    break;
            }
            mainQuestCtrl.InitializeMenuItems();
        }
    }
    #region Create Level Main
    private void CreateLevelMain(Transform holder)
    {
        lvType[] levelTypes = (lvType[])Enum.GetValues(typeof(lvType)); // Get all values ​​of the lvType enum

        for (int i = 0; i < levelTypes.Length; i++)
        {
            GameObject newQuestLV = Instantiate(lvQuestPrefab, holder); // Khởi tạo LV

            LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();


            lvQuestCtrl.Level = i + 1;

            if (lvQuestCtrl == null)
            {
                Debug.LogError("LVQuestCtrl Null!");
            }

            lvQuestCtrl.CheckPlayerLV(i+1, lvQuestCtrl);
            lvQuestCtrl.tmp_Text.text = "+"+ StringExtensions.ToFormattedString(levelTypes[i].ToString()) + (i + 1);
            ALLButtons.Add(lvQuestCtrl._Button);

            //Array QuestAbstract
            QuestAbstractSO[] questAbstractSO = QuestManager.Instance.GetQuestsByLvType(levelTypes[i]);

            //Debug.Log("So luong Quest: "+ questAbstractSO.Length);

            lvQuestCtrl.itemsCount = questAbstractSO.Length;

            for (int j = 0; j < questAbstractSO.Length; j++)
            {
                GameObject newQuestDes = Instantiate(desQuestPrefab, lvQuestCtrl.Holder);
                
                QuestCtrl questCtrl = newQuestDes.GetComponent<QuestCtrl>();

                this.UpdateNameQuest(questAbstractSO[j], questCtrl.TMP_Text);


                if (questCtrl == null)
                {
                    Debug.LogError("QuestCtrl Null!");
                }
                //Add This Buttons
                this.questButtons.Add(questCtrl.ButtonQuest);
                //ADdd QuestCtrl This
                questCtrl.SetQuestUIDisPlay(this);
                //Add Lv Quest This is QuestCtrl
                lvQuestCtrl.AddMenuItem(questCtrl);

                lvQuestCtrl.MenuItems[j].SetQuestAbstractSO(questAbstractSO[j]); //ASIGN Quest
            }
        }
    }
    #endregion
    #region Create Clan Quest
    public void CreateClanQuest(Transform holder, GuildSO guildSO)
    {
        ClanQuestType[] clanQuets = (ClanQuestType[])Enum.GetValues(typeof(ClanQuestType)); // Get all values ​​of the lvType enum

        for (int i = 0; i < clanQuets.Length; i++)
        {
            GameObject newQuestLV = Instantiate(lvQuestPrefab, holder); // Khởi tạo LV

            LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();

            lvQuestCtrl.tmp_Text.text = "+" + StringExtensions.ToFormattedString(clanQuets[i].ToString());

            //Array QuestAbstract
            QuestAbstractSO[] questAbstractSO = ClanQuestSO.GetQuestsByGuildType(
                QuestManager.Instance.GetQuestsByLvType(clanQuets[i]).OfType<ClanQuestSO>().ToArray(),
                guildSO.guildType
            );
            ALLButtons.Add(lvQuestCtrl._Button);

            lvQuestCtrl.itemsCount = questAbstractSO.Length;

            for (int j = 0; j < questAbstractSO.Length; j++)
            {
                GameObject newQuestDes = Instantiate(desQuestPrefab, lvQuestCtrl.Holder);

                QuestCtrl questCtrl = newQuestDes.GetComponent<QuestCtrl>();

                this.UpdateNameQuest(questAbstractSO[j], questCtrl.TMP_Text);

                if (questCtrl == null)
                {
                    Debug.LogError("QuestCtrl Null!");
                }
                //Add This Buttons
                this.questButtons.Add(questCtrl.ButtonQuest);
                //ADdd QuestCtrl This
                questCtrl.SetQuestUIDisPlay(this);
                //Add Lv Quest This is QuestCtrl
                lvQuestCtrl.AddMenuItem(questCtrl);

                lvQuestCtrl.MenuItems[j].SetQuestAbstractSO(questAbstractSO[j]); //ASIGN Quest
            }
        }
    }
    #endregion
    #region Create PVP Quest
    public void CreatePVPQuest(Transform holder, MainQuestCtrl mainQuestCtrl)
    {
        PVPQuestType[] pvpQuets = (PVPQuestType[])Enum.GetValues(typeof(PVPQuestType)); // Get all values ​​of the lvType enum

        for (int i = 0; i < pvpQuets.Length; i++)
        {
            GameObject newQuestLV = Instantiate(lvQuestPrefab, holder); // Khởi tạo LV

            LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();

            lvQuestCtrl.tmp_Text.text = StringExtensions.ToFormattedString(pvpQuets[i].ToString());

            //Array QuestAbstract
            QuestAbstractSO[] questAbstractSO = QuestManager.Instance.GetQuestsByLvType(pvpQuets[i]);

            ALLButtons.Add(lvQuestCtrl._Button);


            lvQuestCtrl.itemsCount = questAbstractSO.Length;

            for (int j = 0; j < questAbstractSO.Length; j++)
            {
                GameObject newQuestDes = Instantiate(desQuestPrefab, lvQuestCtrl.Holder);

                QuestCtrl questCtrl = newQuestDes.GetComponent<QuestCtrl>();

                this.UpdateNameQuest(questAbstractSO[j], questCtrl.TMP_Text);


                if (questCtrl == null)
                {
                    Debug.LogError("QuestCtrl Null!");
                }
                //Add This Buttons
                this.questButtons.Add(questCtrl.ButtonQuest);
                //ADdd QuestCtrl This
                questCtrl.SetQuestUIDisPlay(this);
                //Add Lv Quest This is QuestCtrl
                lvQuestCtrl.AddMenuItem(questCtrl);

                lvQuestCtrl.MenuItems[j].SetQuestAbstractSO(questAbstractSO[j]); //ASIGN Quest
            }
        }
    }
    #region Event Quest
    private void CheckEventQuest(MainQuestCtrl mainQuestCtrl)
    {
        //LOCK
        mainQuestCtrl.NameQuestRequiredMain = "Event Not Yet Happened";
        mainQuestCtrl.IsCheckMain = false;
        mainQuestCtrl.lockObj.SetActive(true);
        mainQuestCtrl.TMP_Text.color = new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f);
    }
    #endregion
    #endregion
    #region Handle Button
    public void QuestPress(QuestCtrl quest, Button questButton, Image tickImage)
    {
        currentQuestCtrl = quest;
        // Kiểm tra để đảm bảo previousButtonIndex là hợp lệ
        if (questButtons != null && questButtons.Count > 0 && previousButtonIndex >= 0 && previousButtonIndex < questButtons.Count)
        {
            // Tắt tickImage của nút trước đó

            HighlightQuestButton(questButtons[previousButtonIndex], currentTickImage, false);
        }

        // Bật tickImage của nút hiện tại
        HighlightQuestButton(questButton, tickImage, true);
        previousButtonIndex = questButtons.IndexOf(questButton);
    }

    private void HighlightQuestButton(Button questButton, Image tickImage, bool active)
    {
        // Làm nổi bật hoặc tắt nổi bật cho nút
        //questButton.image.color = active ? new Color(1f, 0.6f, 0f, 0.3f) : new Color(0, 0, 0, 0);

        // Bật hoặc tắt tickImage của nút
        if (tickImage != null)
        {
            tickImage.gameObject.SetActive(active);
        }

        // Cập nhật nút và tickImage hiện tại
        if (active)
        {
            currentTickImage = tickImage;
            currentButtonActive = questButton;
        }
    }

    public void ShowQuestDetails(QuestAbstractSO QuestSO)
    {
        //SetQuest
        currentQuestSO = QuestSO;

        this.UpdateNameQuest(QuestSO, nameQuest);

        DesTable.gameObject.SetActive(false); //Set False Parent UI

        descriptionQuest.text = "Description:\n+" + QuestSO.questInfoSO.bio + "\n\n";

        requireQuest.text = "Required:\n";
        foreach (Objective objectives in QuestSO.questInfoSO.objectives)
        {
            // Kiểm tra nếu currentAmount bằng requiredAmount
            // Lấy phần văn bản phía trước dấu "/"
            string objectiveText = objectives.requiredText.Split('/')[0];

            // Lấy phần số yêu cầu (phần sau dấu "/")
            string requiredAmount = objectives.requiredText.Split('/')[1];

            // Kiểm tra và thay đổi màu sắc cho currentAmount
            if (objectives.requiredCount > objectives.currentCount)
            {
                requireQuest.text = $"+{objectiveText}<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.red)}>{objectives.currentCount}</color>/{requiredAmount}";
            }
            else
            {
                requireQuest.text = $"+{objectiveText}<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(Color.green)}>{objectives.currentCount}</color>/{requiredAmount}";
            }

        }
        requireQuest.text += "\n";
        foreach (Transform child in holderReward)
        {
            Destroy(child.gameObject);
        }
        bool questComplete = false;
        //Button Handle Accept Quest, Complete Quest
        foreach (Objective objective in QuestSO.questInfoSO.objectives)
        {
            if (!objective.QuestComPlete())
            {
                questComplete = false;
                break;
            }
            questComplete = true;
        }
        if (questComplete)
        {
            btnQuest.gameObject.SetActive(true);
        }
        else
        {
            btnQuest.gameObject.SetActive(false);

        }
        if(QuestSO.questInfoSO.isFinishQuest)
        {
            btnQuest.gameObject.SetActive(false);
        }
        //btnQuest.gameObject.SetActive(true);

        rewardText.text = "Reward:\n";
        foreach (UIGameDataMap.Resources resources in QuestSO.questInfoSO.Reward)
        {
            GameObject newObjReward = Instantiate(prefabReward, holderReward);
            ItemTooltipReward itemTooltip = newObjReward.GetComponent<ItemTooltipReward>();

            itemTooltip.ItemReward = resources.item;
            itemTooltip.Avatar.sprite = resources.item.Image; 
            itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resources.item.itemRarity);
            itemTooltip.CountTxt.text = "X" + resources.Count.ToString();
 

            if (QuestSO.questInfoSO.isFinishQuest)
                itemTooltip.Tick.SetActive(true);
        }

        DesTable.gameObject.SetActive(true);//Set True Parent UI

    }
    private void UpdateNameQuest(QuestAbstractSO questAbstractSO, TMP_Text nameQuest)
    {
        if (questAbstractSO.questInfoSO.isFinishQuest)
        {
            nameQuest.text = "+"+ questAbstractSO.questInfoSO.nameQuest + " (<color=#00FF00>Finish</color>)";
        }
        else
        {
            nameQuest.text = "+"+questAbstractSO.questInfoSO.nameQuest;
        }
    }
    #endregion
}
