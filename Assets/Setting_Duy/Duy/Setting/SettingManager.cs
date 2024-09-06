using System;
using UIGameDataManager;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private static SettingManager instance;
    public static SettingManager Instance { get => instance; set => instance = value; }

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

    public void LoadSetting(GameData data){
        if(data== null)
        {
            Debug.Log("Data Null");
        }
        

        currentSettings = new Settings(data.settings);

        Debug.Log("Current Setting: " + currentSettings.musicVolume);
        //settingUI.LoadUIFromSetting(currentSettings);
        Debug.Log("Load Setting");
    }

    public void SaveSetting(){
        GameDataManager.Instance.GameData.settings = currentSettings;
        GameDataManager.Instance.SaveManager.SaveGame();
        Debug.Log("Save Setting");
    }

    public void OpenSetting(){
        LoadSetting(GameDataManager.Instance.GameData);
        settingUI.gameObject.SetActive(true);
    }

    public void CloseSetting(){
        settingUI.gameObject.SetActive(false);
    }
}
