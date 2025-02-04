using UnityEngine;
using System;
using Unity.VisualScripting;

namespace UIGameDataManager
{
    [RequireComponent(typeof(SaveManager))]
    public class GameDataManager : MonoBehaviour
    {
        public static event Action<GameData> FundsUpdated;
        public static event Action<GameData> StonesUpdated;

        public static event Action<GameData> ResourcesMapUpdated;
        public static event Action<ShopItemSO, Vector2> TransactionProcessed;
        public static Action OnPopUpSpin;

        public static event Action<int> OnSpin;

        [SerializeField] GameData m_GameData;
        public GameData GameData { set => m_GameData = value; get => m_GameData; }

        SaveManager m_SaveManager;
        public SaveManager SaveManager => m_SaveManager;

        private static GameDataManager instance;
        public static GameDataManager Instance => instance;
        [SerializeField]
        private ItemReward[] _itemReward;
        public ItemReward[] ItemRewards => _itemReward;

        private const string QuestMainFolderPath = "GameData/ItemSO";
        //LOAD Resources
        private ItemReward[] LoadQuestsFromFolder(string folderPath)
        {
            // Load all QuestAbstractSO assets from the folder
            ItemReward[] quests = UnityEngine.Resources.LoadAll<ItemReward>(folderPath);

            return quests;
        }
        public ItemReward GetItemRewardByID(string ID)
        {
            foreach (ItemReward itemReward in _itemReward)
            {
                if (!string.IsNullOrEmpty(itemReward.ID) &&
                     string.Equals(itemReward.ID.Trim(), ID.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return itemReward;
                }
            }
            return null;
        }
        public int GetCountItemRewardById(string ID)
        {
            string _id = ID.Trim();

            switch (_id)
            {
                case "CRYSTALLINEREWARD":
                    return (int)m_GameData.StoneEnemy;
                case "MAGICALCRYSTAL":
                    return (int)m_GameData.StoneBoss;
                default:
                    return 0;
            }
        }
        void Awake()
        {
            if (instance != null) Debug.LogError("Onlly 1 GameDataManager Warning");
            instance = this;

            m_SaveManager = GetComponent<SaveManager>();


            _itemReward = LoadQuestsFromFolder(QuestMainFolderPath);
        }
        void OnEnable()
        {
            //UpdateEnemyGemsUI();
            GameManager.UpdateResources += UpdateResources;


            ShopController.ShopItemPurchasing += OnPurchaseItem;
            Spin.OnSpinData += HasSufficientFundsItemSpin;
        }

        void OnDisable()
        {
            GameManager.UpdateResources -= UpdateResources;

            ShopController.ShopItemPurchasing -= OnPurchaseItem;

            Spin.OnSpinData -= HasSufficientFundsItemSpin;
        }
        void Start()
        {
            currentEnergyAmount = CalculateEnergy();

            m_SaveManager?.LoadGame();

            RemoveItem();

            AddItem(); // Call Before UpdateFunds

            UpdateFunds();

        }
        private void OnApplicationFocus(bool hasFocus) // APly Android
        {
            if (!hasFocus) // Mất tiêu điểm
            {
                PlayerPrefs.SetString(nameof(lastRestoreTime), DateTime.Now.ToString());
                PlayerPrefs.SetInt(nameof(currentEnergyAmount), currentEnergyAmount);
                //PlayerPrefs.Save(); // Đảm bảo lưu ngay lập tức
            }
        }
        #region Energy UI ----------------------------------------------------------
        public event Action OnEnergyChanged; // Sự kiện khi năng lượng thay đổi
        [SerializeField] int currentEnergyAmount;
        public int CurrentEnergyAmount => currentEnergyAmount;
        [SerializeField] int maxEnergyAmount;

        [Tooltip("Restore 1 energy every ... seconds")]
        [SerializeField] float energyRestoreInterval;
        [SerializeField]
        float timeCount;

        DateTime lastRestoreTime;

        private void Update()
        {
            RestoreEnergy();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ConsumeEnergy();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddEnergy();
            }
            //Debug.Log("Time: " + GetEnergyTimer());
        }

        //private void OnApplicationQuit()
        //{
        //    PlayerPrefs.SetString(nameof(lastRestoreTime), DateTime.Now.ToString());
        //    PlayerPrefs.SetInt(nameof(currentEnergyAmount), currentEnergyAmount);
        //}

        public void ConsumeEnergy()
        {
            Debug.Log("ConsumeEnergy: " + currentEnergyAmount);
            currentEnergyAmount -= (currentEnergyAmount > 0) ? 1 : 0;

            if (OnEnergyChanged != null)
            {
                Debug.Log("Invoking OnEnergyChanged");
                OnEnergyChanged.Invoke();
            }
            else
            {
                Debug.LogWarning("No listeners for OnEnergyChanged!");
            }
        }


        public void AddEnergy()
        {
            //Debug.Log("AddEnergy: " + currentEnergyAmount);

            currentEnergyAmount += (currentEnergyAmount < maxEnergyAmount) ? 1 : 0;
            lastRestoreTime = DateTime.Now;
            timeCount = energyRestoreInterval;

            OnEnergyChanged?.Invoke();
        }

