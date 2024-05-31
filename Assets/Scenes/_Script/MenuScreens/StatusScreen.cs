using UnityEngine;
//using UnityEngine.UIElements;
using System;
using UnityEngine.UI;
using TMPro;

namespace UIGameDataManager
{
    public class StatusScreen : MonoBehaviour
    {


        public static event Action ScreenEnabled;
        public static event Action ScreenDisabled;
        public static event Action LevelUpClicked;
        public static event Action GemUpClicked;
        public static event Action updateStat;
        public static event Action updateStatMaxlv;
        //public static event Action updateViewStarUp;
        public static event Action<CardStatCharacters> CharStatsWindowUpdated;


        [Header("Exit Panel")]
        [SerializeField] Button m_ExitPanel;

        [Header("LVup")]
        [SerializeField] Button m_LevelUpButton;
        [SerializeField] TMP_Text m_Potions;
        [SerializeField] TMP_Text m_PotionsNextLV;
        [SerializeField] TMP_Text m_LevelTextButton;
        [SerializeField] Image m_LevelImgButton;
        [SerializeField] GameObject m_LevelUpButtonVFX;
        [Header("StarUp")]
        [SerializeField] Button m_StarUpButton;
        [SerializeField] TMP_Text m_Gems;
        [SerializeField] TMP_Text m_GemsNextStar;
        [SerializeField] TMP_Text m_StarTextButton;
        [SerializeField] Image m_StarImgButton;
        [SerializeField] GameObject m_StarUpButtonVFX;

        [Space]
        [Space]
        [Space]
        [Space]
        [Header("Component")]
        [SerializeField] CardStatCharacters m_CharStatsWindow;
        [SerializeField] GameIconsSO m_GameIconsData;

        [Header("OBJ")]
        [SerializeField] GameObject ObjLvup;
        [SerializeField] GameObject ObjStarup;
        private bool hasUpdatedViewUp = false;

