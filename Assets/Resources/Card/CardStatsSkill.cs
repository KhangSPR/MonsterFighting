using DG.Tweening;
using System;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardStatsSkill : MonoBehaviour
{

    [Header("Scripts Base")]
    [SerializeField] CardStatsTower cardStatsTower;

    [SerializeField]  int m_ActiveIndex;
    [SerializeField] Image m_ActiveFrame;

    [Header("Panel Skill")]
    [SerializeField] Text m_Skill;
    [SerializeField] Text m_Category;
    [SerializeField] Text m_Tier;
    [SerializeField] TMP_Text m_Damage;
    [SerializeField] Text m_NextTier;

    [Header("Button Skill")]
    [SerializeField] Button m_NextSkillButton;
    [SerializeField] Button m_LastSkillButton;

    [SerializeField] Button m_Skill1;
    [SerializeField] Button m_Skill2;
    [SerializeField] Button m_Skill3;

    [Header("Card Locked")]
    [SerializeField] GameObject[] ObjLocks = new GameObject[2];
    [SerializeField] Sprite m_lockImage;
    Color m_Color = new Color(0.6f, 0.6f, 0.58f, 100 / 255f);





    float timeToNextClick = 0f;
    const float clickCooldown = 0.2f;


    private void OnEnable()
    {
        //InitializeSkillMarker();
        //ShowSkill(0);

        SetUpLock(cardStatsTower.GetCharacterData());
    }
    private void Start()
    {
        ShowSkillLv0();

        m_LastSkillButton.onClick.AddListener(SelectLastSkill);
        m_NextSkillButton.onClick.AddListener(SelectNextSkill);

        m_Skill1.onClick.AddListener(SelectSkill);
        m_Skill2.onClick.AddListener(SelectSkill);
        m_Skill3.onClick.AddListener(SelectSkill);


    }
    void SetUpLock(CharacterData characterData)
    {
        if (characterData == null) return;

        Color lockedColor = m_Color;
        int[] skillIndicesToLock = { 1, 2 };
        int[] requiredStarForLock = { 2, 5 };


        for (int i = 0; i < skillIndicesToLock.Length; i++)
        {
            // Check if the index is within the bounds of requiredStarForLock array
            if (characterData.Star < requiredStarForLock[i])
            {
                cardStatsTower.GetSkillIcons()[skillIndicesToLock[i]].color = lockedColor;
                
                ObjLocks[i].SetActive(true);
            }
        }
        //Debug.Log("LEVEL: " + characterData.Star);
        //if (characterData.Star == 5)
        //{
        //    m_NextTier.text = "";
        //}
    }
    void SelectLastSkill()
    {
        if (Time.time < timeToNextClick)
            return;

        timeToNextClick = Time.time + clickCooldown;

        // only select when clicking directly on the visual element
        m_ActiveIndex--;
        if (m_ActiveIndex < 0)
        {
            m_ActiveIndex = 2;
        }
        ShowSkill(m_ActiveIndex);
        //AudioManager.PlayDefaultButtonSound();
    }

    void SelectNextSkill()
    {
        if (Time.time < timeToNextClick)
            return;

        timeToNextClick = Time.time + clickCooldown;
        m_ActiveIndex++;
        if (m_ActiveIndex > 2)
        {
            m_ActiveIndex = 0;
        }
        ShowSkill(m_ActiveIndex);
        //AudioManager.PlayDefaultButtonSound();
    }

    void SelectSkill()
    {
        Debug.Log("IsActive");
        for (int i = 0; i < cardStatsTower.GetSkillIcons().Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == cardStatsTower.GetSkillIcons()[i].gameObject)
            {
                int index = i;
                ShowSkill(index);
                Debug.Log("SelectSkill method is called from skill icon " + index);
                //AudioManager.PlayAltButtonSound();
                break;
            }
        }
    }

    void ShowSkillLv0()
    {
        SkillSO skill = cardStatsTower.GetBaseSkills()[0];
        if (skill != null)
        {
            m_ActiveIndex = 0;
            SetSkillData(skill);
        }
    }
    void ShowSkill(int index)
    {
        SkillSO skill = cardStatsTower.GetBaseSkills()[index];
        if (skill != null)
        {
            SetSkillData(skill);
            MarkTargetElement(cardStatsTower.GetSkillIcons()[index].rectTransform, 300);
            m_ActiveIndex = index;
        }
    }

    // show the description for a given skill
    void SetSkillData(SkillSO skill)
    {
        m_Skill.text = skill.skillName;
        m_Category.text = skill.GetCategoryText();
        m_Tier.text = skill.GetTierText(m_ActiveIndex);
        m_Damage.text = skill.GetDamageText();
        m_NextTier.text = skill.GetNextTierStarText((int)cardStatsTower.GetCharacterData().Star);
    }
    // set up the active frame after the layout builds
    void InitializeSkillMarker()
    {
        // set its position over the first icon
        MarkTargetElement(cardStatsTower.GetSkillIcons()[0].rectTransform, 0);
    }
    void MarkTargetElement(RectTransform targetElement, int duration = 200)
    {
        // target element, converted into the root space of the Active Frame
        Vector3 targetInRootSpace = ElementInRootSpace(targetElement, m_ActiveFrame.rectTransform);

        // padding offset
        Vector3 offset = new Vector3(0, 17, 0f); //Padding IMG

        m_ActiveFrame?.rectTransform.DOAnchorPos(targetInRootSpace - offset, duration / 1000f);
    }

    // convert target RectTransform into another RectTransform's parent space 
    Vector3 ElementInRootSpace(RectTransform targetElement, RectTransform newElement)
    {
        Vector2 targetInWorldSpace = targetElement.TransformPoint(targetElement.rect.center);

        RectTransform newRoot = newElement.parent.GetComponent<RectTransform>();

        return newRoot.InverseTransformPoint(targetInWorldSpace);
    }


}
