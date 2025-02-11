using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIGameDataManager
{
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


        CardALLCard cardManagerALL;

        private int CardCount = 0;

        private Dictionary<int, AttackCategory> AttacktypesTower = new Dictionary<int, AttackCategory>()
        {
            { 0, AttackCategory.ALL },
            { 1, AttackCategory.Tank},
            { 2, AttackCategory.Warrior },
            { 3, AttackCategory.Archer },            
            { 4, AttackCategory.Wizard},


        };


        [Header("Game ICON SO")]
        [SerializeField] GameIconsSO m_GameIconsData;

        private List<CardUITower> m_CardUITowerList = new List<CardUITower>();
        public List<CardUITower> CardUITowerList;
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
        public void OnTowerButtonClickedTower(int index)
        {
            List<CardCharacter> cardCharacters = cardManagerALL.CardCharacters;
            // View card trên PanelCard
            DisplayCardsOnPanelTower(cardCharacters);


        }
        //On click Card Tower
        public void OnTowerButtonClickbyAttackType(int objLoading, int numberType)
        {
            if (AttacktypesTower.TryGetValue(numberType, out var selectedType))
            {
                UpdateCards(objLoading, selectedType);
            }
        }
        private void UpdateCards(int objLoading, AttackCategory attackType)
        {
            switch (objLoading)
            {
                case 0:
                    List<CardCharacter> towerCards = cardManagerALL.GetCharacterAttackType(attackType);
                    DisplayCardsOnPanelTower(towerCards);
                    break;
            }

        }
        private void DisplayCardsOnPanelTower(List<CardCharacter> cards)
        {
            if (charScreen == null) return;
            if (charScreen.M_Characters == null) return;

            m_CardUITowerList.Clear();
            // Xóa tất cả các đối tượng con của PanelCard trước khi thêm mới
            foreach (Transform child in PanelCard)
            {
                CardCount = 0;

                charScreen.M_Characters.Clear();//

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
                m_CardUITowerList.Add(cardUI);

                if (TowerData != null)
                {
                    TowerData.SetCharacterBaseData(card);
                }

                if (cardUI != null)
                {
                    cardUI.SetCardInfo(card);
                    cardUI.idCard = CardCount;
                }


                CardCount++;

            }

            charScreen.ClearAndShowPreviews();
        }

        //Card UI Status
        public void CheckCardTypeAndProcess(int id, MonoBehaviour card)
        {
            if (id < 0 || card == null)
                return;

            if (card is CardUITower)
            {
                charScreen.SetCurrentIndex(id);

                CardStatsPrefab.SetActive(true);

                //InstanceCardObject();

            }
            if (card is CardUIMachine)
            {
                Debug.Log("Card UI Machine");
            }
        }
        public void UpdateRarityCardPlayer(CardPlayer cardPlayer)
        {
            foreach(CardUITower card in m_CardUITowerList)
            {
                CharacterData characterData = card.transform.GetComponent<CharacterData>();
                if(characterData != null)
                {
                    if(characterData.CharacterBaseData is CardPlayer)
                    {
                        card.SetCardInfo(cardPlayer);
                        return;
                    }
                }
            }
        }
        //private void InstanceCardObject()
        //{
        //    // Lấy RectTransform của GameHouse
        //    RectTransform gameHouseRect = GameHouse.GetComponent<RectTransform>();

        //    // Tính toán tọa độ trung tâm của GameHouse
        //    Vector3 centerPosition = gameHouseRect.position;

        //    // Khởi tạo đối tượng CardStatsPrefab tại tọa độ trung tâm của GameHouse
        //    GameObject cardObject = Instantiate(CardStatsPrefab, centerPosition, Quaternion.identity, GameHouse);
        //}
    }

}