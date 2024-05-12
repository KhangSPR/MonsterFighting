using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Experimental.Rendering.Universal.RenderObjects;

namespace UIGameDataManager
{
    public class CardFilterSettings
    {
        //Tower
        public AttackType selectedAttackType;
        public Rarity selectedRareTower;
        public CharacterClass selectedCharacterClass;

        public int selectedStar = 0; // Default to All Stars

        ////Machine
        //public CardMachine.CardClassMachine selectedClassMachine = CardMachine.CardClassMachine.ALL;
        //public CardMachine.CardRare selectedRareMachine = CardMachine.CardRare.ALL;
    }

    public class CardUIPanelManager : SaiMonoBehaviour
    {
        //Action 

        [Header("Panel Card")]

        [SerializeField] Transform PanelCard;
        [SerializeField] GameObject cardPrefabTower; // Đối tượng Prefab của card (nên thiết kế trước trong Unity và kéo vào đây)
        [SerializeField] GameObject cardPrefabMachine;

        [Header("Card Status")]
        [SerializeField] Transform GameHouse;
        [SerializeField] GameObject CardStatsPrefab;

        [Header("UI Controllers")]
        [SerializeField] CharScreenController charScreen;

        private static CardUIPanelManager instance;
        public static CardUIPanelManager Instance => instance;

        private CardFilterSettings filterSettings = new CardFilterSettings();

        CardALLCard cardManagerALL;
        //Dictionnary Tower

        private Dictionary<int, AttackType> AttacktypesTower = new Dictionary<int, AttackType>()
{
    { 0, AttackType.ALL },
    { 1, AttackType.Melee },
    { 2, AttackType.Ranged },
    { 3, AttackType.Magic }
    // Thêm các ánh xạ cho các giá trị khác nếu cần
};

        private Dictionary<int, CharacterClass> CharacterClasssTower = new Dictionary<int, CharacterClass>()
{
    { 0, CharacterClass.ALL },
    { 1, CharacterClass.Warrior },
    { 2, CharacterClass.Archer },
    { 3, CharacterClass.Witch },
    { 4, CharacterClass.Mage },
    { 5, CharacterClass.Paladin }
    // Thêm các ánh xạ cho các giá trị khác nếu cần
};
        private Dictionary<int, Rarity> classRaresTower = new Dictionary<int, Rarity>()
{
    { 0, Rarity.ALL },
    { 1, Rarity.Common },
    { 2, Rarity.Rare },
    { 3, Rarity.Epic },
    { 4, Rarity.Legendary }
    // Thêm các ánh xạ cho các giá trị khác nếu cần
};

        private Dictionary<int, int> starValues = new Dictionary<int, int>()
    {
    { 0, 0}, // All Stars
    { 1, 1 },
    { 2, 2},
    { 3, 3 },
    { 4, 4 },
    { 5, 5 },
    { 6, 6 }
    };

        //Dictionnary Machine

    //    private Dictionary<int, CardMachine.CardClassMachine> classMachines = new Dictionary<int, CardMachine.CardClassMachine>()
    //{
    //    { 0, CardMachine.CardClassMachine.ALL },
    //    { 1, CardMachine.CardClassMachine.Machine },
    //    { 2, CardMachine.CardClassMachine.TNT },
    //    { 3, CardMachine.CardClassMachine.Cannon }
    //    // Thêm các ánh xạ cho các giá trị khác nếu cần
    //};
    //    private Dictionary<int, CardMachine.CardRare> classRaresMachine = new Dictionary<int, CardMachine.CardRare>()
    //{
    //    { 0, CardMachine.CardRare.ALL },
    //    { 1, CardMachine.CardRare.Common },
    //    { 2, CardMachine.CardRare.Rare },
    //    { 3, CardMachine.CardRare.Epic },
    //    { 4, CardMachine.CardRare.Legendary }
    //    // Thêm các ánh xạ cho các giá trị khác nếu cần
    //};