        void OnEnable()
        {

            //Enable Prefab
            SetButtonExit();

            CharacterData.Maxlv += OnEnableView;

            GameDataManager.LevelUpButtonEnabled += OnLevelUpButtonEnabled;
            GameDataManager.StarUpButtonEnabled += OnStarUpButtonEnabled;

            CharScreenController.CharacterShown += OnCharacterShown;

            GameDataManager.PotionsUpdated += OnPotionUpdated;
            GameDataManager.GemsUpdated += OnGemUpdate;

            Screenenabled();
            //UpdateViewUp();

        }
        void OnDisable()
        {
            CharacterData.Maxlv -= OnEnableView;

            GameDataManager.LevelUpButtonEnabled -= OnLevelUpButtonEnabled;
            GameDataManager.StarUpButtonEnabled -= OnStarUpButtonEnabled;

            CharScreenController.CharacterShown -= OnCharacterShown;


            GameDataManager.PotionsUpdated -= OnPotionUpdated;
            GameDataManager.GemsUpdated -= OnGemUpdate;

        }
        void SetButtonLvUp()
        {

            if (m_LevelUpButton != null)
            {
                Debug.Log("Set 1 lan ButtonLvUp");
                RemoveButtonLvUpListener();
                m_LevelUpButton.onClick.AddListener(LevelUpCharacter);
            }
        }
        void SetButtonStarUp()
        {
            if (m_StarUpButton != null)
            {
                Debug.Log("Set 1 lan");
                RemoveButtonStarUpListener();
                m_StarUpButton.onClick.AddListener(StarUpCharacter);
            }
        }
        void RemoveButtonLvUpListener()
        {
            if (m_LevelUpButton != null)
            {
                m_LevelUpButton.onClick.RemoveListener(LevelUpCharacter);
            }
        }
        void RemoveButtonStarUpListener()
        {
            if (m_LevelUpButton != null)
            {
                m_StarUpButton.onClick.RemoveListener(StarUpCharacter);
            }
        }
        void SetButtonExit()
        {
            if (m_ExitPanel != null)
            {
                m_ExitPanel.onClick.AddListener(Screendisabled);
            }
        }
        void UpdateViewLvUp()
        {
            if (m_CharStatsWindow == null)
                return;

            ObjLvup.SetActive(true);

            ObjStarup.SetActive(false);

        }
        void UpdateViewStarUp()
        {
            if (m_CharStatsWindow == null)
                return;

            ObjLvup.SetActive(false);


            ObjStarup.SetActive(true);

        }
        void Screenenabled()
        {
            ScreenEnabled?.Invoke();
        }
        void Screendisabled()
        {
            ScreenDisabled?.Invoke();
        }
        void LevelUpCharacter()
        {
            LevelUpClicked?.Invoke();
        }
        void StarUpCharacter()
        {
            GemUpClicked?.Invoke();
        }
        void UpdateViewUp()
        {
            if (m_CharStatsWindow == null)
            {
                Debug.Log("Khong set Duoc CardStatsTower");
                return;
            }

            CharacterData characterData = m_CharStatsWindow.GetCharacterData(); // ham nay goi truoc

            if (characterData != null)
            {
                bool isMaxLevel = characterData.ObjMaxLv();

                Debug.Log("Da goi UpdateViewUp");

                if (isMaxLevel)
                {
                    updateStatMaxlv?.Invoke();
                    OnEnableView(isMaxLevel); //2

                    Debug.Log("Da goi MaxLV");

                }
                else
                {

                    updateStat?.Invoke();
                    OnEnableView(isMaxLevel);


                    Debug.Log("Da goi Stat");

                }
            }
        }
        void OnEnableView(bool state)
        {

            if (state)
            {
                UpdateViewStarUp();

                SetButtonStarUp();


                Debug.Log("-----------OnEnableView--------------");
            }
            else if (!state)
            {
                UpdateViewLvUp();

                SetButtonLvUp();


                Debug.Log("===========OnEnableView=========");


            }
        }
        void OnPotionUpdated(GameData gameData)
        {
            if (m_Potions == null)
                return;

            if (gameData == null)
                return;
            //LV XP1, XP2, XP3
            m_Potions.text = gameData.xpLv1.ToString();

            UpdatePotionCountLabel();
        }
        void OnGemUpdate(GameData gameData)
        {
            if (m_Gems == null)
                return;

            if (gameData == null)
                return;
            Debug.Log("OnGemUpdate" + gameData.enemyBoss);

            m_Gems.text = gameData.enemyBoss.ToString();

            UpdateGemCountLable();
        }
        public void OnCharacterShown(CharacterData characterToShow)
        {
            if (characterToShow == null)
            {
                Debug.Log("No Show Potions");

                return;

            }
            if(characterToShow.PreviewInstance == null)
            {
                Debug.Log("No PreviewInstance");

                return;
            }
            bool ObjMaxLv = characterToShow.ObjMaxLv();

            //if (m_PowerLabel != null)
            //    m_PowerLabel.text = characterToShow.GetCurrentPower().ToString();
            if (!ObjMaxLv)
            {
                if (m_PotionsNextLV != null)
                {
                    Debug.Log("Da Next XP for level ");

                    Debug.Log(characterToShow.GetXPForNextLevel().ToString() + " XP");

                    m_PotionsNextLV.text = "/" + characterToShow.GetXPForNextLevel().ToString();


                    updateStat?.Invoke();


                    UpdatePotionCountLabel();


                }
                UpdateCharacterStats(characterToShow);
            }
            else if (ObjMaxLv)
            {
                if (m_GemsNextStar != null)
                {
                    Debug.Log("Da goi ");


                    //Debug.Log(characterToShow.GetXPForNextStar().ToString() + " GEM");

                    m_GemsNextStar.text = "/" + characterToShow.GetXPForNextStar().ToString();



                    updateStatMaxlv?.Invoke();


                    UpdateGemCountLable();

                    Debug.Log("Xuat hien / /");
                }
                UpdateCharacterStats(characterToShow);
            }


            characterToShow.PreviewInstance?.gameObject.SetActive(true);

        }
        void OnLevelUpButtonEnabled(bool state)
        {

            Debug.Log("ObjLevel.activeSelf: " + ObjLvup.activeSelf);

            if (m_LevelUpButtonVFX == null || m_LevelUpButton == null)
                return;

            // Toggle the visibility of the VFX
            m_LevelUpButtonVFX.gameObject.SetActive(state);
            m_LevelUpButton.interactable = state;

            // Set the sorting order to ensure the VFX is in front of the button
            //m_LevelUpButtonVFX.GetComponent<Canvas>().sortingOrder = state ? 1 : 0;

            if (state)
            {
                // Activate classes for the active state
                m_LevelImgButton.color = new Color(36 / 255f, 204 / 255f, 0f, 241 / 255f);
                m_LevelTextButton.color = new Color(1f, 1f, 1f, 1f);

                Debug.Log("OnLevelUpButtonEnabled: " + state);

                Debug.Log("Da Bat Mau Level");
            }
            else
            {
                m_LevelImgButton.color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 220 / 255f);
                m_LevelTextButton.color = new Color(150 / 255f, 150 / 255f, 150 / 255f, 130 / 255f);

                Debug.Log("Da Tat Mau Level");
            }

        }
        void OnStarUpButtonEnabled(bool state)
        {
            Debug.Log("ObjStar.activeSelf: " + ObjStarup.activeSelf);




            if (m_StarUpButtonVFX == null || m_StarUpButton == null)
                return;

            // Toggle the visibility of the VFX
            m_StarUpButtonVFX.gameObject.SetActive(state);
            m_StarUpButton.interactable = state;

            // Set the sorting order to ensure the VFX is in front of the button
            //m_LevelUpButtonVFX.GetComponent<Canvas>().sortingOrder = state ? 1 : 0;

            if (state)
            {
                // Activate classes for the active state
                m_StarImgButton.color = new Color(1f, 1f, 1f, 1f);
                m_StarTextButton.color = new Color(1f, 1f, 1f, 1f);

                Debug.Log("Da Bat Mau");
            }
            else
            {
                m_StarImgButton.color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 220 / 255f);
                m_StarTextButton.color = new Color(150 / 255f, 150 / 255f, 150 / 255f, 130 / 255f);

                Debug.Log("Da Tat Mau");
            }

        }
        public void UpdateCharacterStats(CharacterData characterToShow)
        {

            //// update character statistics UI
            //if (m_GameIconsData == null)
            //{
            //    Debug.LogWarning("CharScreen.ShowCharacter: missing GameIcons ScriptableObject data");
            //    return;
            //}

            // create the CharStatsWindow if it doesn't exist already; otherwise, just update it
            if (m_CharStatsWindow != null)
            {


                m_CharStatsWindow.SetCardStatusTower(m_GameIconsData, characterToShow); //Set

                if (!hasUpdatedViewUp)
                {
                    UpdateViewUp();
                    hasUpdatedViewUp = true;

                    characterToShow.SetDataCharacter(m_CharStatsWindow.GetCardTower().Level, m_CharStatsWindow.GetCardTower().Star);
                }

                m_CharStatsWindow?.SetGameData();

                CharStatsWindowUpdated?.Invoke(m_CharStatsWindow);


            }
            else
            {
                m_CharStatsWindow.UpdateWindow(characterToShow);
            }
        }

        void UpdateGemCountLable()
        {
            if (m_Gems == null)
                return;
            string gemsForNextLevelString = m_Gems.text.TrimStart('/');

            if (gemsForNextLevelString != string.Empty)
            {
                int gemsForNextLevel = Int32.Parse(gemsForNextLevelString);
                int gemsCount = Int32.Parse(m_Gems.text);
                m_Gems.color = (gemsForNextLevel > gemsCount) ? new Color(0.88f, .36f, 0f) : new Color(30f / 255f, 255f / 255f, 25f / 255f); // Your desired green color
            }
        }
        void UpdatePotionCountLabel()
        {
            if (m_Potions == null)
                return;

            string potionsForNextLevelString = m_Potions.text.TrimStart('/');

            if (potionsForNextLevelString != string.Empty)
            {
                int potionsForNextLevel = Int32.Parse(potionsForNextLevelString);
                int potionsCount = Int32.Parse(m_Potions.text);
                m_Potions.color = (potionsForNextLevel > potionsCount) ? new Color(0.88f, .36f, 0f) : new Color(30f / 255f, 255f / 255f, 25f / 255f); // Your desired green color
            }
        }
    }
}
