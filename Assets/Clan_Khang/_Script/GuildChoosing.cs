using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuildChoosing : MonoBehaviour
{
    [SerializeField] Image Icon;
    public Image ICON => Icon;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;

    [Header("Purchase")]
    [SerializeField] TMP_Text m_Des;
    [SerializeField] Button m_Button;
    public Button ButtonPurchase => m_Button;
    [Space]
    [Space]
    [Header("Joined")]
    [SerializeField] GameObject m_Joined;

    public TextBlur TextBlur;

    GuildSO m_guildSO;
    public GuildSO GuildSO => m_guildSO;

    GuildUICtrl GuildUI;   
    //[SerializeField] BlurManager blurManager;
    //public BlurManager BlurManager => blurManager;
    private void Start()
    {
        m_Button.onClick.AddListener(OnClickPurchase);
    }
    void OnClickPurchase()
    {
        GuildUI.PurchaseGuild.m_NameGuild = Name.text;
        GuildUI.PurchaseGuild.GuildSO = m_guildSO;

        GuildUI.PurchaseGuild.gameObject.SetActive(true);
        Debug.Log("GuildName: " + GuildUI.PurchaseGuild.m_NameGuild);

    }
    public void SetUI(GuildSO guildSO, GuildUICtrl guildUICtrl)
    {
        if (GuildUI == null) GuildUI = guildUICtrl;


        Icon.sprite = guildSO.GuildIcon;
        Name.text = guildSO.name;
        Description.text = guildSO.GuildDescription;
        m_guildSO = guildSO;
        //
        if (guildSO.Joined)
        {
            m_Joined.SetActive(true);
            m_Des.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_Des.text = "Join " + "<color=white>" + guildSO.Cost + "</color>";
            m_Joined.SetActive(false);
            m_Des.transform.parent.gameObject.SetActive(true);
            //if (guildSO.Cost <= GameDataManager.Instance.GameData.badGe)
            //{
            //    m_Button.interactable = true;

            //    Debug.Log("1");
            //}
            //else
            //{
            //    m_Button.interactable = false;
            //    Debug.Log("2");


            //}
        }

    }
}
