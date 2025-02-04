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

        [Tooltip("Characters to choose from.")]
        [SerializeField] List<CharacterData> m_Characters;

        //[Tooltip("Parent transform for all character previews.")]
        [SerializeField] Transform m_previewTransform;

        public List<CharacterData> M_Characters { get { return m_Characters; } set { m_Characters = value; } }
        public CharacterData CurrentCharacter { get => M_Characters[m_CurrentIndex]; }

        CardStatsSkillUI m_Stats;
        public CardStatsSkillUI Stats { get { return m_Stats; } set { m_Stats = value;  } }

        [SerializeField] int m_CurrentIndex;
        int m_ActiveGearSlot;
        private void OnEnable()
        {
            StatusScreen.ScreenEnabled += OnCharScreenStarted;
            StatusScreen.ScreenDisabled += OnCharScreenEnded;


            StatusScreen.CharStatsWindowUpdated += OnCharStatsWindowUpdated;

            CardInfoCharacterUI.OnNextCharacter += SelectNextCharacter;
            CardInfoCharacterUI.OnPreviousCharacter += SelectLastCharacter;

        }


        private void OnDisable()
        {
            StatusScreen.ScreenEnabled -= OnCharScreenStarted;
            StatusScreen.ScreenDisabled -= OnCharScreenEnded;


            StatusScreen.CharStatsWindowUpdated -= OnCharStatsWindowUpdated;

            CardInfoCharacterUI.OnNextCharacter -= SelectNextCharacter;
            CardInfoCharacterUI.OnPreviousCharacter -= SelectLastCharacter;

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
            InitializeCharPreview();
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
                //if (charData.PreviewInstance == null)
                //{
                //    // Khởi tạo instance mới và gán vào PreviewInstance
                //    //if (charData.CharacterBaseData.characterVisualsPrefab == null) return;

                //    //charData.PreviewInstance = Instantiate(charData.CharacterBaseData.characterVisualsPrefab, m_previewTransform.position, Quaternion.identity);
                //    //charData.PreviewInstance.transform.SetParent(m_previewTransform);
                //    //charData.PreviewInstance.SetActive(false);
                //}

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

            //if(currentCharacter.PreviewInstance != null)
            //    currentCharacter.PreviewInstance?.gameObject.SetActive(state);


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
        void OnCharStatsWindowUpdated(CardStatsSkillUI charStatsWindow)
        {
            // Cập nhật Stats từ CharStatsWindow
            m_Stats = charStatsWindow;
        }
    }
}