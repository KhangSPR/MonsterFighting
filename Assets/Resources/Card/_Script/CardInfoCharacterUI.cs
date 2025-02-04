using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoCharacterUI : MonoBehaviour
{
    [SerializeField] Image m_FrameAvatar;
    [SerializeField] Image m_FrameName;
    [SerializeField] Image m_Avatar;
    [SerializeField] TMP_Text m_NameText;
    [SerializeField] Button m_nextCharacterBtn;
    [SerializeField] Button m_previousCharacterBtn;

    CharacterData m_CharacterData;

    public static Action OnNextCharacter;
    public static Action OnPreviousCharacter;
    private void Start()
    {
        m_nextCharacterBtn.onClick.AddListener(OnClickNext);
        m_previousCharacterBtn.onClick.AddListener(OnClickPrevious);
    }
    private void OnClickNext()
    {
        OnPreviousCharacter?.Invoke();
    }
    private void OnClickPrevious()
    {
        OnNextCharacter?.Invoke();
    }
    public void SetInfo(CardCharacter cardCharacter)
    {
        m_FrameAvatar.sprite = cardCharacter.background;
        m_FrameName.sprite = cardCharacter._frameCardName;
        m_Avatar.sprite = cardCharacter.frame;
        m_NameText.text = cardCharacter.nameCard;
    }
}
