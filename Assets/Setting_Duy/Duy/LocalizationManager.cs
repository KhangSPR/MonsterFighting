using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    private static LocalizationManager instance;
    public static LocalizationManager Instance { get => instance; set => instance = value; }

    private bool active = false;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void Start(){
        SettingManager.Instance.settingUI.NextLocale.onClick.AddListener(() => NextLocale());
        SettingManager.Instance.settingUI.PreviousLocale.onClick.AddListener(() => PreviousLocale());
        StartCoroutine(SetLocale(SettingManager.Instance.currentSettings.localeID));
    }

    private void NextLocale(){
        int localeID = SettingManager.Instance.currentSettings.localeID + 1;
        localeID = (localeID >= LocalizationSettings.AvailableLocales.Locales.Count) ? 0 : localeID;
        ChangeLocale(localeID);
    }

    private void PreviousLocale(){
        int localeID = SettingManager.Instance.currentSettings.localeID - 1;
        localeID = (localeID < 0) ? LocalizationSettings.AvailableLocales.Locales.Count - 1 : localeID;
        ChangeLocale(localeID);
    }

    public void ChangeLocale(int localeID){
        if (active == true) return;
        SettingManager.Instance.currentSettings.localeID = localeID;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int localeID){
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}
