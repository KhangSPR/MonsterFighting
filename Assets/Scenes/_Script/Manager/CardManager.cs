using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;


public class CardManager : MonoBehaviour
{
    [Header("Manager Chosse Card In Game")]
    private static CardManager instance;
    public static CardManager Instance => instance;

    [SerializeField] CardManagerData cardManagerDataPlay;
    public CardManagerData CardManagerData => cardManagerDataPlay;

    [Header("Chosse Card In Game")]
    [Header("Holder Card")]
    [SerializeField] Transform Holder;
    [SerializeField] Transform HolderSlot;
    [Header("Prefabs")]
    [Header("CardSelect")]
    [SerializeField] GameObject cardSelectTower;
    [SerializeField] GameObject cardSelectMachine;
    [Header("CardSlot")]
    [SerializeField] GameObject cardSlot;
    [Header("Manager Card")]
    [SerializeField] CardALLCard cardManagerALL;
    public CardALLCard CardALLCard => cardManagerALL;

    public int slotCard;

    [Header("UI SelectCard")]
    //public CardSelectTower CurrentCardSelectTower;
    //public CardHasSelect CurrentCardHasSelect;
    [Header("Panel")]
    [SerializeField] PanelCardHasSelect panelCardHasSelect;
    public PanelCardHasSelect PanelCardHasSelect => panelCardHasSelect;
    [Header("Button")]
    [SerializeField] ButtonCard buttonCard;
    public ButtonCard ButtonCard => buttonCard;
    [Header("CardSelect Play")]
    public List<CardSelectTower> cardSelectTowers;
    private void Awake()
    {
        if (CardManager.instance != null)
        {
            Debug.LogError("Only 1 CardManager Warning");
        }
        CardManager.instance = this;

        //ClearCardALLList();
    }
    private void OnApplicationQuit()
    {
        // Reset the CardManager when the game is about to quit

        //ClearCardALLList();
    }
    //public void ClearCardALLList()
    //{
    //    cardManagerDataPlay.cardTowers.Clear();
    //}



