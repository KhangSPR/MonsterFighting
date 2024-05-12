using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

namespace UIGameDataManager
{
    // stores data for character instance + static data from a ScriptableObject

    public class CharacterData : MonoBehaviour
    {
        public static event Action<CharacterData> LevelIncremented;
        public static event Action<int> numBerStar;
        public static event Action<CharacterData> StarIncremented;
        public static event Action<bool> Maxlv;

        // how quickly XP requirements increase as level increases
        const float k_ProgressionFactor = 10f;
        const float k_ProgressionStat = 5f;
        const float k_ProgressionStar = 5f;
        int k_EnableMaxlv = 10;


        // currently equipped gear
        //[SerializeField] EquipmentSO[] m_GearData = new EquipmentSO[4];

        // baseline data and common shared stats
        [SerializeField] CardCharacter m_CharacterBaseData;


        [SerializeField] GameObject m_PreviewInstance;

        // Experience and level, serialized for demo purposes
        [SerializeField] int m_CurrentLevel;
        [SerializeField] int m_Star;

        public int Star
        {
            get { return m_Star; }
            set
            {
                if (m_Star != value)
                {
                    value = m_Star;
                    // Thực hiện xử lý khi m_Star thay đổi
                }
            }
        }

        public int CurrentLevel
        {
            get { return m_CurrentLevel; }
            set
            {
                if (m_CurrentLevel != value)
                {
                    value = m_Star;
                    // Thực hiện xử lý khi m_CurrentLevel thay đổi
                }
            }
        }

        public GameObject PreviewInstance { get { return m_PreviewInstance; } set { m_PreviewInstance = value; } }
        public CardCharacter CharacterBaseData => m_CharacterBaseData;
        //public EquipmentSO[] GearData => m_GearData;
        public void SetCharacterBaseData(CardCharacter cardTower)
        {
            m_CharacterBaseData = cardTower;
        }
        public void SetDataCharacter(int CurrentLevel, int Star)
        {
            m_CurrentLevel = CurrentLevel;
            m_Star = Star;

            Debug.Log("CurrentLevel: " + m_CurrentLevel);
            Debug.Log("Star: " + m_Star);
        }
        // clamped to non-negative values  
        //Exp ponts Needed
        public uint GetXPForNextLevel()
        {
            return (uint)GetPotionsForNextLevel(m_CurrentLevel, k_ProgressionFactor, m_Star);
        }
        // power potions needed to increment character level
        //This method calculates the number of "power potions" needed to increment the character level
        int GetPotionsForNextLevel(int currentLevel, float progressionFactor, int star)
        {
            //currentStar = 

            currentLevel = Mathf.Clamp(currentLevel, 1, currentLevel);
            progressionFactor = Mathf.Clamp(progressionFactor, 1f, progressionFactor);

            float xp = (progressionFactor * (currentLevel) + (star*10));
            xp = Mathf.Ceil((float)xp);
            return (int)xp;
        }  
        //Star ponts Needed
        public uint GetXPForNextStar()
        {
            return (uint)GetPotionsForNextStar(m_Star, k_ProgressionStar);
        }
        // power potions needed to increment character level
        //This method calculates the number of "power potions" needed to increment the character level
        int GetPotionsForNextStar(int currentStar, float progressionStar)
        {
            if (currentStar == 0)
            {
                Debug.Log("IndexStat: " + 5 + " Gems");
                return 5;

            }
            currentStar = Mathf.Clamp(currentStar, 1, currentStar);
            progressionStar = Mathf.Clamp(progressionStar, 1f, progressionStar);

            float xp = (progressionStar * (currentStar)) + 5f;
            xp = Mathf.Ceil((float)xp);

            return (int)xp;
        }

        //Stat Increase Index
        public uint GetStatForNextStar()
        {
            return (uint)IncreaseStatsForStarUpgrade(m_Star, k_ProgressionStat);
        }

        int IncreaseStatsForStarUpgrade(int star, float k_ProgressionStat)
        {
            if (star == 0)
                return 0;
            // Tăng các chỉ số khi Star tăng lên 1 sao
            star = Mathf.Clamp(star, 1, star);
            k_ProgressionStat = Mathf.Clamp(k_ProgressionStat, 1f, k_ProgressionStat);
            float IndexStat = (k_ProgressionStat * (star));

            IndexStat = Mathf.Ceil((float)IndexStat);

            return (int)IndexStat;
        }

        public void IncrementLevel()
        {
            m_CurrentLevel++;
            LevelIncremented?.Invoke(this);

            Debug.Log("MaxLv(): " + m_CurrentLevel);

            CheckAndInvokeMaxLv();

        }
        public void IncrementStar()
        {
            m_Star++;
            StarIncremented?.Invoke(this);

            Debug.Log("Star: " + Star);

            numBerStar?.Invoke(m_Star);

            CheckAndInvokeStarIncrease();

        }
        private void CheckAndInvokeStarIncrease()
        {
            Maxlv?.Invoke(false);
        }
        private void CheckAndInvokeMaxLv()
        {
            if (ObjMaxLv())
                Maxlv?.Invoke(true);
        }
        public bool ObjMaxLv()
        {
            return (m_CurrentLevel == k_EnableMaxlv);
        }
        public int ResetLv()
        {
            return m_CurrentLevel = 1;
        }
        //Power
        //public uint GetCurrentPower()
        //{
        //    // temp formula
        //    float basePoints = m_CharacterBaseData.basePointsAttack + m_CharacterBaseData.basePointsDefense + m_CharacterBaseData.basePointsLife + m_CharacterBaseData.basePointsCriticalHit;
        //    float equipmentPoints = 0f;

        //    return (uint)(m_CurrentLevel * basePoints + equipmentPoints) / 10;
        //}
        //
        //notifying subscribers about the level increment
    }
}