        protected override void Awake()
        {
            if (CardUIPanelManager.instance != null)
            {
                Debug.LogError("Only 1 CardManager Warning");
            }
            CardUIPanelManager.instance = this;
        }
        protected override void Start()
        {
            base.Start();
            cardManagerALL = CardManager.Instance.CardALLCard;
            OnTowerButtonClickedTower(0);

        }
        private void OnApplicationQuit()
        {
            // Reset the CardManager when the game is about to quit
            //ClearCardList();
            ClearCardALLList();
        }
        private void ClearCardALLList()
        {
            cardManagerALL.CardsTower.Clear();
            cardManagerALL.CardsMachine.Clear();
        }

        public void OnTowerButtonClickedTower(int index)
        {
            List<CardCharacter> towerCards = cardManagerALL.GetTowerCards();
            // Hiển thị card trên PanelCard
            DisplayCardsOnPanelTower(towerCards);

            filterSettings.selectedAttackType = AttackType.ALL;
            filterSettings.selectedRareTower = Rarity.ALL;
            filterSettings.selectedCharacterClass = CharacterClass.ALL;
            filterSettings.selectedStar = 0;

        }
        public void OnTowerButtonClickedMachine(int index)
        {
            List<CardMachine> cardMachines = cardManagerALL.GetMachineCards();

            // Hiển thị card trên PanelCard
            DisplayCardsOnPanelMachine(cardMachines);

        }


        //On click Card Tower
        public void OnTowerButtonClickbyAttackType(int objLoading, int numberType)
        {
            if (AttacktypesTower.TryGetValue(numberType, out var selectedType))
            {
                filterSettings.selectedAttackType = selectedType;
                UpdateFilteredCards(objLoading);
            }
        }
        public void OnTowerButtonClickbyCharacterClass(int objLoading, int numberClass)
        {
            if (CharacterClasssTower.TryGetValue(numberClass, out var selectedClass))
            {
                filterSettings.selectedCharacterClass = selectedClass;
                UpdateFilteredCards(objLoading);
            }
        }

        public void OnTowerButtonClickbyRare(int objLoading, int numberRare)
        {
            if (classRaresTower.TryGetValue(numberRare, out var selectedRare))
            {
                filterSettings.selectedRareTower = selectedRare;
                UpdateFilteredCards(objLoading);
            }
        }
        public void OnTowerButtonClickedStar(int objLoading, int numberStar)
        {
            if (starValues.TryGetValue(numberStar, out int selectedStar))
            {
                filterSettings.selectedStar = selectedStar;
                UpdateFilteredCards(objLoading);
            }
        }
        //On click Card Machine

        public void OnMachineButtonClickbyClass(int objLoading, int numberClass)
        {
            //if (classMachines.TryGetValue(numberClass, out var selectedClass))
            //{
            //    filterSettings.selectedClassMachine = selectedClass;
            //    UpdateFilteredCards(objLoading);
            //}
        }
        public void OnMachineButtonClickbyRare(int objLoading, int numberRare)
        {
            //if (classRaresMachine.TryGetValue(numberRare, out var selectedRare))
            //{
            //    filterSettings.selectedRareMachine = selectedRare;
            //    UpdateFilteredCards(objLoading);
            //}
        }
        private void UpdateFilteredCards(int objLoading)
        {

            switch (objLoading)
            {
                case 0:
                    List<CardCharacter> towerCards = GetFilteredCardsTower();
                    DisplayCardsOnPanelTower(towerCards);
                    break;

                //Thêm các trường hợp khác nếu cần
                case 1:
                    List<CardMachine> machineCards = GetFilteredCardsMachine();
                    DisplayCardsOnPanelMachine(machineCards);
                    break;
                default: return;
            }

        }

