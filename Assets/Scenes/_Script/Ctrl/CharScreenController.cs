using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace UIGameDataManager
{
    public class CharScreenController : MonoBehaviour
    {

        public static event Action<CharacterData> CharacterShown;
        public static event Action<CharacterData> LevelPotionUsed;
        public static event Action<CharacterData> StarGemUsed;

        [Tooltip("Characters to choose from.")]
        List<CharacterData> m_Characters;

        //[Tooltip("Parent transform for all character previews.")]
        [SerializeField] Transform m_previewTransform;

        [Header("Inventory")]
        [Tooltip("Check this option to allow only one type of gear (armor, weapon, etc.) per character.")]
        [SerializeField] bool m_UnequipDuplicateGearType;

        [Header("Level Up")]
        [SerializeField][Tooltip("Controls playback of level up FX.")] PlayableDirector m_LevelUpPlayable;

        public List<CharacterData> M_Characters { get { return m_Characters; } set { m_Characters = value; } }
        public CharacterData CurrentCharacter { get => M_Characters[m_CurrentIndex]; }

        CardStatsTower m_Stats;
        public CardStatsTower Stats { get { return m_Stats; } set { m_Stats = value;  } }

        [SerializeField] int m_CurrentIndex;
        int m_ActiveGearSlot;
        private void OnEnable()
        {
            StatusScreen.ScreenEnabled += OnCharScreenStarted;
            StatusScreen.ScreenDisabled += OnCharScreenEnded;
            StatusScreen.LevelUpClicked += OnLevelUpClicked;
            StatusScreen.GemUpClicked += OnStarUpClicked;

            CharacterData.LevelIncremented += OnLevelIncremented;
            CharacterData.StarIncremented += OnStarIncremented;

            GameDataManager.CharacterLeveledUp += OnCharacterLeveledUp;
            GameDataManager.CharacterGemedUp += OnCharacterGemedUp;

            StatusScreen.CharStatsWindowUpdated += OnCharStatsWindowUpdated;


        }


        private void OnDisable()
        {
            StatusScreen.ScreenEnabled -= OnCharScreenStarted;
            StatusScreen.ScreenDisabled -= OnCharScreenEnded;
            StatusScreen.LevelUpClicked -= OnLevelUpClicked;
            StatusScreen.GemUpClicked -= OnStarUpClicked;

            CharacterData.LevelIncremented -= OnLevelIncremented;
            CharacterData.StarIncremented -= OnStarIncremented;

            GameDataManager.CharacterLeveledUp -= OnCharacterLeveledUp;
            GameDataManager.CharacterGemedUp -= OnCharacterGemedUp;

            StatusScreen.CharStatsWindowUpdated -= OnCharStatsWindowUpdated;
        }
        void Start()
        {
            //InitializeCharPreview();


        }
        public void SetCurrentIndex(int index)
        {
            m_CurrentIndex = index;
        }
        // update the upper left level meter
        void UpdateView()
        {
            if (m_Characters.Count == 0)
                return;

            // show the Character Prefab
            if(CurrentCharacter !=null)
            {
                CharacterShown?.Invoke(CurrentCharacter);


                Debug.Log("UpdateView");
            }

            // update the four gear slots
            //UpdateGearSlots();
        }
        // event-handling methods
        public void ClearAndShowPreviews()
        {
            // Lặp qua tất cả các đối tượng con trong m_previewTransform
            for (int i = 0; i < m_previewTransform.childCount; i++)
            {
                // Lấy đối tượng con tại vị trí i
                Transform child = m_previewTransform.GetChild(i);

                // Kiểm tra xem đối tượng con có tồn tại không
                if (child != null)
                {
                    // Hủy bỏ đối tượng con
                    Destroy(child.gameObject);
                }
            }

            // Hiển thị lại đối tượng
            //InitializeCharPreview();
        }

        // preview GameObject for each character
        void InitializeCharPreview()
        {
            foreach (CharacterData charData in m_Characters)
            {
                if (charData == null)
                {
                    Debug.LogWarning("CharScreenController.InitializeCharPreview Warning: Missing character data.");
                    continue;
                }

                // Kiểm tra xem PreviewInstance đã được tạo chưa
                if (charData.PreviewInstance == null)
                {
                    // Nếu chưa, tạo mới và gán vào PreviewInstance
                    //charData.PreviewInstance = Instantiate(charData.CharacterBaseData.characterVisualsPrefab);
                    charData.PreviewInstance.transform.SetParent(m_previewTransform);
                    charData.PreviewInstance.SetActive(false);
                }
            }

            //Initialized?.Invoke();
        }


        // character preview methods
        public void SelectNextCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex++;
            if (m_CurrentIndex >= m_Characters.Count)
                m_CurrentIndex = 0;

            // select next character from m_Characters and refresh the CharScreen
            UpdateView();
        }

        public void SelectLastCharacter()
        {
            if (m_Characters.Count == 0)
                return;

            ShowCharacterPreview(false);

            m_CurrentIndex--;
            if (m_CurrentIndex < 0)
                m_CurrentIndex = m_Characters.Count - 1;

            // select last character from m_Characters and refresh the CharScreen
            UpdateView();
        }


        void ShowCharacterPreview(bool state)
        {
            if (m_Characters.Count == 0)
                return;

            CharacterData currentCharacter = m_Characters[m_CurrentIndex];
            currentCharacter.PreviewInstance?.gameObject.SetActive(state);


            //UpdateLevelMeter();

        }

        void OnCharScreenStarted()
        {
            UpdateView();
            ShowCharacterPreview(true);
        }

        void OnCharScreenEnded()
        {
            ShowCharacterPreview(false);
        }
        // click the level up button
        void OnLevelUpClicked()
        {
            // notify GameDataManager that we want to spend LevelUpPotion
            Debug.Log("OnLevelUpClicked(): Da click");

            LevelPotionUsed?.Invoke(CurrentCharacter);
        }
        void OnStarUpClicked()
        {
            // notify GameDataManager that we want to spend LevelUpPotion
            StarGemUsed?.Invoke(CurrentCharacter);

            Debug.Log("OnStarUpClicked(): Da click");
        }
        // success or failure when leveling up a character 
        void OnCharacterLeveledUp(bool didLevel)
        {
            //Enable Max Lv

            if (didLevel)
            {

                //increment the Player Level
                CurrentCharacter.IncrementLevel();

                //CardStatsTower
                m_Stats.LevelUpStats();

                // playback the FX sequence
                m_LevelUpPlayable?.Play();

                //StarUpOnEnableView?.Invoke(false);

                //CurrentCharacter.EnableMaxLv(); //1
            }
        }
        void OnCharacterGemedUp(bool didLevel)
        {
            if (didLevel)
            {
                //increment the Player Level
                CurrentCharacter.IncrementStar();

                //CardStatsTower
                //m_Stats.LevelUpStats();

                Debug.Log("Star Up");

                // playback the FX sequence
                m_LevelUpPlayable?.Play();


                m_Stats.SetGameDataStarUp();

                CurrentCharacter.ResetLv();

                CharacterShown?.Invoke(CurrentCharacter); //no


            }
        }
        // update the character stats UI
        void OnLevelIncremented(CharacterData charData)
        {
            if (charData == CurrentCharacter)
            {
                CharacterShown?.Invoke(CurrentCharacter);
            }
        }
        void OnStarIncremented(CharacterData charData)
        {
            if (charData == CurrentCharacter)
            {
                CharacterShown?.Invoke(CurrentCharacter);
            }
        }
        void OnResetPlayerLevel()
        {
            foreach (CharacterData charData in m_Characters)
            {
                charData.CurrentLevel = 1;
            }
            CharacterShown?.Invoke(CurrentCharacter);
            //UpdateLevelMeter();
        }
        void OnCharStatsWindowUpdated(CardStatsTower charStatsWindow)
        {
            // Cập nhật Stats từ CharStatsWindow
            m_Stats = charStatsWindow;
        }
    }
}