using UnityEngine;
using System;
using UnityEditor;
using TMPro;
using UIGameDataMap;

namespace UIGameDataManager
{
    [RequireComponent(typeof(SaveManager))]
    public class GameDataManager : MonoBehaviour
    {
        public static event Action<GameData> PotionsUpdated;
        public static event Action<GameData> GemsUpdated;
        public static event Action<GameData> FundsUpdated;
        public static event Action<GameData> StonesUpdated;

        public static event Action<bool> CharacterLeveledUp;
        public static event Action<bool> CharacterGemedUp;
        public static event Action<bool> LevelUpButtonEnabled;
        public static event Action<bool> StarUpButtonEnabled;

        [SerializeField] GameData m_GameData;
        public GameData GameData { set => m_GameData = value; get => m_GameData; }

        SaveManager m_SaveManager;

        private static GameDataManager instance;
        public static GameDataManager Instance => instance;
        public MapSO currentMapSO;
        public bool FirstTimeGetFullStars;
        void Awake()
        {
            if (GameDataManager.instance != null) Debug.LogError("Onlly 1 GameDataManager Warning");
            GameDataManager.instance = this;

            m_SaveManager = GetComponent<SaveManager>();

        }
        #region Test Reset Funds
        /*        bool m_IsGameDataInitialized;*/ //Not

        //void OnEnable()
        //{

        //    SettingsScreen.ResetPlayerFunds += OnResetFunds;
        //}

        //void OnDisable()
        //{ 

        //    SettingsScreen.ResetPlayerFunds -= OnResetFunds;
        //}
        //void OnResetFunds()
        //{
        //    m_GameData.gold = 0;
        //    m_GameData.gems = 0;
        //    m_GameData.lvUpGame = 0;
        //    //m_GameData.levelUpPotions = 0;
        //    UpdateFunds();
        //    UpdatePotions();
        //}
        #endregion
        void OnEnable()
        {
            //UpdateEnemyGemsUI();
            GameManager.UpdateResources += UpdateResources;

            CharScreenController.LevelPotionUsed += OnLevelPotionUsed;
            CharScreenController.StarGemUsed += OnStarGemUsed;


            StatusScreen.updateStat += UpdatePotions;
            StatusScreen.updateStatMaxlv += UpdateGems;

            ShopController.ShopItemPurchasing += OnPurchaseItem;
            ShopController.ShopItemCardPurchasing += OnPurchaseItemCard;


            CharScreenController.CharacterShown += OnCharacterShown;
            CharScreenController.CharacterShown += OnCharacterShowStar;
        }

        void OnDisable()
        {
            GameManager.UpdateResources -= UpdateResources;

            CharScreenController.LevelPotionUsed -= OnLevelPotionUsed;
            CharScreenController.StarGemUsed -= OnStarGemUsed;

            StatusScreen.updateStat -= UpdatePotions;
            StatusScreen.updateStatMaxlv -= UpdateGems;

            ShopController.ShopItemPurchasing -= OnPurchaseItem;
            ShopController.ShopItemCardPurchasing += OnPurchaseItemCard;

            CharScreenController.CharacterShown -= OnCharacterShown;
            CharScreenController.CharacterShown -= OnCharacterShowStar;
        }

        void Start()
        {
            m_SaveManager?.LoadGame();

            RemoveItem();

            AddItem(); //Call Before UpdateFunds

            UpdateFunds();


        }
        void RemoveItem()
        {
            m_GameData.xpLv1 = 0;
            m_GameData.enemyBoss = 0;
            m_GameData.gold = 0;
            m_GameData.enemyStone = 0;
        }
        [Header("Test Add Item")]
        public uint ItemADD;
        void AddItem()
        {
            m_GameData.enemyStone += 10;
            m_GameData.enemyBoss += ItemADD;
            m_GameData.gold += 500;

        }
        /// <summary>
        /// 
        /// </summary>
        // transaction methods 
        
        void UpdateFunds()
        {
            if (m_GameData != null)
            {
                FundsUpdated?.Invoke(m_GameData);
                Debug.Log("Update Funds");
            }
        }
        void UpdateResources()
        {
            if (m_GameData != null)
            {
                StonesUpdated?.Invoke(m_GameData);
                Debug.Log("Update Funds");
            }
        }
        void UpdateGems()
        {
            if (m_GameData != null)
            {
                GemsUpdated?.Invoke(m_GameData);

                //Update OptionBar
                UpdateFunds();
            }

            Debug.Log("GameData: " + m_GameData.enemyBoss);
        }
        void UpdatePotions()
        {
            if (m_GameData != null)
                PotionsUpdated?.Invoke(m_GameData);
            Debug.Log("GameData: " + m_GameData.xpLv1);
        }

        // do we have enough potions to level up?
        public bool CanLevelUp(CharacterData character)
        {
            if (m_GameData == null || character == null)
                return false;

            return (character.GetXPForNextLevel() <= m_GameData.xpLv1);
        }
        public bool CanStarUp(CharacterData character)
        {
            Debug.Log("Can lv up");

            if (m_GameData == null || character == null)
                return false;

            return (character.GetXPForNextStar() <= m_GameData.enemyBoss);
        }

