using System;
using System.Collections.Generic;
using UIGameDataManager;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class CardStatsTower : MonoBehaviour
{

    [Header("Character Info")]
    [SerializeField] Text m_LevelLabel;
    [SerializeField] Text m_CharacterLabel;
    [SerializeField] Text m_RarityLabel;
    [SerializeField] Text m_ClassLabel;

    [Header("Base Stats")]
    [SerializeField] Text m_BasePointsLife;
    [SerializeField] Text m_BasePointsDefense;
    [SerializeField] Text m_BasePointsAttack;
    [SerializeField] Text m_BasePointsAttackSpeed;
    [SerializeField] Text m_BasePointsSpecialAttack;
    [SerializeField] Text m_BasePointsCriticalHit;

    [Header("Visual Assets")]
    [SerializeField] Image ClassSprite;
    [SerializeField] Image raritySprite;
    [SerializeField] Image attackTypeSprite;

    [Header("Star System")]
    [SerializeField] ScrollStar m_ScrollStar;
    [SerializeField] GameObject StarPrefabs;
    [SerializeField] Transform StarHolder;


    [Header("Class Card")]
    // inventory, xp, and base data
    CharacterData m_CharacterData;


    // static base data from ScriptableObject
    CardCharacter m_BaseStats;

    // pairs icons with specific data types
    GameIconsSO m_GameIconsData;

    StatsTowerContainer statsContainer;

    [Header("Component Skill")]
    [SerializeField] Image[] m_SkillIcons = new Image[3];
    [SerializeField] SkillSO[] m_BaseSkills = new SkillSO[3];

    [SerializeField] List<StatReader> statReaders = new List<StatReader>();

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        statReaders[0].UpdateText();
        statReaders[1].UpdateText();
        statReaders[2].UpdateText();
        statReaders[3].UpdateText();
        statReaders[4].UpdateText();
        statReaders[5].UpdateText();
    }
    public void SetCardStatusTower(GameIconsSO gameIconData, CharacterData charData)
    {
        m_GameIconsData = gameIconData;
        m_CharacterData = charData;
        m_BaseStats = charData.CharacterBaseData;
    }
    // if the UI already exists, update the character data
    public void UpdateWindow(CharacterData charData)
    {
        m_CharacterData = charData;
        m_BaseStats = charData.CharacterBaseData;
        SetGameData();
        //ShowSkill(0);
    }
    void SetCharacterData()
    {
        m_CharacterData.Star = m_BaseStats.Star;

        //Debug.Log("Stat: Star - " + m_CharacterData.Star);
    }
    public void SetGameData()
    {
        Debug.Log("SetGameData()");
        //First
        SetStats();
        SetCharacterData();

        m_BaseSkills[0] = m_BaseStats.skill1;
        m_BaseSkills[1] = m_BaseStats.skill2;
        m_BaseSkills[2] = m_BaseStats.skill3;

        //// set level from CharacterData...

        int levelNumber = m_CharacterData.CurrentLevel;

        Debug.Log("Level: " + levelNumber);

        m_LevelLabel.text = levelNumber.ToString();

        // class/rarity/attackType
        m_CharacterLabel.text = m_BaseStats.cardAttack.ToString();
        m_RarityLabel.text = m_BaseStats.cardRare.ToString();
        m_ClassLabel.text = m_BaseStats.cardClass.ToString();

        // TO-DO: missing data validation
        ClassSprite.sprite = m_GameIconsData.GetCharacterClassIcon(m_BaseStats.cardClass);
        raritySprite.sprite = m_GameIconsData.GetRarityIcon(m_BaseStats.cardRare);
        attackTypeSprite.sprite = m_GameIconsData.GetAttackTypeIcon(m_BaseStats.cardAttack);

        //base points
        m_BasePointsLife.text = statReaders[0].GetCurrentValue().ToString();
        m_BasePointsDefense.text = statReaders[1].GetCurrentValue().ToString();
        m_BasePointsAttack.text = statReaders[2].GetCurrentValue().ToString();
        m_BasePointsAttackSpeed.text = statReaders[3].GetCurrentValue().ToString() + "/s";
        m_BasePointsSpecialAttack.text = statReaders[4].GetCurrentValue().ToString() + "/s";
        m_BasePointsCriticalHit.text = statReaders[5].GetCurrentValue().ToString();


        //Star
        // Check Star
        Debug.Log("Star: " + m_CharacterData.Star);

        foreach (Transform child in StarHolder)
        {
            Destroy(child.gameObject);

        }
        for(int i = 0; i< m_CharacterData.Star; i++)
        {
            Instantiate(StarPrefabs, StarHolder);
        }
           //Scroll
        m_ScrollStar.NumberStar = m_CharacterData.Star;

        //Debug.Log("m_ScrollStar : " + m_ScrollStar.numBerStar);

        //// bio
        //m_BioTitle.text = m_BaseStats.bioTitle;
        //m_BioText.text = m_BaseStats.bio;

        if (m_BaseStats.skill1 != null && m_BaseStats.skill2 != null && m_BaseStats.skill3 != null)
        {
            m_SkillIcons[0].sprite = (m_BaseStats.skill1.sprite);
            m_SkillIcons[1].sprite = (m_BaseStats.skill2.sprite);
            m_SkillIcons[2].sprite = (m_BaseStats.skill3.sprite);
        }
        else
        {
            Debug.LogWarning("CharStatsWindow.SetGameData: " + m_CharacterData.CharacterBaseData.nameCard +
                " missing Skill ScriptableObject(s)");
            return;
        }
    }
    public void SetGameDataStarUp()
    {
        //reset lv = 0


        int levelNumber = m_CharacterData.ResetLv();

        Debug.Log("m_CharacterData: " + m_CharacterData.CurrentLevel);
        m_LevelLabel.text = levelNumber.ToString();


    }
    public void SetStats()
    {
        if (statsContainer == null)
        {
            statsContainer = new StatsTowerContainer(m_BaseStats);

            //Debug.Log("Set Stats: "+ statsContainer);
        }
    }
    public void LevelUpStats()
    {
        uint statIncrease = m_CharacterData.GetStatForNextStar();

        statsContainer.basePointsAttack.statValue += CalculateStatWithRandomModifier(m_BaseStats.cardStat.Attack, statIncrease);
        //statsContainer.basePointsCriticalHit.statValue += CalculateStatWithRandomModifier(m_BaseStats.cardCharacter.CriticalHit, statIncrease);
        statsContainer.basePointsLife.statValue += CalculateStatWithRandomModifier(m_BaseStats.cardStat.Life, statIncrease);
        //statsContainer.basePointsDefense.statValue += CalculateStatWithRandomModifier(m_BaseStats.cardCharacter.Defense, statIncrease);
        statsContainer.basePointsAttackSpeed.statValue -= GetStatValue(m_BaseStats.cardStat.AttackSpeed);
        statsContainer.basePointsSpecialAttack.statValue += CalculateStatWithRandomModifier(m_BaseStats.cardStat.SpecialAttack, statIncrease);
    }

    private int CalculateStatWithRandomModifier(BaseStat stat, uint statIncrease)
    {
        float randomModifier = UnityEngine.Random.Range(0f, 1f);
        return Mathf.RoundToInt(stat.baseStatModifier.Evaluate(randomModifier) * (GetStatValue(stat) + (int)statIncrease));
    }

    public float GetStatValue(BaseStat stat)
    {
        // Hàm này trả về giá trị của baseStatValue của một BaseStat
        return stat.baseStatValue;
    }
    public Stat GetStat(StatKeys statName)
    {
        switch (statName)
        {
            case StatKeys.Attack:
                return statsContainer.basePointsAttack;
            case StatKeys.CriticalHit:
                return statsContainer.basePointsCriticalHit;
            case StatKeys.Life:
                return statsContainer.basePointsLife;
            case StatKeys.Defense:
                return statsContainer.basePointsDefense;
            case StatKeys.AttackSpeed:
                return statsContainer.basePointsAttackSpeed;
            case StatKeys.SpecialAttack:
                return statsContainer.basePointsSpecialAttack;
            default:
                return statsContainer.basePointsLife;
        }
    }

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
    //public Get
}
