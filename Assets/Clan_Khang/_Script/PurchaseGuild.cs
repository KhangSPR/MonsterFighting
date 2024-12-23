using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseGuild : MonoBehaviour
{

    [SerializeField] Button m_ButtonAccept;
    [SerializeField] Button m_ButtonCancel;
    [SerializeField] TMP_Text m_DesText;

    string m_DesT = "Do you want to join the Guild";
    public string m_NameGuild;


    [SerializeField] GuildUICtrl guildUI;

    GuildSO guildSO;
    public GuildSO GuildSO { get { return guildSO; }set { guildSO = value; } }
    private void Start()
    {
        m_ButtonCancel.onClick.AddListener(OnClickButtonCancel);
        m_ButtonAccept.onClick.AddListener(OnClickButtonAccpet);
    }
    private void OnEnable()
    {
        SetDesText();

    }
    private void OnDisable()
    {
        
    }
    void SetDesText()
    {
        m_DesText.text = m_DesT+ "\n" + m_NameGuild + "?";
    }
    void OnClickButtonCancel()
    {
        gameObject.SetActive(false);
    }
    void OnClickButtonAccpet()
    {
        if (guildSO.Cost > GameDataManager.Instance.GameData.badGe) return;

        GameDataManager.Instance.GameData.badGe -= guildSO.Cost;
        GuildManager.Instance.IsActiveJoined(guildSO);

        CardManager.Instance.CardManagerData.RemoveAllJoinedGuild();

        guildUI.IsActivePurchase();

        gameObject.SetActive(false);

        GameDataManager.Instance.UpdateFunds();

        Debug.Log("badGe:" + GameDataManager.Instance.GameData.badGe);
    }
}
