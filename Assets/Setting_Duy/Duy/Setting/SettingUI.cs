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
    public Slider MusicVolumeSlider;
    public Toggle SFXMuteToggle;
    public Slider SFXVolumeSlider;
    public TextMeshProUGUI GraphicText;
    public Button NextGraphic;
    public Button PreviousGraphic;
    public Image LocaleImage;
    public Button NextLocale;
    public Button PreviousLocale;

    private void OnEnable(){
        SaveButton.onClick.AddListener(() => SettingManager.Instance.SaveSetting());
        SaveButton.onClick.AddListener(() => CloseSettingUI());
    }

    private void OnDisable(){
        SaveButton.onClick.RemoveAllListeners();
    }

    public void LoadUIFromSetting(Settings settings) {
        MusicVolumeSlider.value = settings.musicVolume;
        SFXVolumeSlider.value = settings.sfxVolume;
        MusicMuteToggle.isOn = settings.musicMute;
        SFXMuteToggle.isOn = settings.sfxMute;
        LocalizationManager.Instance.ChangeLocale(settings.localeID);
        GraphicManager.Instance.ChangeGraphic(settings.graphic);
    }
    //public void LoadUIFromSettingInGame(Settings settings)
    //{
    //    MusicVolumeSlider.value = settings.musicVolume;
    //    SFXVolumeSlider.value = settings.sfxVolume;
    //    MusicMuteToggle.isOn = settings.musicMute;
    //    SFXMuteToggle.isOn = settings.sfxMute;
    //}

    public void OpenSettingUI(){
        SettingManager.Instance.LoadSetting(UIGameDataManager.GameDataManager.Instance.GameData);
        gameObject.SetActive(true);
    }

    public void CloseSettingUI(){
        gameObject.SetActive(false);
    }

}
