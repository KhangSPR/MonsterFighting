using System.Collections.Generic;
using UnityEngine;
using UIGameDataManager;
using System.Linq;

[CreateAssetMenu(fileName = "CardManagerData", menuName = "Custom/CardManagerData", order = 1)]
public class CardManagerData : ScriptableObject
{
    // Lists of cards for each type
    public List<CardCharacter> cardCharacter;
    public List<CardMachine> cardMachines;

    private void Awake()
    {
        LoadData();
    }

    public void SaveData()
    {
        // Lưu danh sách cardCharacter bằng PlayerPrefsExtra
        PlayerPrefsExtra.SetList("CardCharacter", cardCharacter);
    }

    void LoadData()
    {
        // Tải danh sách cardCharacter từ PlayerPrefsExtra
        if (cardCharacter != null && cardCharacter is List<CardCharacter>)
        {
            // Lọc các phần tử không phải là CardCharacter 
            var validCharacters = ((List<CardCharacter>)cardCharacter)
                .OfType<CardCharacter>()
                .ToList();

            // Gán danh sách đã lọc vào cardCharacter
            cardCharacter = validCharacters;

            // Lưu danh sách vào PlayerPrefsExtra
            PlayerPrefsExtra.SetList("CardCharacter", cardCharacter);
        }
    }

}
