using CodeMonkey.Utils;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardHasSelect : MonoBehaviour
{
    [SerializeField] public GameObject Card;
    [SerializeField] CardCharacter cardTower;
    [SerializeField] Button_UI button;

    [Header("UI Setting")]
    [SerializeField] Image Background;
    [SerializeField] Image Frame;
    [SerializeField] Image FrameName;
    [SerializeField] TMP_Text Name;

    public CardCharacter CardTower
    {
        get { return cardTower; }
        set { cardTower = value; }
    }

    public CardSelectTower cardSelectTower;
    private void Start()
    {
        this.button.ClickFunc = () => RemoveCard();
    }
    void RemoveCard()
    {
        if (cardTower is CardPlayer) return;
        CardManager.Instance.RemoveCardFromCardManager(cardTower);
        CardManager.Instance.RemovePanel(this);
    }
    public void SettingCard(CardCharacter cardTower)
    {
        Background.sprite = cardTower.background;
        Frame.sprite = cardTower.frame;
        Name.text = cardTower.nameCard;
        FrameName.sprite = cardTower._frameCardName;
    }
}