[System.Serializable]
public struct Settings
{
    [UnityEngine.Header("GRAPHIC")]
    public int graphic;

    [UnityEngine.Header("AUDIO")]
    public float musicVolume;
    public bool musicMute;
    public float sfxVolume;
    public bool sfxMute;
    [UnityEngine.Header("PointStar")]
    public bool starPoint;

    [UnityEngine.Header("LANGUAGE")]
    public int localeID;

    public bool Compare(Settings settings)
    {
        if (!graphic.Equals(settings.graphic)) return false;
        if (!musicVolume.Equals(settings.musicVolume)) return false;
        if (!musicMute.Equals(settings.musicMute)) return false;
        if (!sfxVolume.Equals(settings.sfxVolume)) return false;
        if (!sfxMute.Equals(settings.sfxMute)) return false;
        if (!localeID.Equals(settings.localeID)) return false;
        if(!starPoint.Equals(settings.starPoint)) return false;
        return true;
    }
}
