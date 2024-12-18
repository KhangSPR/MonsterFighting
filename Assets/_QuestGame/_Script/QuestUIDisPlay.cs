using System;
using System.Collections;
using System.Collections.Generic;
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
    protected override void OnEnable()
    {
        base.OnEnable();
        CreateDisPlayQuest();
        DesTable.gameObject.SetActive(false); //Set False Parent UI
    }

    private void CreateDisPlayQuest()
    {

        foreach (TypeQuestMain questType in Enum.GetValues(typeof(TypeQuestMain)))
        {
            Debug.Log($"Processing quest type: {questType}");

            // Kiểm tra điều kiện quest type 'clan'
            if (questType == TypeQuestMain.clan)
            {
                continue;
            }

            // Khởi tạo Quest chính
            GameObject newQuestMain = Instantiate(mainQuestPrefab, holderMainQuest);
            Transform holderNewQuestMain = newQuestMain.transform.Find("Holder");

            MainQuestCtrl mainQuestCtrl = newQuestMain.GetComponent<MainQuestCtrl>();

            if (holderNewQuestMain != null)
            {
                int lvIndex = 1;
                foreach (lvType level in Enum.GetValues(typeof(lvType)))
                {
                    GameObject newQuestLV = Instantiate(lvQuestPrefab, holderNewQuestMain); // Khởi tạo LV

                    Transform holderNewQuestLV = newQuestLV.transform.Find("Holder");

                    newQuestLV.transform.Find("Text").GetComponent<TMP_Text>().text = "+Hero Level " + lvIndex;

                    LvQuestCtrl lvQuestCtrl = newQuestLV.GetComponent<LvQuestCtrl>();
                    
                    if (lvQuestCtrl != null)
                    {
                        int i = 0;
                        foreach (var questLV in QuestManager.Instance.GetQuestsByLvType(level))
                        {
                            GameObject newQuestDes = Instantiate(desQuestPrefab, holderNewQuestLV); // Khởi tạo Quest mô tả

                            QuestCtrl questCtrl = newQuestDes.GetComponent<QuestCtrl>();
                            // Thêm Button vào List questButtons
                            if (questCtrl != null)
                            {
                                questButtons.Add(questCtrl.ButtonQuest); // Thêm ButtonQuest vào List
                                questCtrl.SetQuestUIDisPlay(this);
                                lvQuestCtrl.AddMenuItem(questCtrl);

                                lvQuestCtrl.MenuItems[i].SetQuestAbstractSO(questLV); // Gán quest
                                i++;
                            }
                        }
                        lvQuestCtrl.itemsCount = i;
                    }
                    lvIndex++;
                }
            }
            else
            {
                Debug.LogWarning("Holder not found in newQuestMain");
            }
            mainQuestCtrl.InitializeMenuItems();
        }
    }

    private void ResetClan()
    {
        // Reset hành động khi thay đổi clan
    }
    private int previousButtonIndex = 0;
    //[SerializeField]
    private List<Button> questButtons = new List<Button>();
    public void QuestPress(Button questButton)
    {
        // Kiểm tra để đảm bảo previousButtonIndex là hợp lệ
        if (questButtons != null && questButtons.Count > 0 && previousButtonIndex >= 0 && previousButtonIndex < questButtons.Count)
        {
            HighlightQuestButton(questButtons[previousButtonIndex], false);
        }

        HighlightQuestButton(questButton, true);
        previousButtonIndex = questButtons.IndexOf(questButton);
    }

    private void HighlightQuestButton(Button questButton, bool active)
    { //Làm nổi bật hoặc tắt nổi bật cho nút. 
        questButton.image.color = active ? new Color(1f, 0.6f, 0f, 0.3f) : new Color(0, 0, 0, 0);
    }
    public void ShowQuestDetails(QuestAbstractSO QuestSO)
    {
        DesTable.gameObject.SetActive(false); //Set False Parent UI

        nameQuest.text = QuestSO.questInfoSO.nameQuest;
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
            if (objectives.amount > objectives.currentAmount)
            {
                requireQuest.text = $"+{objectiveText}<color=#{ColorUtility.ToHtmlStringRGB(Color.red)}>{objectives.currentAmount}</color>/{requiredAmount}";
            }
            else
            {
                requireQuest.text = $"+{objectiveText}<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>{objectives.currentAmount}</color>/{requiredAmount}";
            }

        }
        requireQuest.text += "\n";
        foreach (Transform child in holderReward)
        {
            Destroy(child.gameObject);
        }
        rewardText.text = "Reward:\n";
        foreach (UIGameDataMap.Resources resources in QuestSO.questInfoSO.Reward)
        {
            GameObject newObjReward = Instantiate(prefabReward, holderReward);

            newObjReward.GetComponentInChildren<TMP_Text>().text = "X"+resources.Count.ToString();

            newObjReward.transform.Find("Avatar").GetComponent<Image>().sprite = resources.item.Image;
        }
        DesTable.gameObject.SetActive(true);//Set True Parent UI

        //Button Handle Accept Quest, Complete Quest
        btnQuest.gameObject.SetActive(true);
    }
}
