using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct Settings {

    [Header("DISPLAY")]
    public int resolutionWidth;
    public int resolutionHeight;
    public bool isFullScreen;

    [Header("AUDIO")]
    public float musicVolume;
    public float sfxVolume;
    public bool mute;

    [Header("LANGUAGE")]
    public int localeID;

    public bool Compare(Settings settings){
        if (!resolutionWidth.Equals(settings.resolutionWidth)) return false;
        if (!resolutionHeight.Equals(settings.resolutionHeight)) return false;
        if (!isFullScreen.Equals(settings.isFullScreen)) return false;
        if (!musicVolume.Equals(settings.musicVolume)) return false;
        if (!sfxVolume.Equals(settings.sfxVolume)) return false;
        if (!mute.Equals(settings.mute)) return false;
        if (!localeID.Equals(settings.localeID)) return false;
        return true;
    }
}

