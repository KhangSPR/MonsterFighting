using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UIGameDataManager;

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

    private const string CharacterKey = "SavedCardCharacters";
    private const string MachineKey = "SavedCardMachines";
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
    public void SaveData()
    {
        var characterIds = cardCharacter.Select(c => c.ID).ToList();
        //var machineIds = cardMachines.Select(m => m.ID).ToList();

        // Serialize ID list to JSON
        string characterJson = JsonUtility.ToJson(new IDListWrapper { IDs = characterIds });
        //string machineJson = JsonUtility.ToJson(new IDListWrapper { IDs = machineIds });

        //Save JSON to PlayerPrefs
        PlayerPrefs.SetString(CharacterKey, characterJson);
        //PlayerPrefs.SetString(MachineKey, machineJson);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        // Tải JSON từ PlayerPrefs
        string characterJson = PlayerPrefs.GetString(CharacterKey, "{}");
        //string machineJson = PlayerPrefs.GetString(MachineKey, "{}");

        // Deserialize JSON thành danh sách ID
        var savedCharacterIds = JsonUtility.FromJson<IDListWrapper>(characterJson)?.IDs ?? new List<string>();
        //var savedMachineIds = JsonUtility.FromJson<IDListWrapper>(machineJson)?.IDs ?? new List<string>();

        // Lọc danh sách các card theo ID đã lưu
        cardCharacter = cardALL.CardCharacters
            .Where(c => savedCharacterIds.Contains(c.ID))
            .ToList();
        //cardMachines = cardALL.CardMachines
        //    .Where(m => savedMachineIds.Contains(m.ID))
        //    .ToList();
    }
    public void LoadDataCardPlayer()
    {
        cardCharacter.Insert(0, PlayerManager.Instance.CardCurrentPlayer);
    }

}

[System.Serializable]
public class IDListWrapper
{
    public List<string> IDs;
}
