using System.Collections;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    // rich text color highlight
    public static string TextHighlight = "F8BB19";

    [SerializeField] Text m_PopUpText;
    [SerializeField] Image m_Frame;

    // customizes active/inactive styles, duration, and delay for each text prompt
    float m_Delay = 0f;
    float m_Duration = 1f;

    private void Start()
    {
        if(PlayerManager.Instance.IsDiaLog)
        {
            OnPopupStart();
        }
    }
    private void OnPopupStart()
    {
        m_Delay = 0f;
        m_Duration = 1.8f;
        string failMessage = "Welcome to Kingdom";
        ShowMessage(failMessage);
    }
    void OnEnable()
    {
        DialogUI.OnPopUpText += OnPopupStart;
        ButtonCard.CardClicked += OnTransactionFailed;
        ButtonClan.CardClicked += OnShowNotClanYet;
        EnergyUI.LevelInfoClicked += OnShowhasEnergy;
        LvQuestCtrl.LvQuestCtrlClicked += OnLvQuestCtrl;
        MainQuestCtrl.LvMainCtrlClicked += OnMainQuest;
        GameDataManager.OnPopUpSpin += OnShowhasSpin;
    }

    void OnDisable()
    {
        DialogUI.OnPopUpText -= OnPopupStart;
        ButtonCard.CardClicked -= OnTransactionFailed;
        ButtonClan.CardClicked -= OnShowNotClanYet;
        EnergyUI.LevelInfoClicked -= OnShowhasEnergy;
        LvQuestCtrl.LvQuestCtrlClicked -= OnLvQuestCtrl;
        MainQuestCtrl.LvMainCtrlClicked -= OnMainQuest;
        GameDataManager.OnPopUpSpin -= OnShowhasSpin;
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
    IEnumerator FadeFrameEffect(float duration)
    {
        CanvasGroup frameCanvasGroup = m_Frame.GetComponent<CanvasGroup>();
        if (frameCanvasGroup == null)
        {
            frameCanvasGroup = m_Frame.gameObject.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;
        float minAlpha = 0.1f;
        float maxAlpha = 1f;

        // Lặp trong khoảng thời gian
        while (elapsedTime < duration)
        {
            // Tăng giá trị alpha từ minAlpha đến maxAlpha theo thời gian
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, elapsedTime / duration);
            frameCanvasGroup.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo alpha đạt giá trị max khi hoàn thành
        frameCanvasGroup.alpha = maxAlpha;
    }


    IEnumerator ShowMessageRoutine(string message)
    {
        if (m_PopUpText != null)
        {
            // Reset Selectors và thiết lập nội dung text
            SetupText();
            m_PopUpText.text = message;

            // Ẩn text trước khi hiện
            HideText();

            // Đợi delay
            yield return new WaitForSeconds(m_Delay);

            // Hiện text và bắt đầu hiệu ứng fade cho frame
            ShowText();
            StartCoroutine(FadeFrameEffect(0.35f));

            // Đợi thời gian hiển thị
            yield return new WaitForSeconds(m_Duration);

            // Ẩn text sau khi hoàn thành
            HideText();
        }
    }


    void HideText()
    {
        if (m_PopUpText != null)
        {
            m_PopUpText.transform.parent.gameObject.SetActive(false);
        }
    }

    void ShowText()
    {
        if (m_PopUpText != null)
        {
            m_PopUpText.transform.parent.gameObject.SetActive(true);
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

    void OnTransactionFailed(int count)
    {
        Debug.Log("OnTransactionFailed");
        // use a longer delay, messages are longer here
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = "You need to choose more \n<color=#"
        + PopUpText.TextHighlight + ">" + count + " card</color>.";
        //"Buy more <color=#" + PopUpText.TextHighlight + ">" + shopItemSO.CostInCurrencyType + "</color>.";
        ShowMessage(failMessage);
    }
    void OnShowNotClanYet()
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = "Not Joined Clan Yet";
        ShowMessage(failMessage);

    }
    void OnShowhasEnergy()
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = "Not Enough Energy";
        ShowMessage(failMessage);
    }
    void OnShowhasSpin()
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = "Not Enough Magical Crystal";
        ShowMessage(failMessage);

    }
    void OnLvQuestCtrl(int level)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = "Not Enough Level " + level;
        ShowMessage(failMessage);
    }
    void OnMainQuest(string _string)
    {
        m_Delay = 0f;
        m_Duration = 1.2f;
        string failMessage = _string;
        ShowMessage(failMessage);
    }
}

