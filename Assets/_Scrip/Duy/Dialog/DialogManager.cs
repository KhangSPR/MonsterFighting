using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    private static DialogManager instance;
    public static DialogManager Instance { get => instance; }

    [SerializeField] Typewriter typewriter;
    [SerializeField] Image actorAvatarLeftUI;
    [SerializeField] TextMeshProUGUI actorNameLeftTextUI;
    [SerializeField] Transform actorNameLeftContainerUI;
    [SerializeField] Image actorAvatarRightUI;
    [SerializeField] TextMeshProUGUI actorNameRightTextUI;
    [SerializeField] Transform actorNameRightContainerUI;
    [SerializeField] Button continueBtn;

    [SerializeField] Color activeColor;
    [SerializeField] Color unactiveColor;

    [SerializeField] DialogObject currentDialog;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    // temporary : can call DisplayDialog() any where on Start() and Update()
    private void Start(){
        // DisplayDialog(database.get(dialogName));
        DisplayDialog(currentDialog);
    }

    private void OnEnable(){
        continueBtn.onClick.AddListener(() => Continue());
    }
    private void OnDisable(){
        continueBtn.onClick.RemoveAllListeners();
    }

    public void SetDialog(DialogObject dialog){
        currentDialog = dialog;
        LoadDialogLine(0);
    }

    public void LoadDialogLine(int index){
        currentDialog.index = index;
        var line = currentDialog.lines[index];
        // event
        line.onBeforeDialog?.Invoke();
        typewriter.onTextCompleted += () => line.onAfterDialog?.Invoke();

        typewriter.Set(line.content);
        LoadDialogLeftActor(line);
        LoadDialogRightActor(line);
    }
    public void LoadDialogLeftActor(DialogObject.DialogLine line){
        if (line.speaker.HasFlag(DialogObject.DialogLine.Speaker.Left)){
            actorAvatarLeftUI.color = activeColor;
            actorNameLeftTextUI.gameObject.SetActive(true);
            actorNameLeftContainerUI.gameObject.SetActive(true);
        } else {
            actorAvatarLeftUI.color = unactiveColor;
            actorNameLeftTextUI.gameObject.SetActive(false);
            actorNameLeftContainerUI.gameObject.SetActive(false);
        }
        if (line.leftActor.avatar != null){
            actorAvatarLeftUI.gameObject.SetActive(true);
            actorAvatarLeftUI.sprite = line.leftActor.avatar;
        } else {
            actorAvatarLeftUI.gameObject.SetActive(false);
        }
        if (!string.IsNullOrWhiteSpace(line.leftActor.name)){
            actorNameLeftTextUI.gameObject.SetActive(true);
            actorNameLeftContainerUI.gameObject.SetActive(true);
            actorNameLeftTextUI.text = line.leftActor.name;
        } else {
            actorNameLeftTextUI.gameObject.SetActive(false);
            actorNameLeftContainerUI.gameObject.SetActive(false);
        }
    }
    public void LoadDialogRightActor(DialogObject.DialogLine line){
        if (line.speaker.HasFlag(DialogObject.DialogLine.Speaker.Right)){
            actorAvatarRightUI.color = activeColor;
            actorNameRightTextUI.gameObject.SetActive(true);
            actorNameRightContainerUI.gameObject.SetActive(true);
        } else {
            actorAvatarRightUI.color = unactiveColor;
            actorNameRightTextUI.gameObject.SetActive(false);
            actorNameRightContainerUI.gameObject.SetActive(false);
        }
        if (line.rightActor.avatar != null){
            actorAvatarRightUI.gameObject.SetActive(true);
            actorAvatarRightUI.sprite = line.rightActor.avatar;
        } else {
            actorAvatarRightUI.gameObject.SetActive(false);
        }
        if (!string.IsNullOrWhiteSpace(line.rightActor.name)){
            actorNameRightTextUI.gameObject.SetActive(true);
            actorNameRightContainerUI.gameObject.SetActive(true);
            actorNameRightTextUI.text = line.rightActor.name;
        } else {
            actorNameRightTextUI.gameObject.SetActive(false);
            actorNameRightContainerUI.gameObject.SetActive(false);
        }
    }

    public void Continue(){
        if (typewriter.isTyping) typewriter.Skip();
        else Next();
    }
    public void Next(){
        if (currentDialog.index >= currentDialog.lines.Length-1){
            currentDialog.onCompleted?.Invoke();
            HideDialog();
            currentDialog = null;
        } else {
            var index = ++currentDialog.index;
            LoadDialogLine(index);
        }
    }

    public void DisplayDialog(DialogObject dialog){
        ShowDialog();
        SetDialog(dialog);
    }
    public void ShowDialog(){
        Instance.gameObject.SetActive(true);
    }
    public void HideDialog(){
        Instance.gameObject.SetActive(false);
    }
}
