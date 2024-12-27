using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : MonoBehaviour {
    private TextMeshProUGUI textBox;

    private Coroutine typewriterCoroutine;
    private WaitForSeconds typewriterDelay;

    private int currentVisibleCharacterIndex;

    public bool isTyping;
    [SerializeField] float speed;

    private void Awake(){
        textBox = GetComponent<TextMeshProUGUI>();
    }

    private void Start(){
        isTyping = false;
        typewriterDelay = new(1f/speed);
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
        isTyping = false;
    }

    private IEnumerator Type(){
        isTyping = true;
        var textInfo = textBox.textInfo;
        while (currentVisibleCharacterIndex < textInfo.characterCount+1){
            // var character = textInfo.characterInfo[currentVisibleCharacterIndex].character;
            textBox.maxVisibleCharacters++;
            yield return typewriterDelay;
            currentVisibleCharacterIndex++;
        }
        isTyping = false;
    }
}
