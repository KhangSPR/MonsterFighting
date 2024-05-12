using System.Collections.Generic;
using System.Linq;
using UIGameDataManager;
using UnityEngine;

[CreateAssetMenu(fileName = "CardManagerALLCard", menuName = "Custom/CardManagerALLCard", order = 1)]
public class CardALLCard : ScriptableObject
{
    public List<CardCharacter> CardsTower = new List<CardCharacter>();
    public List<CardMachine> CardsMachine = new List<CardMachine>();

    private void OnEnable()
    {
        // Khi ScriptableObject được kích hoạt hoặc tải lên, gọi hàm để load toàn bộ Card ScriptableObject
        LoadAllCardTowerScriptableObjects();
        LoadAllCardMachineScriptableObjects();
        LoadDataCard();
    }
    public void LoadAllCardTowerScriptableObjects()
    {
        // Đường dẫn của thư mục chứa các Card ScriptableObject
        string resPath = "Card/CardSAOJ/CardCharacter";

        // Load tất cả các Card ScriptableObject từ thư mục
        CardCharacter[] loadedCards = Resources.LoadAll<CardCharacter>(resPath);


        // Gán danh sách các Card đã load vào biến allCards
        CardsTower = new List<CardCharacter>(loadedCards);

        // Hiển thị thông báo log (có thể loại bỏ sau khi kiểm tra)
        Debug.Log("Loaded " + CardsTower.Count + " Card ScriptableObjects from " + resPath);


    }
    public void LoadAllCardMachineScriptableObjects()
    {
        // Đường dẫn của thư mục chứa các Card ScriptableObject
        string resPath = "Card/CardSAOJ/CardMachine";

        // Load tất cả các Card ScriptableObject từ thư mục
        CardMachine[] loadedCards = Resources.LoadAll<CardMachine>(resPath);

        // Gán danh sách các Card đã load vào biến allCards
        CardsMachine = new List<CardMachine>(loadedCards);

        // Hiển thị thông báo log (có thể loại bỏ sau khi kiểm tra)
        Debug.Log("Loaded " + CardsTower.Count + " Card ScriptableObjects from " + resPath);


    }
    public void LoadDataCard()
    {
        CardsTower = SortCardCharacterByCost(CardsTower);
        CardsMachine = SortCardMachineByCost(CardsMachine);
    }
    List<CardCharacter> SortCardCharacterByCost(List<CardCharacter> originalList)
    {
        return originalList.OrderBy(x => x.price).ToList();
    }
    List<CardMachine> SortCardMachineByCost(List<CardMachine> originalList)
    {
        return originalList.OrderBy(x => x.price).ToList();
    }
    //Card Guard
    //List<CardCharacter> SortCardGuardByCost(List<Ca> originalList)
    //{
    //    return originalList.OrderBy(x => x.price).ToList();
    //}
    public List<CardCharacter> GetTowerCards()
    {
        return CardsTower;
    }
    public List<CardMachine> GetMachineCards()
    {
        return CardsMachine;
    }
}


