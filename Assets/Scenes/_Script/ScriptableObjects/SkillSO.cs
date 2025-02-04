using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



// each character has up to three special attack skills
[CreateAssetMenu(fileName = "Assets/Resources/GameData/Skills/SkillGameData", menuName = "GameData/Skill", order = 2)]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public float damage;
    // time in seconds
    public float coutdownTimer;


    // icon for character screen
    public Sprite sprite;

    public bool skillUnlock;

    public float manaRequirement;
    public float distanceAttack;
    public int countSkill;
    public string GetTierText(int m_ActiveIndex)
    {
        return "Tier "+ m_ActiveIndex;
    }

    public string GetNextTierStarText()
    {
        return "";
    }
    // Name Skill
    [TextArea] public string textTemplate;

    public string GetDamageText()
    {
        return textTemplate;
    }
    public ISkill GetSkillInstance()
    {
        return SkillFactory.CreateSkill(skillName);
    }
}