    //Take Card Class
    public void RemoveCardFromCardManager(CardCharacter cardTower)
    {
        CardManager.Instance.CardManagerData.cardCharacter.Remove(cardTower);
        CardManagerData.SaveData();
    }
    public void AddCardToCardManager(CardCharacter cardTower)
    {
        CardManager.Instance.CardManagerData.cardCharacter.Add(cardTower);
        CardManagerData.SaveData();

    }
    public bool CheckCardPresenceInCardPlay(CardCharacter card)
    {
        foreach (CardCharacter cardTower in cardManagerDataPlay.cardCharacter)
        {
            if (cardTower.Equals(card))
            {
                return true;
            }
        }
        return false;
    }
    public void RemoALlCard()
    {
        foreach (Transform child in Holder)
        {
            Destroy(child.gameObject);
        }
    }
    public void RemoALLSlot()
    {
        foreach (Transform child in HolderSlot)
        {
            Destroy(child.gameObject);
        }
    }
    public void InstanceAllCard()
    {
        foreach (Transform child in Holder)
        {
            Destroy(child.gameObject);
        }
        foreach (CardCharacter card in cardManagerALL.CardsTower)
        {
            //for (int i = 0; i < 10; i++)
            //{
            GameObject cardObject = Instantiate(cardSelectTower, Holder);

            //Set CardSelectTower script
            CardSelectTower cardSelectTowerScript = cardObject.GetComponent<CardSelectTower>();

            cardSelectTowerScript.CardTower = card;

            //Take Card Select Play
            TakeCardSelectPlay(cardSelectTowerScript);

            //Settings
            cardObject.transform.Find("Frame").GetComponent<Image>().sprite = card.frame;
            cardObject.transform.Find("Background").GetComponent<Image>().sprite = card.background;
            //cardObject.transform.Find("Top/Avatar").GetComponent<Image>().sprite = card.avatar;

            cardObject.transform.Find("Name").GetComponent<TMP_Text>().text = card.nameCard;

        }
    }
    public void InstanceCardSlot()
    {
        foreach (Transform child in HolderSlot)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotCard; i++)
        {
            GameObject cardObject = Instantiate(cardSlot, HolderSlot);

            Debug.Log(i);
        }
    }
    //CardManager.Instance.PanelCardHasSelect.LoadPanelCardHasSelect();
    //    CardManager.Instance.CheckCardIsWorking();
    //    CardManager.Instance.PanelCardHasSelect.CheckForEnoughCard();
    //    CardManager.Instance.CheckCardActive();

    void InstanceAllCardMachine()
    {

    }
    public void AddPanel(CardCharacter cardTower, CardSelectTower cardSelectTower)
    {
        for (int i = 0; i < PanelCardHasSelect.CardHasSelects.Count; i++)
        {
            CardHasSelect cardHasSelect = PanelCardHasSelect.CardHasSelects[i];
            if (!cardHasSelect.Card.activeSelf)
            {
                cardHasSelect.cardSelectTower = cardSelectTower;
                cardHasSelect.CardTower = cardTower;
                cardHasSelect.SettingCard(cardTower);
                cardHasSelect.Card.SetActive(true);

                //Card Check Enough
                PanelCardHasSelect.CheckForEnoughCard();
                break;
            }
        }
    }
    public void RemovePanel(CardHasSelect cardHasSelect)
    {
        for (int i = 0; i < PanelCardHasSelect.CardHasSelects.Count; i++)
        {
            if (PanelCardHasSelect.CardHasSelects[i] == cardHasSelect)
            {
                if (cardHasSelect.Card.activeSelf)
                {
                    cardHasSelect.cardSelectTower.DeCardPresenceInCardPlay();
                    cardHasSelect.CardTower = null;
                    cardHasSelect.Card.SetActive(false);

                    //Card Check Enough
                    PanelCardHasSelect.CheckForEnoughCard();
                    break;
                }
            }
        }
    }
    void TakeCardSelectPlay(CardSelectTower cardSelectTower)
    {
        foreach (CardCharacter cardTower in cardManagerDataPlay.cardCharacter)
        {
            if (cardSelectTower.CardTower == cardTower)
            {
                cardSelectTowers.Add(cardSelectTower);

                Debug.Log(cardTower);
            }
        }
    }
    public void CheckCardIsWorking()
    {
        // Kiểm tra nếu không có dữ liệu trong cardSelectTowers hoặc cardManagerDataPlay.cardTowers
        if (cardSelectTowers.Count < 1 || cardManagerDataPlay.cardCharacter.Count < 1)
            return;


        int i = 0; // Reset i về 0 ở đầu vòng lặp while
        foreach (CardHasSelect cardHasSelect in panelCardHasSelect.CardHasSelects)
        {
            // Kiểm tra xem có phải đã đủ dữ liệu trong cardManagerDataPlay.cardTowers hay không
            if (i >= cardManagerDataPlay.cardCharacter.Count)
                return;

            cardHasSelect.CardTower = cardManagerDataPlay.cardCharacter[i];
            cardHasSelect.SettingCard(cardHasSelect.CardTower);
            for (int j = 0; j < cardSelectTowers.Count; j++)
            {
                if (cardHasSelect.CardTower == cardSelectTowers[j].CardTower)
                {
                    cardHasSelect.cardSelectTower = cardSelectTowers[j];
                }
            }
            i++;
        }

    }



    public void CheckCardActive()
    {
        foreach (CardHasSelect cardHasSelect in panelCardHasSelect.CardHasSelects)
        {
            if (cardHasSelect.CardTower != null)
            {
                cardHasSelect.Card.SetActive(true);
            }
            else
            {
                cardHasSelect.Card.SetActive(false);
            }
        }
    }

}
