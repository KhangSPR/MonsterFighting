using CodeMonkey.Utils;
using UIGameDataManager;
using UnityEngine;


public class ToggleCard : MonoBehaviour
{
    //[SerializeField] Button_UI button_UI;
    //[SerializeField] ClickHandler clickHandler;
    //[SerializeField] CardTower cardTower;
    //public ClickHandler ClickHandler { get { return clickHandler; } set { clickHandler = value; } }

    //[SerializeField] bool isTurnOnActive;
    //private void Start()
    //{
    //    button_UI.MouseUpFunc = () => ToggleTurnOn();

    //}
    //public void SetCardTower(CardTower Tower)
    //{
    //    cardTower = Tower;
    //}
    //void ToggleTurnOn()
    //{
    //    if (CardManager.Instance.Active)
    //    {
    //        if (clickHandler.Cards.Contains(this))
    //        {
    //            isTurnOnActive = !isTurnOnActive;
    //            SetActiveFade(isTurnOnActive);
    //        }
    //    }
    //    else
    //    {
    //        isTurnOnActive = !isTurnOnActive;
    //        SetActiveFade(isTurnOnActive);

    //    }
    //}
    //private void SetActiveFade(bool active)
    //{
    //    if (!active)
    //    {
    //        RemovePins();
    //        CardManager.Instance.RemoveCardFromCardManager(cardTower);
    //        clickHandler.Cards.Remove(this);
    //        button_UI.hoverBehaviour_Color_Enter = new Color(100 / 255f, 100 / 255f, 100 / 255f, 30 / 255f);
    //        button_UI.hoverBehaviour_Color_Exit = new Color(0, 0, 0, 150 / 255f);
    //        CardManager.Instance.Active = false;
    //    }
    //    else
    //    {
    //        AddPins();
    //        CardManager.Instance.AddCardToCardManager(cardTower);
    //        clickHandler.Cards.Add(this);
    //        button_UI.hoverBehaviour_Color_Exit = new Color(0, 0, 0, 0);

    //        if (CardManager.Instance.CardManagerData.cardTowers.Count > 2)
    //        {
    //            CardManager.Instance.Active = true;

    //            return;
    //        }
    //    }
    //}
    //void AddPins()
    //{
    //    for(int i = 0;i< ClickHandler.Pins.Length;i++)
    //    {
    //        if (!ClickHandler.Pins[i].activeSelf)
    //        {
    //            ClickHandler.Pins[i].SetActive(true);
    //            break;
    //        }
    //    }    
    //}
    //void RemovePins()
    //{
    //    for (int i = ClickHandler.Pins.Length - 1; i >= 0; i--)
    //    {
    //        if (ClickHandler.Pins[i].activeSelf)
    //        {
    //            ClickHandler.Pins[i].SetActive(false);
    //            break;
    //        }
    //    }
    //}

}