        private List<CardCharacter> GetFilteredCardsTower()
        {
            List<CardCharacter> result = new List<CardCharacter>();

            foreach (CardCharacter card in cardManagerALL.GetTowerCards())
            {
                if (
                    (filterSettings.selectedCharacterClass == 0 || card.GetCharacterClass() == filterSettings.selectedCharacterClass) &&
                    (filterSettings.selectedAttackType == 0 || card.GetAttackType() == filterSettings.selectedAttackType) &&
                    (filterSettings.selectedRareTower == 0 || card.GetRarity() == filterSettings.selectedRareTower) &&
                    (filterSettings.selectedStar == 0 || card.Star + 1 == filterSettings.selectedStar)
                    )
                {
                    result.Add(card);
                }
            }

            return result;
        }

        private List<CardMachine> GetFilteredCardsMachine()
        {
            List<CardMachine> result = new List<CardMachine>();

            //foreach (CardMachine card in cardManagerALL.CardsMachine)
            //{
            //    if ((filterSettings.selectedClassMachine == CardMachine.CardClassMachine.ALL || card.cardClassMachine == filterSettings.selectedClassMachine) &&
            //        (filterSettings.selectedRareMachine == CardMachine.CardRare.ALL || card.carRare == filterSettings.selectedRareMachine))
            //    {
            //        result.Add(card);
            //    }
            //}

            return result;
        }
        public int CardCount = 0;
        private void DisplayCardsOnPanelTower(List<CardCharacter> cards)
        {

            // Xóa tất cả các đối tượng con của PanelCard trước khi thêm mới
            foreach (Transform child in PanelCard)
            {
                CardCount = 0;

                charScreen.M_Characters.Clear();

                Destroy(child.gameObject);

            }

            // Tạo và hiển thị một đối tượng UI (GameObject) cho mỗi CardTower
            foreach (CardCharacter card in cards)
            {

                GameObject cardObject = Instantiate(cardPrefabTower, PanelCard);

                // Gọi hàm để cấu hình thông tin card trên đối tượng UI
                CardUITower cardUI = cardObject.transform.GetComponent<CardUITower>();

                CharacterData TowerData = cardObject.transform.GetComponent<CharacterData>();

                // Set Index based on the loop variable

                // Add List CharScreen
                charScreen.M_Characters.Add(TowerData);


                if (TowerData != null)
                {
                    TowerData.SetCharacterBaseData(card);
                }

                if (cardUI != null)
                {
                    cardUI.SetCardInfo(cardObject, card);
                    cardUI.idCard = CardCount;
                }


                CardCount++;
            }

            charScreen.ClearAndShowPreviews();
        }

        private void DisplayCardsOnPanelMachine(List<CardMachine> cards)
        {
            // Xóa tất cả các đối tượng con của PanelCard trước khi thêm mới
            foreach (Transform child in PanelCard)
            {
                Destroy(child.gameObject);
            }

            // Tạo và hiển thị một đối tượng UI (GameObject) cho mỗi CardTower
            foreach (CardMachine card in cards)
            {

                GameObject cardObject = Instantiate(cardPrefabMachine, PanelCard);
                // Gọi hàm để cấu hình thông tin card trên đối tượng UI
                CardUIMachine cardUI = cardObject.transform.GetComponent<CardUIMachine>();

                if (cardUI != null)
                {
                    cardUI.SetCardInfo(cardObject, card);
                }

                Debug.Log(card.nameCard);
            }
        }

        //Card UI Status

        public void CheckCardTypeAndProcess(int id, MonoBehaviour card)
        {
            if (id < 0 || card == null)
                return;

            if (card is CardUITower)
            {
                charScreen.SetCurrentIndex(id);

                InstanceCardObject();

            }
            if (card is CardUIMachine)
            {
                Debug.Log("Card UI Machine");
            }
        }

        private void InstanceCardObject()
        {
            GameObject cardObject = Instantiate(CardStatsPrefab, GameHouse);
            // Thực hiện các hành động cụ thể khi instance CardUITower

        }

        //private void InstanceCardObject(CardMachine card)
        //{
        //    GameObject cardObject = Instantiate(CardStatusPrefab, GameHouse);
        //    // Thực hiện các hành động cụ thể khi instance CardUIMachine
        //}


    }

}