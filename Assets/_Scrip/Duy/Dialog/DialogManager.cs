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
    [SerializeField] TextMeshProUGUI actorNameLeftUI;
    [SerializeField] Image actorAvatarRightUI;
    [SerializeField] TextMeshProUGUI actorNameRightUI;
    [SerializeField] Button continueBtn;

    [SerializeField] Color activeColor;
    [SerializeField] Color unactiveColor;

    [SerializeField] DialogObject currentDialog;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void OnEnable(){
        continueBtn.onClick.AddListener(() => Continue());

        /* temporary */ DisplayDialog(currentDialog);
    }
    private void OnDisable(){
        continueBtn.onClick.RemoveAllListeners();
    }

    public void SetDialog(DialogObject dialog){
        currentDialog = dialog;
        SetLine(0);
    }
    public void SetLine(int index){
        currentDialog.index = index;
        var line = currentDialog.lines[index];
        typewriter.Set(line.content);
        SetLeftActor(line);
        SetRightActor(line);
    }
    public void SetLeftActor(DialogObject.Line line){
        if (!line.leftActor.name.Equals("")){
            actorAvatarLeftUI.gameObject.SetActive(true);
            if (line.leftActor.speaker){
                actorAvatarLeftUI.color = activeColor;
                actorNameLeftUI.gameObject.SetActive(true);
            } else {
                actorAvatarLeftUI.color = unactiveColor;
                actorNameLeftUI.gameObject.SetActive(false);
            }

            var actor = currentDialog.actorDB.GetActor(line.leftActor.name);
            actorAvatarLeftUI.sprite = actor.avatars.Find(x => x.name.Equals(line.leftActor.avatar)).sprite;
            actorNameLeftUI.text = actor.name;
        } else {
            actorAvatarLeftUI.gameObject.SetActive(false);
            actorNameLeftUI.gameObject.SetActive(false);
        }
    }
    public void SetRightActor(DialogObject.Line line){
        if (!line.rightActor.name.Equals("")){
            actorAvatarRightUI.gameObject.SetActive(true);
            if (line.rightActor.speaker){
                actorAvatarRightUI.color = activeColor;
                actorNameRightUI.gameObject.SetActive(true);
            } else {
                actorAvatarRightUI.color = unactiveColor;
                actorNameRightUI.gameObject.SetActive(false);
            }

            var actor = currentDialog.actorDB.GetActor(line.rightActor.name);
            actorAvatarRightUI.sprite = actor.avatars.Find(x => x.name.Equals(line.rightActor.avatar)).sprite;
            actorNameRightUI.text = actor.name;
        } else {
            actorAvatarRightUI.gameObject.SetActive(false);
            actorNameRightUI.gameObject.SetActive(false);
        }
    }

    public void Continue(){
        if (typewriter.isTyping) typewriter.Skip();
        else Next();
    }
    public void Next(){
        if (currentDialog.index >= currentDialog.lines.Length-1){
            HideDialog();
            currentDialog.onCompleted?.Invoke();
        } else {
            var index = ++currentDialog.index;
            SetLine(index);
            currentDialog.onDialog?.Invoke(index);
        }
    }

    public void DisplayDialog(DialogObject dialog){
        ShowDialog();
        SetDialog(dialog);
    }
    public void ShowDialog(){
        gameObject.SetActive(true);
    }
    public void HideDialog(){
        gameObject.SetActive(false);
    }
}
