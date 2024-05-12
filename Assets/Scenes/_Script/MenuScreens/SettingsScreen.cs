using System;
using UnityEngine;
using UnityEngine.UIElements;
//using MyUILibrary;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UIGameDataManager;

namespace UIGameDataManager
{

    // pairs a Theme StyleSheet with a seasonal variation/themes (e.g. Christmas, Halloween, etc.)
    [Serializable]
    public class ThemeSettings
    {
        public string theme;
        public ThemeStyleSheet tss;
    }

    // This controls general settings for the game. Many of these options are non-functional in this demo but
    // show how to sync data from a UI with the GameDataManager.
    public class SettingsScreen : MenuScreen
    {
        public static event Action ResetPlayerFunds;
        //public static event Action ResetPlayerLevel;

        //public static event Action SettingsShown;
        //public static event Action<GameData> SettingsUpdated;

        //[Space]
        //[Tooltip("Define StyleSheets associated with each Theme. Each MenuScreen may have its own StyleSheet.")]
        //[SerializeField] List<ThemeSettings> m_ThemeSettings;

        //// string IDs
        //const string k_PanelBackButton = "settings__panel-back-button";
        //const string k_ResetLevelButton = "settings__social-button1";
        const string k_ResetFundsButton = "settings__social-button2";
        //const string k_PlayerTextfield = "settings__player-textfield";
        //const string k_ExampleToggle = "settings__email-toggle";
        //const string k_ThemeDropdown = "settings__theme-dropdown";
        //const string k_ExampleDropdown = "settings__language-dropdown";
        //const string k_Slider1 = "settings__slider1";
        //const string k_Slider2 = "settings__slider2";
        //const string k_SlideToggle = "settings-notifications__toggle";
        //const string k_SlideToggleOnLabel = "settings-notifications__label-on";
        //const string k_SlideToggleOffLabel = "settings-notifications__label-off";
        //const string k_RadioButtonGroup = "settings__graphics-radio-button-group";

        //const string k_PanelActiveClass = "settings__panel";
        //const string k_PanelInactiveClass = "settings__panel--inactive";
        //const string k_LabelActiveClass = "slide-toggle__label";
        //const string k_LabelInactiveClass = "slide-toggle__label--inactive";
        //const string k_SettingsPanel = "settings__panel";

        //// visual elements
        //Button m_ResetLevelButton;
        Button m_ResetFundsButton;

        void ResetFunds(ClickEvent evt)
        {
            //AudioManager.PlayDefaultButtonSound();
            ResetPlayerFunds?.Invoke();
        }


        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_ResetFundsButton = m_Root.Q<Button>(k_ResetFundsButton);
        }

        protected override void RegisterButtonCallbacks()
        {
            m_ResetFundsButton?.RegisterCallback<ClickEvent>(ResetFunds);

        }


    }
}