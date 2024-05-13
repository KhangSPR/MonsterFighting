using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    private static DisplayManager instance;
    public static DisplayManager Instance { get => instance; set => instance = value; }

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void OnEnable(){
        SettingManager.Instance.settingUI.ResolutionDropDown.ClearOptions();
        //
        List<string> resolationOptions = new();
        for (int i = 0; i < Screen.resolutions.Length; i++){
            resolationOptions.Add(Screen.resolutions[i].width + "x" + Screen.resolutions[i].height);
        }
        SettingManager.Instance.settingUI.ResolutionDropDown.AddOptions(resolationOptions);
    }

    private void Start(){
        SettingManager.Instance.settingUI.ResolutionDropDown.onValueChanged.AddListener((int resolution) => SetResolution(resolution));
        SettingManager.Instance.settingUI.FullScreenToggle.onValueChanged.AddListener((bool isFullScreen) => SetFullScreenMode(isFullScreen));
    }

    public void SetFullScreenMode(bool isFullScreen){
        SettingManager.Instance.currentSettings.isFullScreen = isFullScreen;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, isFullScreen);
    }

    public void SetResolution(int resolution){
        SettingManager.Instance.currentSettings.resolutionWidth = Screen.resolutions[resolution].width;
        SettingManager.Instance.currentSettings.resolutionHeight = Screen.resolutions[resolution].height;
        Screen.SetResolution(Screen.resolutions[resolution].width, Screen.resolutions[resolution].height, Screen.fullScreen);
    }
}
