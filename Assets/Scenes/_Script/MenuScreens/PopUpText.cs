using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    // rich text color highlight
    public static string TextHighlight = "F8BB19";

    [SerializeField] Text m_PopUpText;

    // customizes active/inactive styles, duration, and delay for each text prompt
    float m_Delay = 0f;
    float m_Duration = 1f;

    void OnEnable()
    {
        ButtonCard.CardClicked += OnTransactionFailed;
        ButtonClan.CardClicked += OnShowNotClanYet;
        EnergyUI.LevelInfoClicked += OnShowhasEnergy;
        LvQuestCtrl.LvQuestCtrlClicked += OnLvQuestCtrl;
        MainQuestCtrl.LvMainCtrlClicked += OnMainQuest;

    }

    void OnDisable()
    {
        ButtonCard.CardClicked -= OnTransactionFailed;
        ButtonClan.CardClicked -= OnShowNotClanYet;
        EnergyUI.LevelInfoClicked -= OnShowhasEnergy;
        LvQuestCtrl.LvQuestCtrlClicked -= OnLvQuestCtrl;
        MainQuestCtrl.LvMainCtrlClicked -= OnMainQuest;

    }

    protected void Awake()
    {

        SetupText();
        HideText();
    }


    void ShowMessage(string message)
    {
        if (m_PopUpText == null || string.IsNullOrEmpty(message))
        {
            return;
        }

        StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        if (m_PopUpText != null)
        {

            // reset any leftover Selectors
            SetupText();

            m_PopUpText.text = message;

            // hide text
            HideText();

            // wait for delay
            yield return new WaitForSeconds(m_Delay);

            // show text and wait for duration
            ShowText();
            yield return new WaitForSeconds(m_Duration);

            // hide text again
            HideText();
        }
    }

    void HideText()
    {
        if (m_PopUpText != null)
        {
            m_PopUpText.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    void ShowText()
    {
        if (m_PopUpText != null)
        {
            m_PopUpText.transform.parent.parent.gameObject.SetActive(true);
            Debug.Log("show");
        }
    }

    void SetupText()
    {
        if (m_PopUpText != null)
        {
            m_PopUpText.gameObject.GetComponent<Text>().text = string.Empty;
        }
    }
    // event-handling methods

    void OnTransactionFailed(int count, Vector2 position)
    {
        Debug.Log("OnTransactionFailed");
        // use a longer delay, messages are longer here
        m_Delay = 0f;
        m_Duration = 1.2f;
        m_PopUpText.rectTransform.anchoredPosition = position;
        string failMessage = "You need to choose more \n<color=#"
        + PopUpText.TextHighlight + ">" + count + " card</color>.";
        //"Buy more <color=#" + PopUpText.TextHighlight + ">" + shopItemSO.CostInCurrencyType + "</color>.";
        ShowMessage(failMessage);
    }
    void OnShowNotClanYet(Vector2 position)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        m_PopUpText.rectTransform.anchoredPosition = position;
        string failMessage = "Not Joined Clan Yet";
        ShowMessage(failMessage);

    }
    void OnShowhasEnergy(Vector2 position)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        m_PopUpText.rectTransform.anchoredPosition = position;
        string failMessage = "Not Enough Energy";
        ShowMessage(failMessage);

    }
    void OnLvQuestCtrl(Vector2 position, int level)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        m_PopUpText.rectTransform.anchoredPosition = position;
        string failMessage = "Not Enough Level " + level;
        ShowMessage(failMessage);
    }
    void OnMainQuest(Vector2 position, string _string)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        m_PopUpText.rectTransform.anchoredPosition = position;
        string failMessage = _string;
        ShowMessage(failMessage);
    }
    //void OnGearSelected(EquipmentSO gear)
    //{
    //    m_Delay = 0f;
    //    m_Duration = 0.8f;

    //    // centered on CharScreen
    //    m_ActiveClass = k_GearActiveClass;
    //    m_InactiveClass = k_GearInactiveClass;

    //    string equipMessage = "<color=#" + PopUpText.TextHighlight + ">" + gear.equipmentName + "</color> equipped.";
    //    ShowMessage(equipMessage);
    //}

    //void OnHomeMessageShown(string message)
    //{

    //    // welcome the player when on the HomeScreen
    //    if (Time.time >= m_TimeToNextHomeMessage)
    //    {
    //        // timing and position
    //        m_Delay = 0.25f;
    //        m_Duration = 1.5f;

    //        // centered below title
    //        m_ActiveClass = k_HomeActiveClass;
    //        m_InactiveClass = k_HomeInactiveClass;

    //        ShowMessage(message);

    //        // cooldown delay to avoid spamming
    //        m_TimeToNextHomeMessage = Time.time + k_HomeMessageCooldown;
    //    }
    //}

    //void OnCharacterLeveledUp(bool state)
    //{
    //    // only show text warning on failure
    //    if (!state)
    //    {
    //        // timing and position
    //        m_Delay = 0f;
    //        m_Duration = 0.8f;

    //        // centered on CharScreen
    //        m_ActiveClass = k_GearActiveClass;
    //        m_InactiveClass = k_GearInactiveClass;

    //        if (m_PopUpText != null)
    //        {
    //            string equipMessage = "Insufficient potions to level up.";
    //            ShowMessage(equipMessage);
    //        }
    //    }
    //}
}

