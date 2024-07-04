using DG.Tweening;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardStatsSkill : MonoBehaviour
{
    [SerializeField] int m_ActiveIndex;
    [SerializeField] Image m_ActiveFrame;

    [Header("Component Skill")]
    [SerializeField] Image[] m_SkillIcons;
    [SerializeField] SkillSO[] m_BaseSkills;

    [Header("Panel Skill")]
    [SerializeField] TMP_Text m_TitleSkill;
    [SerializeField] TMP_Text m_Tier;
    [SerializeField] TMP_Text m_Description;

    [Header("Button Skill")]
    [SerializeField] Button m_NextSkillButton;
    [SerializeField] Button m_LastSkillButton;

    [SerializeField] Button m_Skill1;
    [SerializeField] Button m_Skill2;

    [Space]
    [SerializeField] TabButtonUI m_TabButtonUI;

    [SerializeField] Sprite m_lockImage;
    Color lockedColor = new Color(0.6f, 0.6f, 0.6f, 0.6f);
    Color defautColor = new Color(1f, 1f, 1f, 1f);

    // inventory, xp, and base data
    CharacterData m_CharacterData;

    // static base data from ScriptableObject
    CardCharacter m_BaseStats;

    // pairs icons with specific data types
    GameIconsSO m_GameIconsData;

    float timeToNextClick = 0f;
    const float clickCooldown = 0.2f;

    [SerializeField] GameObject objSkill;
    [SerializeField] Transform panelSkill;
    [SerializeField] Image m_EyeHide;
    private void Start()
    {
        m_LastSkillButton.onClick.AddListener(SelectLastSkill);
        m_NextSkillButton.onClick.AddListener(SelectNextSkill);


    }
    private void OnEnable()
    {
        if (m_Skill1 != null)
        {
            m_Skill1.onClick.AddListener(SelectSkill);
        }

        if (m_Skill2 != null)
        {
            m_Skill2.onClick.AddListener(SelectSkill);
        }
    }
    private void SetTabButtonUISkill()
    {
        if (m_TabButtonUI != null)
        {
            // Gọi sự kiện onClick của Button
            m_TabButtonUI.uiButton.onClick.Invoke();
        }
    }

    #region Get Set 
    public Image[] GetSkillIcons()
    {
        return m_SkillIcons;
    }

    public SkillSO[] GetBaseSkills()
    {
        return m_BaseSkills;
    }

    public CharacterData GetCharacterData()
    {
        return m_CharacterData;
    }
    public CardCharacter GetCardTower()
    {
        return m_BaseStats;
    }
    #endregion

    public void SetCardStatusTower(GameIconsSO gameIconData, CharacterData charData)
    {
        m_GameIconsData = gameIconData;
        m_CharacterData = charData;
        m_BaseStats = charData.CharacterBaseData;
    }

    public void UpdateWindow(CharacterData charData)
    {
        m_CharacterData = charData;
        m_BaseStats = charData.CharacterBaseData;
        SetGameData();
    }

    public void SetGameData()
    {
        Debug.Log("SetGameData()");
        SetBaseSkill(m_CharacterData.CharacterBaseData.rarityCard);
        SetTabButtonUISkill();

        m_ActiveIndex = 0;
    }

    private void SetBaseSkill(RarityCard rarityCard)
    {
        switch (rarityCard)
        {
            case RarityCard.D:
            case RarityCard.C:
                CreateBase1Skill();
                break;
            case RarityCard.B:
            case RarityCard.A:
            case RarityCard.S:
            case RarityCard.SS:
                CreateBase2Skill();
                break;
        }
    }

    private void CreateBase1Skill()
    {
        m_EyeHide.gameObject.SetActive(false);

        m_BaseSkills = new SkillSO[1];
        m_BaseSkills[0] = m_BaseStats.skill1;

        m_SkillIcons = new Image[1];

        m_NextSkillButton.gameObject.SetActive(false);
        m_LastSkillButton.gameObject.SetActive(false);

        //Instance Skill
        InstanceSkill(m_BaseSkills);

        //Select Skill
        ShowSkillLv0();
    }

    private void CreateBase2Skill()
    {
        m_EyeHide.gameObject.SetActive(false);


        m_BaseSkills = new SkillSO[2];
        m_BaseSkills[0] = m_BaseStats.skill1;
        m_BaseSkills[1] = m_BaseStats.skill2;


        m_SkillIcons = new Image[2];



        m_NextSkillButton.gameObject.SetActive(true);
        m_LastSkillButton.gameObject.SetActive(true);

        //Instance Skill
        InstanceSkill(m_BaseSkills);

        //Select Skill
        ShowSkillLv0();
    }
    private void InstanceSkill(SkillSO[] skillSO)
    {
        foreach (Transform child in panelSkill)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < skillSO.Length; i++)
        {
            GameObject newSkill = Instantiate(objSkill, panelSkill);


            m_SkillIcons[i] = newSkill.GetComponent<Image>();



            if (skillSO[i] != null && skillSO[i].skillUnlock == true)
            {
                newSkill.transform.Find("Lock").gameObject.SetActive(false);
                newSkill.transform.GetComponent<Image>().sprite = skillSO[i].sprite;
                newSkill.transform.GetComponent<Image>().color = defautColor;
                if(i==0)
                {
                    m_Skill1 = newSkill.transform.GetComponent<Button>();
                }
                else
                {
                    m_Skill2 = newSkill.transform.GetComponent<Button>();
                }
            }
            else
            {
                newSkill.transform.Find("Lock").gameObject.SetActive(true);
                newSkill.transform.GetComponent<Image>().sprite = skillSO[i].sprite;
                newSkill.transform.GetComponent<Image>().color = lockedColor;
                if (i == 0)
                {
                    m_Skill1 = newSkill.transform.GetComponent<Button>();
                }
                else
                {
                    m_Skill2 = newSkill.transform.GetComponent<Button>();
                }
            }

        }
    }
    #region Select Skill Animation
    private void SelectLastSkill()
    {
        if (Time.time < timeToNextClick)
            return;

        timeToNextClick = Time.time + clickCooldown;

        m_ActiveIndex--;
        if (m_ActiveIndex < 0)
        {
            m_ActiveIndex = m_BaseSkills.Length - 1;
        }
        ShowSkill(m_ActiveIndex);
    }

    private void SelectNextSkill()
    {
        if (Time.time < timeToNextClick)
            return;

        timeToNextClick = Time.time + clickCooldown;

        m_ActiveIndex++;
        if (m_ActiveIndex >= m_BaseSkills.Length)
        {
            m_ActiveIndex = 0;
        }
        ShowSkill(m_ActiveIndex);
    }

    private void SelectSkill()
    {
        Debug.Log("IsActive");
        for (int i = 0; i < m_SkillIcons.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == m_SkillIcons[i].gameObject)
            {
                int index = i;
                ShowSkill(index);
                Debug.Log("SelectSkill method is called from skill icon " + index);
                break;
            }
        }
    }
    #endregion

    private void ShowSkillLv0()
    {
        SkillSO skill = m_BaseSkills[0];
        if (skill != null)
        {
            Debug.Log("ShowSkillLv0");  

            SetSkillData(skill);  // Cập nhật dữ liệu kỹ năng
            if(m_BaseSkills.Length >1)
            {
                SetFrameSelect1();
            }
            else
            {
                SetFrameSelect2();
            }
        }
    }
    private void SetFrameSelect1()
    {
        m_ActiveFrame.rectTransform.anchoredPosition = new Vector2(-125f, 171f);
    }
    private void SetFrameSelect2()
    {
        m_ActiveFrame.rectTransform.anchoredPosition = new Vector2(0f, 171f);
    }
    private void ShowSkill(int index)
    {
        SkillSO skill = m_BaseSkills[index];
        if (skill != null)
        {
            m_ActiveIndex = index;
            SetSkillData(skill);
            MarkTargetElement(m_SkillIcons[index].rectTransform, 300);
        }
    }

    private void SetSkillData(SkillSO skill)
    {
        if(!skill.skillUnlock)
        {
            m_EyeHide.gameObject.SetActive(true);

            m_Description.text = "";
            m_Tier.text = skill.GetTierText(m_ActiveIndex + 1);
            m_TitleSkill.text = skill.skillName;

            return;
        }
        m_EyeHide.gameObject.SetActive(false);

        m_TitleSkill.text = skill.skillName;
        m_Tier.text = skill.GetTierText(m_ActiveIndex + 1);
        //m_NextTier.text = ;
        m_Description.text = skill.textTemplate;
    }
    private void InitializeSkillMarker()
    {
        if (m_SkillIcons.Length > 0 && m_SkillIcons[0] != null)
        {
            // Set its position over the first icon
            MarkTargetElement(m_SkillIcons[0].rectTransform, 0);
        }
        else
        {
            Debug.LogError("No skill icons available or first skill icon is null.");
        }
    }

    private void MarkTargetElement(RectTransform targetElement, int duration)
    {
        // target element, converted into the root space of the Active Frame
        Vector3 targetInRootSpace = ElementInRootSpace(targetElement, m_ActiveFrame.rectTransform);

        // padding offset
        Vector3 offset = new Vector3(0, 19, 0f); //Padding IMG

        m_ActiveFrame?.rectTransform.DOAnchorPos(targetInRootSpace - offset, duration / 1000f);
    }

    // convert target RectTransform into another RectTransform's parent space 
    private Vector3 ElementInRootSpace(RectTransform targetElement, RectTransform newElement)
    {
        Vector2 targetInWorldSpace = targetElement.TransformPoint(targetElement.rect.center);
        RectTransform newRoot = newElement.parent.GetComponent<RectTransform>();
        return newRoot.InverseTransformPoint(targetInWorldSpace);
    }
}
