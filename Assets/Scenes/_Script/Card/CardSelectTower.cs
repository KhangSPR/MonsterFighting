using CodeMonkey.Utils;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectTower : MonoBehaviour
{
    [SerializeField] Button_UI button;
    [SerializeField] CardCharacter cardTower;
    public CardCharacter CardTower => cardTower;
    [SerializeField] GameObject ActiveIndicator;
    [SerializeField] Image frameCard;
    [SerializeField] Image frameName;
    [SerializeField] Image avatar;
    [SerializeField] TMP_Text textName;

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
    public void SetUICard(CardCharacter cardCharacter)
    {
        cardTower = cardCharacter;
        frameCard.sprite = cardCharacter.frame;
        frameName.sprite = cardCharacter._frameCardName;
        avatar.sprite = cardCharacter.background;
        textName.text = cardCharacter.nameCard;
    }
    public void DeCardPresenceInCardPlay()
    {
        button.hoverBehaviour_Color_Enter = new Color(1, 1, 1, 150 / 255f);
        button.hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        frameCard.color = new Color(1, 1, 1, 1);
        ActiveIndicator.SetActive(false);
    }

    private void SetActiveCardPresenceInCardPlay()
    {
        button.hoverBehaviour_Color_Enter = new Color(1, 1, 1, 1);
        button.hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        ActiveIndicator.SetActive(true);
        frameCard.color = new Color(1, 1, 1, 1);
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
