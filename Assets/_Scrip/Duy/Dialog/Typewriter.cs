using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : MonoBehaviour {
    private TextMeshProUGUI textBox;

    private Coroutine typewriterCoroutine;
    private WaitForSeconds typewriterDelay;
    private WaitForSeconds textCompleteDelay;

    private int currentVisibleCharacterIndex;

    [HideInInspector] public bool isTyping;
    [SerializeField] float textSpeed;
    [SerializeField] float eventDelay;

    public event Action onTextCompleted;

    private void Awake(){
        textBox = GetComponent<TextMeshProUGUI>();
    }

    private void Start(){
        isTyping = false;
        typewriterDelay = new(1f/textSpeed);
        textCompleteDelay = new(eventDelay);
    }

    public void Set(string text){
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        textBox.text = text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typewriterCoroutine = StartCoroutine(Type());
    }

    public void Skip(){
        if (!isTyping) return;
        StopCoroutine(typewriterCoroutine);
        textBox.maxVisibleCharacters = textBox.textInfo.characterCount;
        onTextCompleted?.Invoke();
        onTextCompleted = null;
        isTyping = false;
    }

    private IEnumerator Type(){
        isTyping = true;
        var textInfo = textBox.textInfo;
        while (currentVisibleCharacterIndex < textInfo.characterCount+1){
            textBox.maxVisibleCharacters++;
            yield return typewriterDelay;
            currentVisibleCharacterIndex++;
        }
        if (onTextCompleted != null){
            yield return textCompleteDelay;
            onTextCompleted.Invoke();
            onTextCompleted = null;
        }
        isTyping = false;
    }
}
