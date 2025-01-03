using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private int previousButtonIndex = 0;
    //[SerializeField]
    private List<Button> questButtons = new List<Button>();
    private Button currentButtonActive;
    public Button CurrentButtonActive => currentButtonActive;
    private Image currentTickImage;
    public Image CurrentTickImage => currentTickImage;


    [SerializeField]
    private QuestAbstractSO currentQuestSO;

    protected override void Update()
    {
        base.Update();

        // Chỉ gọi kiểm tra nếu `currentButtonActive` khác null
        if (currentButtonActive != null)
        {
            UpdateSingleButtonVisibility(currentButtonActive);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if(currentQuestSO != null)
        {
            ShowQuestDetails(currentQuestSO);
        }
    }
    protected override void Start()
    {
        base.Start();
        this.OnCreateDisplayQuest();
        DesTable.gameObject.SetActive(false); //Set False Parent UI
    }

    private void OnCreateDisplayQuest()
    {
        // Loop through all possible quest types in the TypeQuestMain enum
        foreach (TypeQuestMain questType in Enum.GetValues(typeof(TypeQuestMain)))
        {   
            GameObject newQuestMain = Instantiate(mainQuestPrefab, holderMainQuest);

            MainQuestCtrl mainQuestCtrl = newQuestMain.GetComponent<MainQuestCtrl>();

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
                    CreateClanQuest(holder, mainQuestCtrl);
                    mainQuestCtrl.TMP_Text.text = "------------*Clan Quest*-------------";
                    break;

                case TypeQuestMain.PvPQuest:
                    CreatePVPQuest(holder, mainQuestCtrl);
                    mainQuestCtrl.TMP_Text.text = "------------*PVP Quest*--------------";

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

            if(lvQuestCtrl == null)
            {
                Debug.LogError("LVQuestCtrl Null!");
            }

            this.CheckPlayerLV(i, lvQuestCtrl);
            lvQuestCtrl.tmp_Text.text = "+"+ StringExtensions.ToFormattedString(levelTypes[i].ToString()) + (i + 1);

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
    private void CheckPlayerLV(int level, LvQuestCtrl lvQuestCtrl)
    {
        if ((level + 1) > PlayerManager.Instance.LvPlayer)
        {
            //LOCK
            lvQuestCtrl.lockLv.SetActive(true);
            lvQuestCtrl.tmp_Text.color = new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f);
            lvQuestCtrl._Button.enabled = false;
        }
        else
        {
            //UNLOCK
            lvQuestCtrl.lockLv.SetActive(false);
            lvQuestCtrl.tmp_Text.color = new Color(1, 1, 1, 1);
            lvQuestCtrl._Button.enabled = true;
        }
    }
    #endregion
    #region Create Clan Quest
    private void CreateClanQuest(Transform holder, MainQuestCtrl mainQuestCtrl)
    {
        GuildSO guildSO = GuildManager.Instance.GuildJoined;

        bool checkJoined = this.CheckClanJoined(guildSO, mainQuestCtrl);

        Debug.Log("Check Guild Joined:" + checkJoined);

        if (checkJoined == false) return;

        ClanQuestType[] clanQuets = (ClanQuestType[])Enum.GetValues(typeof(ClanQuestType)); // Get all values ​​of the lvType enum

        for (int i = 0; i < clanQuets.Length; i++)
        {
            GameObject newQuestLV = Instantiate(lvQuestPrefab, holder); // Khởi tạo LV

            LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();

            lvQuestCtrl.tmp_Text.text = "+"+ StringExtensions.ToFormattedString(clanQuets[i].ToString());

            //Array QuestAbstract
            QuestAbstractSO[] questAbstractSO = ClanQuestSO.GetQuestsByGuildType(
                QuestManager.Instance.GetQuestsByLvType(clanQuets[i]).OfType<ClanQuestSO>().ToArray(),
                guildSO.guildType
            );

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
    bool CheckClanJoined(GuildSO guildSO, MainQuestCtrl mainQuestCtrl)
    {
        if (guildSO == null)
        {
            //LOCK
            mainQuestCtrl.lockObj.SetActive(true);
            mainQuestCtrl.TMP_Text.color = new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f);
            mainQuestCtrl._Button.enabled = false;

            return false;
        }
        else
        {
            //UNLOCK
            mainQuestCtrl.lockObj.SetActive(false);
            mainQuestCtrl.TMP_Text.color = new Color(1, 1, 1, 1);
            mainQuestCtrl._Button.enabled = true;

            return true;
        }

    }
    #endregion
    #region Create PVP Quest
    private void CreatePVPQuest(Transform holder, MainQuestCtrl mainQuestCtrl)
    {
        bool checkJoined = this.CheckPVPQuest(mainQuestCtrl);

        if (!checkJoined) return;

        PVPQuestType[] pvpQuets = (PVPQuestType[])Enum.GetValues(typeof(PVPQuestType)); // Get all values ​​of the lvType enum

        for (int i = 0; i < pvpQuets.Length; i++)
        {
            GameObject newQuestLV = Instantiate(lvQuestPrefab, holder); // Khởi tạo LV

            LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();

            lvQuestCtrl.tmp_Text.text = StringExtensions.ToFormattedString(pvpQuets[i].ToString());

            //Array QuestAbstract
            QuestAbstractSO[] questAbstractSO = QuestManager.Instance.GetQuestsByLvType(pvpQuets[i]);


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
        mainQuestCtrl.lockObj.SetActive(true);
        mainQuestCtrl.TMP_Text.color = new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f);
        mainQuestCtrl._Button.enabled = false;
    }
    #endregion
    bool CheckPVPQuest(MainQuestCtrl mainQuestCtrl)
    {
        if (PlayerManager.Instance.LvPlayer <= 5)
        {
            //LOCK
            mainQuestCtrl.lockObj.SetActive(true);
            mainQuestCtrl.TMP_Text.color = new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f);
            mainQuestCtrl._Button.enabled = false;

            return false;
        }
        else
        {
            //UNLOCK
            mainQuestCtrl.lockObj.SetActive(false);
            mainQuestCtrl.TMP_Text.color = new Color(1, 1, 1, 1);
            mainQuestCtrl._Button.enabled = true;

            return true;
        }

    }
    #endregion
    #region Handle Button
    void UpdateSingleButtonVisibility(Button button)
    {
        // Lấy góc dưới cùng bên trái và trên cùng bên phải của `viewPortTarget`
        RectTransform viewPortTargetRect = viewPortTarget.GetComponent<RectTransform>();
        Vector3[] worldCorners = new Vector3[4];
        viewPortTargetRect.GetWorldCorners(worldCorners);

        Vector3 bottomLeft = worldCorners[0]; // Góc dưới cùng bên trái
        Vector3 topRight = worldCorners[2];  // Góc trên cùng bên phải

        // Vị trí của `button` trong World Space
        Vector3 buttonPosition = button.transform.position;

        // Kiểm tra nếu button nằm ngoài `viewport`
        bool isOutsideViewport =
            buttonPosition.x < bottomLeft.x || buttonPosition.x > topRight.x ||
            buttonPosition.y < bottomLeft.y || buttonPosition.y > topRight.y;

        // Bật/tắt Image của button dựa trên trạng thái
        button.GetComponent<Image>().enabled = !isOutsideViewport;

    }

    public void QuestPress(Button questButton, Image tickImage)
    {
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
                requireQuest.text = $"+{objectiveText}<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}>{objectives.currentCount}</color>/{requiredAmount}";
            }
            else
            {
                requireQuest.text = $"+{objectiveText}<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>{objectives.currentCount}</color>/{requiredAmount}";
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
            itemTooltip.CountTxt.text = "X" + resources.Count.ToString();
            itemTooltip.AvatarImg.sprite = resources.item.Image;

            if (questComplete)
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
