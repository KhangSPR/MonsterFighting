using System;
using UIGameDataManager;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private static SettingManager instance;
    public static SettingManager Instance { get => instance; set => instance = value; }

    public bool isSaved = true;

    public SettingUI settingUI;
    public Settings currentSettings;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void OnEnable(){
        SaveManager.OnGameDataLoaded += LoadSetting;
    }

    private void OnDisable(){
        SaveManager.OnGameDataLoaded -= LoadSetting;
    }

    private void Update(){
        isSaved = currentSettings.Compare(GameDataManager.Instance.GameData.settings);
    }

    public void LoadSetting(GameData data){
        currentSettings = data.settings;
        settingUI.LoadUIFromSetting(currentSettings);
        Debug.Log("Load Setting");
    }

    public void SaveSetting(){
        if (isSaved) return;
        GameDataManager.Instance.GameData.settings = currentSettings;
        GameDataManager.Instance.SaveManager.SaveGame();
        Debug.Log("Save Setting");
    }

    public void ReloadSetting(){
        LoadSetting(GameDataManager.Instance.GameData);
        Debug.Log("Reload Setting");
    }
}
