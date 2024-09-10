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
    }

    private void OnEnable(){
        graphics.Add(new GraphicData(){name = "Low", width = 1280, height = 720});
        graphics.Add(new GraphicData(){name = "Normal", width = 1920, height = 1080});
        graphics.Add(new GraphicData(){name = "High", width = 2560, height = 1440});
    }

    private void Start(){
        localizeStringEvent = SettingManager.Instance.settingUI.GraphicText.GetComponent<LocalizeStringEvent>();
        localizeStringEvent.StringReference.Arguments = new object[]{SettingManager.Instance.currentSettings.graphic};
    }

    public void NextGraphic(){
        int g = SettingManager.Instance.currentSettings.graphic + 1;
        g = (g >= graphics.Count) ? 0 : g;
        ChangeGraphic(g);
    }

    public void PreviousGraphic(){
        int g = SettingManager.Instance.currentSettings.graphic - 1;
        g = (g < 0) ? graphics.Count - 1 : g;
        ChangeGraphic(g);
    }

    public void ChangeGraphic(int graphic){
        SetGraphic(graphic);
        int width = graphics[graphic].width;
        int height = graphics[graphic].height;
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
    
    public void SetGraphic(int graphic){
        SettingManager.Instance.currentSettings.graphic = graphic;
        localizeStringEvent.StringReference.Arguments[0] = graphic;
        localizeStringEvent.StringReference.RefreshString();
    }
}
