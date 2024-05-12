using CodeMonkey.Utils;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardHasSelect : MonoBehaviour
{
    [SerializeField] GameObject card;
    public GameObject Card => card;
    [SerializeField] CardCharacter cardTower;
    [SerializeField] Button_UI button;

    [Header("UI Setting")]
    [SerializeField] Image Background;
    [SerializeField] Image Frame;
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
        CardManager.Instance.RemoveCardFromCardManager(cardTower);
        CardManager.Instance.RemovePanel(this);
    }
    public void SettingCard(CardCharacter cardTower)
    {
        Background.sprite = cardTower.background;
        Frame.sprite = cardTower.frame;
        Name.text = cardTower.nameCard;
    }
}