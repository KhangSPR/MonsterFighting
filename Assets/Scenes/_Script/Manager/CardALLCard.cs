using System.Collections.Generic;
using System.Linq;
using UIGameDataManager;
using UnityEngine;

[CreateAssetMenu(fileName = "CardManagerALLCard", menuName = "Custom/CardManagerALLCard", order = 1)]
public class CardALLCard : ScriptableObject
{
    [SerializeField]
    List<CardCharacter> cardCharacters = new List<CardCharacter>();
    public List<CardCharacter> CardCharacters => cardCharacters;
    List<CardMachine> cardMachines = new List<CardMachine>();
    public List<CardMachine> CardMachines => cardMachines;

    const string resPathCardPlayer = "Card/CardSAOJ/CardPlayer";
    const string resPathCardCharacter = "Card/CardSAOJ/CardCharacter"; //CardPlayer
    const string resPathCardMachine = "Card/CardSAOJ/CardMachine";

    public void LoadSumALLCard()
    {
        LoadAllCardTowerScriptableObjects();
        LoadAllCardMachineScriptableObjects();
        LoadDataCard();
    }
    private void LoadAllCardTowerScriptableObjects()
    {
        CardCharacter[] loadedCards = Resources.LoadAll<CardCharacter>(resPathCardCharacter);
        cardCharacters = new List<CardCharacter>(loadedCards);
    }
    private void LoadAllCardMachineScriptableObjects()
    {
        CardMachine[] loadedCards = Resources.LoadAll<CardMachine>(resPathCardMachine);
        cardMachines = new List<CardMachine>(loadedCards);
    }
    private void LoadDataCard()
    {
        cardCharacters = SortCardCharacterByCost();
        cardMachines = SortCardMachineByCost();
    }
    List<CardCharacter> SortCardCharacterByCost()
    {
        return cardCharacters.OrderBy(x => x.price).ToList();
    }
    List<CardMachine> SortCardMachineByCost()
    {
        return cardMachines.OrderBy(x => x.price).ToList();
    }
    //Card Guard
    //List<CardCharacter> SortCardGuardByCost(List<Ca> originalList)
    //{
    //    return originalList.OrderBy(x => x.price).ToList();
    //}
    public List<CardCharacter> GetCharacterAttackType(AttackCategory attackType)
    {
        return CardCharacters.Where(x => x.GetAttackType() == attackType).ToList();
    }
    public List<CardCharacter> GetCardsByGuild(GuildType guildType)
    {
        return cardCharacters.Where(c => c.guildType == guildType).ToList();
    }
    public void LoadDataCardPlayer()
    {
        //cardCharacters.Insert(0, PlayerManager.Instance.CardCurrentPlayer);
        //this.AddCardPlayer();
    }
    //Add Card Player
    //private void AddCardPlayer() //Only 1 Guild
    //{
    //    CardPlayer[] cardPlayers = Resources.LoadAll<CardPlayer>(resPathCardPlayer);

    //    foreach(CardPlayer cardPlayer in cardPlayers)
    //    {
    //        if(!HandleCardPlayer(cardPlayer))
    //        {
    //            continue;
    //        }
    //        cardCharacters.Insert(1, cardPlayer);
    //    }
    //}
    //private bool HandleCardPlayer(CardPlayer cardPlayer)
    //{
    //    if(PlayerManager.Instance.CardCurrentPlayer == null)
    //    {
    //        Debug.LogError("CardCurrentPlayer PlayerManager == null!");
    //    }
    //    if(cardPlayer == PlayerManager.Instance.CardCurrentPlayer)
    //    {
    //        return false;
    //    }
    //    return true;
    //}
}


