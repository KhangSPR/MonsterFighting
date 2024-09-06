using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

[System.Serializable]
public struct GraphicData {
    [HideInInspector] public string name;
    public int width;
    public int height;
}

public class GraphicManager : MonoBehaviour
{
    private static GraphicManager instance;
    public static GraphicManager Instance { get => instance; set => instance = value; }

    [SerializeField] List<GraphicData> graphics = new(3);
    LocalizeStringEvent localizeStringEvent;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
        localizeStringEvent = SettingManager.Instance.settingUI.GraphicText.GetComponent<LocalizeStringEvent>();
    }

    private void OnEnable(){
        localizeStringEvent.StringReference.Arguments = new object[]{SettingManager.Instance.currentSettings.graphic};
        graphics.Add(new GraphicData(){name = "Low", width = 1280, height = 720});
        graphics.Add(new GraphicData(){name = "Normal", width = 1920, height = 1080});
        graphics.Add(new GraphicData(){name = "High", width = 2560, height = 1440});
    }

    private void Start(){
        SettingManager.Instance.settingUI.NextGraphic.onClick.AddListener(() => NextGraphic());
        SettingManager.Instance.settingUI.PreviousGraphic.onClick.AddListener(() => PreviousGraphic());
        localizeStringEvent.StringReference.Arguments[0] = SettingManager.Instance.currentSettings.graphic;
        SetResolution(SettingManager.Instance.currentSettings.graphic);
    }

    private void NextGraphic(){
        int g = SettingManager.Instance.currentSettings.graphic + 1;
        g = (g >= graphics.Count) ? 0 : g;
        SettingManager.Instance.currentSettings.graphic = g;
        localizeStringEvent.StringReference.Arguments[0] = g;
        localizeStringEvent.StringReference.RefreshString();
        SetResolution(g);
    }

    private void PreviousGraphic(){
        int g = SettingManager.Instance.currentSettings.graphic - 1;
        g = (g < 0) ? graphics.Count - 1 : g;
        SettingManager.Instance.currentSettings.graphic = g;
        localizeStringEvent.StringReference.Arguments[0] = g;
        localizeStringEvent.StringReference.RefreshString();
        SetResolution(g);
    }

    public void SetResolution(int graphic){
        int width = graphics[graphic].width;
        int height = graphics[graphic].height;
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
