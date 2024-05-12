using CodeMonkey.Utils;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectTower : MonoBehaviour
{
    [SerializeField] Button_UI button;
    [SerializeField] CardCharacter cardTower;
    [SerializeField] GameObject ActiveIndicator;
    [SerializeField] Image frame;

    public CardCharacter CardTower
    {
        get { return cardTower; }
        set { cardTower = value; }
    }

    private void Start()
    {
        SetupButton();
        UpdateUI();
    }

    private void SetupButton()
    {
        button.ClickFunc = () =>
        {
            AddCardPanelHasSelect();
        };
    }

    private void UpdateUI()
    {
        if (CheckCardPresence())
        {
            SetActiveCardPresenceInCardPlay();
        }
        else
        {
            DeCardPresenceInCardPlay();
        }
    }

    public void DeCardPresenceInCardPlay()
    {
        button.hoverBehaviour_Color_Enter = new Color(1, 1, 1, 150 / 255f);
        button.hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        frame.color = new Color(1, 1, 1, 1);
        ActiveIndicator.SetActive(false);
    }

    private void SetActiveCardPresenceInCardPlay()
    {
        button.hoverBehaviour_Color_Enter = new Color(1, 1, 1, 1);
        button.hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        ActiveIndicator.SetActive(true);
        frame.color = new Color(1, 1, 1, 1);
    }

    private void AddCardPanelHasSelect()
    {
        if (!CheckCardPresence())
        {
            CardManager.Instance.AddCardToCardManager(cardTower);
            CardManager.Instance.AddPanel(cardTower, this);
            SetActiveCardPresenceInCardPlay();
        }
    }

    private bool CheckCardPresence()
    {
        return CardManager.Instance.CheckCardPresenceInCardPlay(cardTower);
    }
}
