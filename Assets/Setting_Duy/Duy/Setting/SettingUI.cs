using System;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button SaveButton;

    [Header("Setting Element")]
    public Toggle MusicMuteToggle;
    public UnityEngine.UI.Slider MusicVolumeSlider;
    public Toggle SFXMuteToggle;
    public UnityEngine.UI.Slider SFXVolumeSlider;
    public Image LocaleImage;
    //Star
    public Toggle StarToggle;
    public Button NextLocale;
    public Button PreviousLocale;
    public RectTransform fade;


    private void OnEnable(){
        SaveButton?.onClick.AddListener(() => SettingManager.Instance.SaveSetting());
        SaveButton?.onClick.AddListener(() => CloseSettingUI());
        // audio
        MusicVolumeSlider?.onValueChanged.AddListener((float volume) => AudioManager.Instance.SetMusicVolume(volume));
        SFXVolumeSlider?.onValueChanged.AddListener((float volume) => AudioManager.Instance.SetSFXVolume(volume));
        MusicMuteToggle?.onValueChanged.AddListener((bool mute) => AudioManager.Instance.SetMusicMute(mute));
        SFXMuteToggle?.onValueChanged.AddListener((bool mute) => AudioManager.Instance.SetSFXMute(mute));
        StarToggle?.onValueChanged.AddListener((bool mute) => SettingManager.Instance.SetMusicMute(mute));
        //Star
        // graphic
        // localization
        NextLocale?.onClick.AddListener(() => LocalizationManager.Instance.NextLocale());
        PreviousLocale?.onClick.AddListener(() => LocalizationManager.Instance.PreviousLocale());
    }

    private void OnDisable(){
        SaveButton?.onClick.RemoveAllListeners();
    }

    public void LoadUIFromSetting(Settings settings) {
        MusicVolumeSlider.value = settings.musicVolume;
        SFXVolumeSlider.value = settings.sfxVolume;
        MusicMuteToggle.isOn = settings.musicMute;
        SFXMuteToggle.isOn = settings.sfxMute;
        if(StarToggle!= null)
            StarToggle.isOn = settings.starPoint;
        LocalizationManager.Instance.ChangeLocale(settings.localeID);
    }
    public void LoadUIFromInGameSetting(Settings settings)
    {
       MusicVolumeSlider.value = settings.musicVolume;
       SFXVolumeSlider.value = settings.sfxVolume;
       MusicMuteToggle.isOn = settings.musicMute;
       SFXMuteToggle.isOn = settings.sfxMute;
    }

    public void OpenSettingUI(){
        SettingManager.Instance.LoadSetting(UIGameDataManager.GameDataManager.Instance.GameData);
        LoadUIFromSetting(SettingManager.Instance.currentSettings);
        gameObject.SetActive(true);
        if(fade != null)
            fade.gameObject.SetActive(true);

    }
    public void CloseSettingUI(){
        gameObject.SetActive(false);
        if (fade != null)
            fade.gameObject.SetActive(false);

    }

}