        public string GetEnergyTimer()
        {
            int minutes = (int)(timeCount / 60);
            int seconds = (int)(timeCount % 60);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void RestoreEnergy()
        {
            timeCount = (currentEnergyAmount < maxEnergyAmount) ? timeCount - Time.deltaTime : 0;
            if (timeCount <= 0)
            {
                //Debug.Log("RestoreEnergy: " + currentEnergyAmount);

                AddEnergy();
            }
        }

        // Calculate energy from last recovery time
        private int CalculateEnergy()
        {
            int energy = PlayerPrefs.GetInt(nameof(currentEnergyAmount));
            try
            {
                lastRestoreTime = DateTime.Parse(PlayerPrefs.GetString(nameof(lastRestoreTime)));
            }
            catch (Exception)
            {
                lastRestoreTime = DateTime.Now;
                return maxEnergyAmount;
            }
            float lastRestoreInterval = (float)(DateTime.Now - lastRestoreTime).TotalSeconds;
            energy += (int)(lastRestoreInterval / energyRestoreInterval);
            timeCount = energyRestoreInterval - lastRestoreInterval % energyRestoreInterval;
            lastRestoreTime.AddSeconds(energyRestoreInterval);
            return (energy > maxEnergyAmount) ? maxEnergyAmount : energy;
        }
        #endregion
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
            m_GameData.StoneEnemy += 199;
            m_GameData.StoneBoss += 12;
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
        void HasSufficientFundsItemSpin(int count)
        {
            if (count < 0) return;
            if (GameData.StoneBoss < count)
            {
                OnPopUpSpin?.Invoke();
                return; // Không làm gì nếu không đủ tiền
            }
            GameData.StoneBoss -= (uint)count;
            UpdateFunds();
            OnSpin?.Invoke(count);
        }

        public void ResourceMapUpdated()
        {
            if (m_GameData != null)
            {
                ResourcesMapUpdated?.Invoke(m_GameData);
                Debug.Log("Update Resource Map");
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
            if(shopItem.contentValue <=0) return false;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Badge:
                    return m_GameData.badGe >= discountedPrice;

                case CurrencyType.EnemyStone:
                    return m_GameData.StoneEnemy >= discountedPrice;
                case CurrencyType.Ruby:
                    return m_GameData.ruby >= discountedPrice;
                case CurrencyType.USD:
                    return true;
                case CurrencyType.Watch:
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
                case CurrencyType.EnemyStone:
                    m_GameData.StoneEnemy -= (uint)discountedPrice;
                    break;
                case CurrencyType.Ruby:
                    m_GameData.ruby -= (uint)discountedPrice;
                    break;
                case CurrencyType.Watch:
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
                    m_GameData.StoneEnemy += (uint)resources.Count;
                    break;
                case CurrencyType.EnemyBoss:
                    m_GameData.StoneBoss += (uint)resources.Count;
                    break;
                case CurrencyType.Badge:
                    m_GameData.badGe += (uint)resources.Count;
                    break;
                case CurrencyType.Ruby:
                    m_GameData.ruby += (uint)resources.Count;
                    break;
                case CurrencyType.XP:
                    PlayerManager.Instance.AddXP((uint)resources.Count);
                    break;
                case CurrencyType.Watch:
                    break;
                // non-monetized placeholder - invoke in-app purchase logic/interface for a real application
                case CurrencyType.USD:
                    break;
            }
            Debug.Log("Call");
            UpdateFunds();
        }
        void ReceivePurchasedGoods(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return;

            ShopItemType contentType = shopItem.contentType;
            uint contentValue = shopItem.quantityContent;

            ReceiveContent(contentType, contentValue);
        }
        // for gifts or purchases
        void ReceiveContent(ShopItemType contentType, uint contentValue)
        {
            switch (contentType)
            {
                case ShopItemType.Craft:
                    //Logic Item ADD
                    break;
                case ShopItemType.Medicine:
                    //Logic Item ADD
                    break;
                case ShopItemType.Skill:
                    //Logic Item ADD
                    break;
                case ShopItemType.Item:
                    //Logic Item ADD
                    break;
                case ShopItemType.Ruby:
                    //Logic Item ADD
                    break;
                case ShopItemType.Watch:
                    //Logic Item ADD
                    m_GameData.ruby += contentValue;
                    Debug.Log("ADD Ruby: " + contentType);
                    break;
            }
            UpdateFunds();
        }
        void DeductItemShopByID(ShopItemSO shopItem, ShopItemComponent shopItemComponent)
        {
            if(shopItem == null || shopItemComponent == null) return;

            shopItem.contentValue--;

            shopItemComponent.SetContentValue((int)shopItem.contentValue);

            if (shopItem.contentValue <= 0)
            {
                shopItemComponent.UpdateBuyButton(false);
            }
            else
            {
                shopItemComponent.UpdateBuyButton(true);
            }

            if (shopItem.contentType == ShopItemType.Watch)
            {
                shopItemComponent.StartWatch();
            }
        }
        // event-handling methods

        // buying item from ShopScreen, pass button screen position 
        void OnPurchaseItem(ShopItemSO shopItem, ShopItemComponent shopItemComponent, Vector2 screenPos)
        {
            if (shopItem == null)
                return;
            //Deduct Case Watch
            if(shopItemComponent.IsWatch)
            {
                //On Event
                return;
            }
            if(shopItem.contentType == ShopItemType.Watch)
            {
                Rewarded.Instance.ShowRewardedAdShop();
            }

            // invoke transaction succeeded or failed
            if (HasSufficientFunds(shopItem))
            {
                PayTransaction(shopItem);
                ReceivePurchasedGoods(shopItem);
                DeductItemShopByID(shopItem, shopItemComponent);
                TransactionProcessed?.Invoke(shopItem, screenPos);

                Debug.Log("Vector Receive: " + screenPos);

                //AudioManager.PlayDefaultTransactionSound();

                Debug.Log("OnPurchaseItem");
            }
            else
            {
                // notify listeners (PopUpText, sound, etc.)
                //TransactionFailed?.Invoke(shopItem);
                //AudioManager.PlayDefaultWarningSound();

                Debug.Log("NotPurchaseItem");
            }
        }
    }
}