        void PayLevelUpPotions(uint numberPotions)
        {
            if (m_GameData != null)
            {
                m_GameData.xpLv1 -= numberPotions;
                PotionsUpdated?.Invoke(m_GameData);
            }
        }
        void PayStarUpGems(uint numberGems)
        {
            if (m_GameData != null)
            {
                m_GameData.enemyBoss -= numberGems;
                GemsUpdated?.Invoke(m_GameData);
            }
        }
        // attempt to level up the character using a potion
        void OnLevelPotionUsed(CharacterData charData)
        {
            if (charData == null)
                return;

            bool isLeveled = false;
            if (CanLevelUp(charData))
            {
                PayLevelUpPotions(charData.GetXPForNextLevel());
                isLeveled = true;
                //AudioManager.PlayVictorySound();
            }
            else
            {
                //AudioManager.PlayDefaultWarningSound();
                Debug.Log("Khong du binh EXP");
            }
            // notify other objects if level up succeeded or failed
            CharacterLeveledUp?.Invoke(isLeveled);
        }
        // attempt to level up the character using a Gems
        void OnStarGemUsed(CharacterData charData)
        {
            if (charData == null)
                return;

            bool isLeveled = false;
            if (CanStarUp(charData))
            {
                PayStarUpGems(charData.GetXPForNextStar());
                isLeveled = true;
                //AudioManager.PlayVictorySound();

                Debug.Log("OnStarGemUsed : " + charData.GetXPForNextStar());
            }
            else
            {
                //AudioManager.PlayDefaultWarningSound();
                Debug.Log("Khong du Gems  ");
            }
            // notify other objects if level up succeeded or failed
            CharacterGemedUp?.Invoke(isLeveled);
        }
        //possibly indicating whether the level-up button should be enabled or disabled.
        void OnCharacterShown(CharacterData charData)
        {

            LevelUpButtonEnabled?.Invoke(CanLevelUp(charData));

        }
        void OnCharacterShowStar(CharacterData charData)
        {

            StarUpButtonEnabled?.Invoke(CanStarUp(charData));



        }
        bool HasSufficientFunds(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return false;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    return m_GameData.gold >= discountedPrice;

                case CurrencyType.EnemyStone:
                    return m_GameData.enemyStone >= discountedPrice;

                case CurrencyType.USD:
                    return true;

                default:
                    return false;
            }
        }
        void PayTransaction(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    m_GameData.gold -= (uint)discountedPrice;
                    break;

                case CurrencyType.EnemyStone:
                    m_GameData.enemyStone -= (uint)discountedPrice;
                    break;

                // non-monetized placeholder - invoke in-app purchase logic/interface for a real application
                case CurrencyType.USD:
                    break;
            }
        }
        bool HasSufficientFunds(ShopItemCardSO shopItem)
        {
            if (shopItem == null)
                return false;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    return m_GameData.gold >= discountedPrice;

                case CurrencyType.EnemyStone:
                    return m_GameData.enemyStone >= discountedPrice;

                case CurrencyType.USD:
                    return true;

                default:
                    return false;
            }
        }
        void PayTransaction(ShopItemCardSO shopItem)
        {
            if (shopItem == null)
                return;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    m_GameData.gold -= (uint)discountedPrice;
                    break;

                case CurrencyType.EnemyStone:
                    m_GameData.enemyStone -= (uint)discountedPrice;
                    break;

                // non-monetized placeholder - invoke in-app purchase logic/interface for a real application
                case CurrencyType.USD:
                    break;
            }
        }

        void ReceivePurchasedGoods(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return;

            ShopItemType contentType = shopItem.contentType;
            uint contentValue = shopItem.contentValue;

            ReceiveContent(contentType, contentValue);
        }
        void ReceivePurchasedGoods(ShopItemCardSO shopItem)
        {
            if (shopItem == null)
                return;

            ShopItemType contentType = shopItem.contentType;

            ReceiveContent(contentType);
        }
        // for gifts or purchases
        void ReceiveContent(ShopItemType contentType)
        {
            switch (contentType)
            {
                case ShopItemType.CardCharacter:
                    UpdateFunds();
                    break;
                case ShopItemType.CardMachine:
                    UpdateFunds();
                    break;
                case ShopItemType.CardGuard:
                    UpdateFunds();
                    break;
            }
        }
        // for gifts or purchases
        void ReceiveContent(ShopItemType contentType, uint contentValue)
        {
            switch (contentType)
            {
                case ShopItemType.Gold:
                    m_GameData.gold += contentValue;
                    UpdateFunds();
                    break;

                case ShopItemType.XpLv1:
                    m_GameData.xpLv1 += contentValue;
                    UpdateFunds();
                    break;

                case ShopItemType.XpLv2:
                    m_GameData.xpLv2 += contentValue;
                    UpdatePotions();
                    UpdateFunds();
                    break;

                case ShopItemType.XpLv3:
                    m_GameData.xpLv3 += contentValue;
                    UpdatePotions();
                    UpdateFunds();
                    break;
                case ShopItemType.XpLv4:
                    m_GameData.xpLv4 += contentValue;
                    UpdatePotions();
                    UpdateFunds();
                    break;
            }
        }
        // event-handling methods

        // buying item from ShopScreen, pass button screen position 
        void OnPurchaseItem(ShopItemSO shopItem, Vector2 screenPos)
        {
            if (shopItem == null)
                return;

            // invoke transaction succeeded or failed
            if (HasSufficientFunds(shopItem))
            {
                PayTransaction(shopItem);
                ReceivePurchasedGoods(shopItem);

                Debug.Log("OnPurchaseItem");
                //TransactionProcessed?.Invoke(shopItem, screenPos);

                //AudioManager.PlayDefaultTransactionSound();
            }
            else
            {
                // notify listeners (PopUpText, sound, etc.)
                //TransactionFailed?.Invoke(shopItem);
                //AudioManager.PlayDefaultWarningSound();
            }
        }
        void OnPurchaseItemCard(ShopItemCardComponent shopItemComponent, ShopItemCardSO shopItem, Vector2 screenPos)
        {
            if (shopItem == null)
                return;

            // invoke transaction succeeded or failed
            if (HasSufficientFunds(shopItem))
            {
                PayTransaction(shopItem);
                ReceivePurchasedGoods(shopItem);
                //Save has Buy
                shopItem.hasBuy = true;
                SaveShopItemState(shopItem);
                ////HasBuy
                shopItemComponent.DisableObjectContainingShopItemCardSO();
                AddCardComponentToFolder(shopItem);

                //CardManager Sort
                //CardManager.Instance.ClearCardALLList();
                CardManager.Instance.CardALLCard.LoadAllCardTowerScriptableObjects();
                CardManager.Instance.CardALLCard.LoadDataCard();

                Debug.Log("OnPurchaseItem");
                //TransactionProcessed?.Invoke(shopItem, screenPos);

                //AudioManager.PlayDefaultTransactionSound();
            }
            else
            {
                // notify listeners (PopUpText, sound, etc.)
                //TransactionFailed?.Invoke(shopItem);
                //AudioManager.PlayDefaultWarningSound();
            }
        }
        void SaveShopItemState(ShopItemCardSO shopItem)
        {
            // Lưu trạng thái của shopItem vào ScriptableObject
            UnityEditor.EditorUtility.SetDirty(shopItem);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        void AddCardComponentToFolder(ShopItemCardSO shopItemCardSO)
        {
            // Get the type of card
            string cardType = shopItemCardSO.GetShopItemType().ToString();

            string newFolderPath = "Assets/Resources/Card/CardSAOJ/" + cardType;

            // Create ScriptableObject

            // Create link
            string newAssetPath = newFolderPath + "/" + shopItemCardSO.cardComponent.nameCard + ".asset";

            // Move the new ScriptableObject into the new folder
            AssetDatabase.CreateAsset(CreateInstanceCardComponent(cardType, shopItemCardSO), newAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        CardComponent CreateInstanceCardComponent(string cardType, ShopItemCardSO shopItemCardSO)
        {
            switch (cardType)
            {
                case "CardCharacter":
                    CardCharacter characterCard = shopItemCardSO.cardComponent as CardCharacter;
                    // Create a new instance of CardCharacter and transfer all values from the existing characterCard
                    characterCard = new CardCharacter(
                        shopItemCardSO.cardComponent.nameCard,
                        shopItemCardSO.cardComponent.cardRefresh,
                        shopItemCardSO.cardComponent.price,
                        shopItemCardSO.cardComponent.frame,
                        shopItemCardSO.cardComponent.background,
                        shopItemCardSO.cardComponent.avatar,
                        characterCard.Level,
                        characterCard.Star,
                        characterCard.basePointsAttack,
                        characterCard.basePointsLife,
                        characterCard.basePointsAttackSpeed,
                        characterCard.basePointsSpecialAttack,
                        characterCard.bioTitle,
                        characterCard.bio,
                        characterCard.skill1,
                        characterCard.skill2,
                        characterCard.skill3,
                        characterCard.cardRare,
                        characterCard.cardAttack,
                        characterCard.cardClass,
                        //characterCard.characterVisualsPrefab,
                        characterCard.cardStat
                    );
                    return characterCard;
                case "CardMachine":
                    return ScriptableObject.CreateInstance<CardMachine>();
                case "CardGuard":
                    return null;
                default:
                    return new CardComponent(shopItemCardSO.cardComponent.nameCard, shopItemCardSO.cardComponent.cardRefresh, shopItemCardSO.cardComponent.price,
                                              shopItemCardSO.cardComponent.frame, shopItemCardSO.cardComponent.background, shopItemCardSO.cardComponent.avatar);;
            }
        }



    }
}
