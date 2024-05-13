using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipInfo {
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get => instance; set => instance = value; }

    [Header("Audio Sources")]
    public AudioSource music;
    public AudioSource sfx;

    [Header("Audio Clips")] [Space(5)]
    [SerializeField] List<AudioClipInfo> musicClips = new();
    [SerializeField] List<AudioClipInfo> sfxClips = new();

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void OnEnable(){
        music = Instantiate(new GameObject("Music").AddComponent<AudioSource>(), transform);
        sfx = Instantiate(new GameObject("SFX").AddComponent<AudioSource>(), transform);
        music.loop = true;
        sfx.playOnAwake = false;
    }

    private void Start(){
        SettingManager.Instance.settingUI.MusicVolumeSlider.onValueChanged.AddListener((float volume) => SetMusicVolume(volume));
        SettingManager.Instance.settingUI.SFXVolumeSlider.onValueChanged.AddListener((float volume) => SetSFXVolume(volume));
        SettingManager.Instance.settingUI.MuteToggle.onValueChanged.AddListener((bool mute) => SetMute(mute));

        PlayMusic("AudioGame");
    }

    public void PlayMusic(string clipName, float delayedTime = 0){
        music.clip = musicClips.Find(x => x.name.Equals(clipName))?.clip;
        music.PlayDelayed(delayedTime);
    }

    public void PlaySFX(string clipName){
        sfx.PlayOneShot(sfxClips.Find(x => x.name.Equals(clipName))?.clip);
    }

    public void SetMusicVolume(float volume){
        SettingManager.Instance.currentSettings.musicVolume = volume;
        music.volume = volume;
        SettingManager.Instance.settingUI.MusicVolumeField.text = ((int)(volume * 100)).ToString();
    }

    public void SetSFXVolume(float volume){
        SettingManager.Instance.currentSettings.sfxVolume = volume;
        sfx.volume = volume;
        SettingManager.Instance.settingUI.SFXVolumeField.text = ((int)(volume * 100)).ToString();
    }

    public void SetMute(bool mute){
        SettingManager.Instance.currentSettings.mute = mute;
        music.gameObject.SetActive(!mute);
        sfx.gameObject.SetActive(!mute);
    }
}
