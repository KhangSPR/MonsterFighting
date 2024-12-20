using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

namespace UIGameDataManager
{
    [RequireComponent(typeof(SaveManager))]
    public class GameDataManager : MonoBehaviour
    {
        public static event Action<GameData> FundsUpdated;
        public static event Action<GameData> StonesUpdated;



        [SerializeField] GameData m_GameData;
        public GameData GameData { set => m_GameData = value; get => m_GameData; }

        SaveManager m_SaveManager;
        public SaveManager SaveManager => m_SaveManager;

        private static GameDataManager instance;
        public static GameDataManager Instance => instance;

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


            ShopController.ShopItemPurchasing += OnPurchaseItem;
            ShopController.ShopItemCardPurchasing += OnPurchaseItemCard;

        }

        void OnDisable()
        {
            GameManager.UpdateResources -= UpdateResources;

            ShopController.ShopItemPurchasing -= OnPurchaseItem;
            ShopController.ShopItemCardPurchasing += OnPurchaseItemCard;
        }
        void Start()
        {
            m_SaveManager?.LoadGame();

            RemoveItem();

            AddItem(); // Call Before UpdateFunds

            UpdateFunds();
        }
        void RemoveItem()
        {
            m_GameData.StoneBoss = 0;
            m_GameData.badGe = 0;
            m_GameData.StoneEnemy = 0;
            
        }
        [Header("Test Add Item")]
        public uint ItemADD;

        void AddItem()
        {
            m_GameData.StoneEnemy += 99;
            m_GameData.StoneBoss += 5;
            m_GameData.badGe += 35;

            m_SaveManager?.SaveGame();
        }
        /// <summary>
        /// 
        /// </summary>
        // transaction methods 

        public void UpdateFunds()
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
   
        bool HasSufficientFunds(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return false;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    return m_GameData.badGe >= discountedPrice;

                case CurrencyType.EnemyStone:
                    return m_GameData.StoneEnemy >= discountedPrice;

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
                    m_GameData.badGe -= (uint)discountedPrice;
                    break;

                case CurrencyType.EnemyStone:
                    m_GameData.StoneEnemy -= (uint)discountedPrice;
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
                    return m_GameData.badGe >= discountedPrice;

                case CurrencyType.EnemyStone:
                    return m_GameData.StoneEnemy >= discountedPrice;

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
                    m_GameData.badGe -= (uint)discountedPrice;
                    break;

                case CurrencyType.EnemyStone:
                    m_GameData.StoneEnemy -= (uint)discountedPrice;
                    break;

                // non-monetized placeholder - invoke in-app purchase logic/interface for a real application
                case CurrencyType.USD:
                    break;
            }
        }
        public void OnReceiverRewardResources(UIGameDataMap.Resources resources)
        {
            CurrencyType currencyType = resources.item.CurrencyType;

            switch (currencyType)
            {
                case CurrencyType.EnemyStone:
                    m_GameData.StoneEnemy -= (uint)resources.Count;
                    break;

                case CurrencyType.EnemyBoss:
                    m_GameData.StoneBoss -= (uint)resources.Count;
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
                    m_GameData.badGe += contentValue;
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
                //CardManager.Instance.CardALLCard.LoadAllCardTowerScriptableObjects();
                //CardManager.Instance.CardALLCard.LoadDataCard();

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
#if UNITY_EDITOR
            EditorUtility.SetDirty(shopItem);
            AssetDatabase.SaveAssets();
#endif
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
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(CreateInstanceCardComponent(cardType, shopItemCardSO), newAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
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
                        characterCard.CharacterStats,
                        characterCard.bioTitle,
                        characterCard.bio,
                        characterCard.skill1,
                        characterCard.skill2,
                        characterCard.rarityCard,
                        characterCard.attackTypeCard,
                        characterCard.characterVisualsPrefab
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
