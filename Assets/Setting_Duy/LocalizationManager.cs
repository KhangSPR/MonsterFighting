using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

[System.Serializable]
public struct Language {
    public string name;
    public Locale locale;
}

public class LocalizationManager : MonoBehaviour
{
    private static LocalizationManager instance;
    public static LocalizationManager Instance { get => instance; set => instance = value; }

    [SerializeField] List<Language> languages = new();

    private bool active = false;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void OnEnable(){
        if (SettingManager.Instance == null || SettingManager.Instance.settingUI == null)
        {
            Debug.LogError("SettingManager or settingUI has not been initialized.");
            return; // Exit the method if SettingManager or settingUI is not available
        }
        SettingManager.Instance.settingUI.LanguageDropDown.ClearOptions();
        //
        List<string> languageOptions = new();
        for (int i = 0; i < languages.Count; i++){
            languageOptions.Add(languages[i].name);
        }
        SettingManager.Instance.settingUI.LanguageDropDown.AddOptions(languageOptions);
    }

    private void Start(){
        SettingManager.Instance.settingUI.LanguageDropDown.onValueChanged.AddListener((int language) => ChangeLocale(language));
    }

    public void ChangeLocale(int localeID){
        if (active == true) return;
        SettingManager.Instance.currentSettings.localeID = localeID;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int localeID){
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = languages[localeID].locale;
        active = false;
    }
}
