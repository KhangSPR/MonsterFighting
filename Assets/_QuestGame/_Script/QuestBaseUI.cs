using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestBaseUI : MonoBehaviour
{
    [SerializeField] QuestUIDisPlay questUIDisPlay;
    //Main Quest Ctrl
    public List<MainQuestCtrl> MainQuestCtrlList = new List<MainQuestCtrl>();
    private void OnEnable()
    {
        PlayerManager.OnQuestUIDisplayUpdate += UpdateMainQuest;
        GuildManager.OnGuildDisplayQuest += UpdateGuildJoinedQuest;
        PlayerManager.OnQuestPVP += UpdatePVPQuest;
    }
    private void OnDisable()
    {
        PlayerManager.OnQuestUIDisplayUpdate -= UpdateMainQuest;
        GuildManager.OnGuildDisplayQuest -= UpdateGuildJoinedQuest;
        PlayerManager.OnQuestPVP -= UpdatePVPQuest;
    }
    private void UpdateMainQuest()
    {
        if (MainQuestCtrlList.Count <= 0) return;

        foreach (MainQuestCtrl mainQuestCtrl in MainQuestCtrlList)
        {
            if (mainQuestCtrl.typeQuestMain == TypeQuestMain.MainQuest)
            {
                foreach (LvQuestCtrl lvQuestCtrl in mainQuestCtrl.MenuItems)
                {
                    lvQuestCtrl.CheckPlayerLV(lvQuestCtrl.Level, lvQuestCtrl);
                }
            }
        }
    }
    //Later
    private void UpdatePVPQuest()
    {
        if (MainQuestCtrlList.Count <= 0) return;

        foreach (MainQuestCtrl mainQuestCtrl in MainQuestCtrlList)
        {
            if (mainQuestCtrl.typeQuestMain == TypeQuestMain.PvPQuest)
            {
                mainQuestCtrl.CheckPVPQuest(mainQuestCtrl);

                Transform holder = mainQuestCtrl.HolderMain;

                questUIDisPlay.CreatePVPQuest(holder, mainQuestCtrl);

                mainQuestCtrl.InitializeMenuItems();

            }
        }
    }
    private void UpdateGuildJoinedQuest(GuildSO guildSOJoined)
    {
        if (MainQuestCtrlList.Count <= 0) return;

        foreach (MainQuestCtrl mainQuestCtrl in MainQuestCtrlList)
        {
            if (mainQuestCtrl.typeQuestMain == TypeQuestMain.ClanQuest)
            {
                mainQuestCtrl.CheckClanJoined(guildSOJoined, mainQuestCtrl);
                Transform holder = mainQuestCtrl.HolderMain;

                questUIDisPlay.CreateClanQuest(holder, guildSOJoined);

                mainQuestCtrl.InitializeMenuItems();

            }
        }
    }
}
