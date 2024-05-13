using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] GameObject settingWindow;
    [SerializeField] GameObject settingWarning;

    [Header("Button")]
    [SerializeField] Button SaveButton;
    [SerializeField] Button CloseButton;
    [SerializeField] Button CloseWithoutSavingButton;
    [SerializeField] Button CloseWithSavingButton;

    [Header("Setting Element")]
    public TMP_Dropdown ResolutionDropDown;
    public Toggle FullScreenToggle;
    public Slider MusicVolumeSlider;
    public TextMeshProUGUI MusicVolumeField;
    public Slider SFXVolumeSlider;
    public TextMeshProUGUI SFXVolumeField;
    public Toggle MuteToggle;
    public TMP_Dropdown LanguageDropDown;

    private void OnEnable(){
        SaveButton.onClick.AddListener(() => SettingManager.Instance.SaveSetting());
        CloseButton.onClick.AddListener(() => CloseSetting());
        CloseWithoutSavingButton.onClick.AddListener(() => CloseWithoutSaving());
        CloseWithSavingButton.onClick.AddListener(() => CloseWithSaving());
    }

    private void OnDisable(){
        SaveButton.onClick.RemoveAllListeners();
        CloseButton.onClick.RemoveAllListeners();
        CloseWithoutSavingButton.onClick.RemoveAllListeners();
        CloseWithSavingButton.onClick.RemoveAllListeners();
    }

    private void Update(){
        HandleUIAccessibility();
    }

    public void LoadUIFromSetting(Settings settings){
        ResolutionDropDown.value = Array.IndexOf(Screen.resolutions, new Resolution(){
            width = settings.resolutionWidth,
            height = settings.resolutionHeight,
            refreshRateRatio = Screen.currentResolution.refreshRateRatio
        });
        FullScreenToggle.isOn = settings.isFullScreen;
        MusicVolumeSlider.value = settings.musicVolume;
        SFXVolumeSlider.value = settings.sfxVolume;
        MuteToggle.isOn = settings.mute;
        LanguageDropDown.value = settings.localeID;
    }

    private void HandleUIAccessibility(){
        SaveButton.interactable = !SettingManager.Instance.isSaved;
        MusicVolumeSlider.interactable = !SettingManager.Instance.currentSettings.mute;
        SFXVolumeSlider.interactable = !SettingManager.Instance.currentSettings.mute;
    }

    public void OpenSetting(){
        var settingUI = SettingManager.Instance.settingUI;
        settingUI.settingWindow.GetComponent<CanvasGroup>().interactable = true;
        settingUI.gameObject.SetActive(true);
    }

    public void CloseSetting(){
        var settingUI = SettingManager.Instance.settingUI;
        if (SettingManager.Instance.isSaved){
            settingUI.gameObject.SetActive(false);
        }
        else {
            settingUI.settingWindow.GetComponent<CanvasGroup>().interactable = false;
            settingUI.settingWarning.SetActive(true);
        }
    }

    public void CloseWithoutSaving(){
        var settingUI = SettingManager.Instance.settingUI;
        SettingManager.Instance.ReloadSetting();
        settingUI.gameObject.SetActive(false);
        settingUI.settingWarning.SetActive(false);
    }

    public void CloseWithSaving(){
        var settingUI = SettingManager.Instance.settingUI;
        SettingManager.Instance.SaveSetting();
        settingUI.gameObject.SetActive(false);
        settingUI.settingWarning.SetActive(false);
    }

}
