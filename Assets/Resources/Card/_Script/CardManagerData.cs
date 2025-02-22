using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UIGameDataManager;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "CardManagerData", menuName = "Custom/CardManagerData", order = 1)]
public class CardManagerData : ScriptableObject
{
    [SerializeField] CardALLCard cardALL;
    // Lists of saved cards
    [SerializeField]
    List<CardCharacter> cardCharacter = new List<CardCharacter>();
    public List<CardCharacter> CardCharacters => cardCharacter;
    List<CardMachine> cardMachines = new List<CardMachine>();
    public List<CardMachine> CardMachines => cardMachines;

    private const string CARD_CHARACTER_KEY = "SavedCardCharacters";
    public void RemoveAllJoinedGuild()
    {
        ClearButKeepFirst();
        cardMachines.Clear();
    }
    public void ClearButKeepFirst()
    {
        if (cardCharacter.Count > 1)
        {
            cardCharacter.RemoveRange(1, cardCharacter.Count - 1);
        }
    }
    public void SaveCardData()
    {
        List<string> cardIDs = cardCharacter.Select(c => c.ID).ToList();
        string json = JsonConvert.SerializeObject(cardIDs);
        PlayerPrefs.SetString("CARD_CHARACTER_KEY", json);
        PlayerPrefs.Save();
        Debug.Log("Saved cardCharacter IDs: " + json);
    }
    public void LoadCardData()
    {
        if (PlayerPrefs.HasKey("CARD_CHARACTER_KEY"))
        {
            string json = PlayerPrefs.GetString("CARD_CHARACTER_KEY");
            List<string> savedIDs = JsonConvert.DeserializeObject<List<string>>(json);

            // Nếu bạn muốn sử dụng danh sách ID này để lọc cardCharacter hiện tại:
            cardCharacter = cardALL.CardCharacters
                .Where(c => savedIDs.Contains(c.ID))
                .ToList();

            Debug.Log("Loaded cardCharacter IDs: " + json);
        }
    }


    public void LoadDataCardPlayer()
    {
        cardCharacter.Insert(0, PlayerManager.Instance.CardCurrentPlayer);
    }

}