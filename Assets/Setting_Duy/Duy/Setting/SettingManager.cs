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
    private void Start()
    {
        SaveManager.OnGameDataLoaded += LoadSetting;
    }
    private void OnEnable(){
        SaveManager.OnGameDataLoaded += LoadSetting;
    }

    private void OnDisable(){
        SaveManager.OnGameDataLoaded -= LoadSetting;
    }

    public void LoadSetting(GameData data){
        currentSettings = data.settings;
        Debug.Log("Load Setting");
    }
    public void SetMusicMute(bool mute)
    {
        currentSettings.starPoint = mute;
    }
    public void SaveSetting(){
        GameDataManager.Instance.GameData.settings = currentSettings;
        GameDataManager.Instance.SaveManager.SaveGame();
        Debug.Log("GameDataManager: " + currentSettings.starPoint +"Setting: "+ currentSettings.musicMute);

        Debug.Log("Save Setting");
    }
    
}
