using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace UIGameDataManager
{
    public class StatusScreen : MonoBehaviour
    {


        public static event Action ScreenEnabled;
        public static event Action ScreenDisabled;
        public static event Action<CardStatsSkillUI> CharStatsWindowUpdated;


        [Header("Exit Panel")]
        [SerializeField] Button m_ExitPanel;
        [Space]
        [Space]
        [Space]
        [Space]
        [Header("Component")]
        [SerializeField] CardStatsSkillUI m_CharStatsWindow;
        [SerializeField] GameIconsSO m_GameIconsData;
        private void Start()
        {
            SetButtonExit();
        }

        void OnEnable()
        { 
            CharScreenController.CharacterShown += OnCharacterShown;

            Screenenabled();

        }
        void OnDisable()
        {

            CharScreenController.CharacterShown -= OnCharacterShown;

        }
        void SetButtonExit()
        {
            if (m_ExitPanel != null)
            {
                m_ExitPanel.onClick.AddListener(Screendisabled);
            }
        }
        void Screenenabled()
        {
            ScreenEnabled?.Invoke();
            gameObject.SetActive(true);
        }
        void Screendisabled()
        {
            ScreenDisabled?.Invoke();
            gameObject.SetActive(false);
        }
        public void OnCharacterShown(CharacterData characterToShow)
        {
            if (characterToShow == null)
            {
                Debug.Log("No Show Potions");

                return;

            }
            if (characterToShow.PreviewInstance == null)
            {
                Debug.Log("No PreviewInstance");

                return;
            }

            UpdateCharacterStats(characterToShow);


            characterToShow.PreviewInstance?.gameObject.SetActive(true);

        }
        public void UpdateCharacterStats(CharacterData characterToShow)
        {
            // create the CharStatsWindow if it doesn't exist already; otherwise, just update it
            if (m_CharStatsWindow != null)
            {
                Debug.Log("m_CharStatsWindow !=null");

                m_CharStatsWindow.SetCardStatusTower(m_GameIconsData, characterToShow); //Set


                m_CharStatsWindow?.SetGameData();

                CharStatsWindowUpdated?.Invoke(m_CharStatsWindow);


            }
            else
            {
                m_CharStatsWindow.UpdateWindow(characterToShow);


                Debug.Log("m_CharStatsWindow ==null");
            }
        }
    }
}
