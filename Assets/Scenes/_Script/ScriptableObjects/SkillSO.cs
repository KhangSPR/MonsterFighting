using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum SkillCategory
{
    Basic = 0,
    Intermediate = 1,
    Advanced = 2
}

// each character has up to three special attack skills
[CreateAssetMenu(fileName = "Assets/Resources/GameData/Skills/SkillGameData", menuName = "UIToolkitDemo/Skill", order = 3)]
public class SkillSO : ScriptableObject
{


    public string skillName;

    // tier: 0 = "basic", 1 = unlocked at level 3, 2 = unlocked at level 6
    public SkillCategory category;

    // damage applied over damage time
    public int damagePoints;

    // time in seconds
    public float damageTime;

    // icon for character screen
    public Sprite sprite;


    public int MinStar
    {
        get
        {
            switch (category)
            {
                case SkillCategory.Basic:
                    return 0;
                case SkillCategory.Intermediate:
                    return 2;
                case SkillCategory.Advanced:
                    return 5;
                default:
                    return 0;
            }
        }
    }

    public string GetCategoryText()
    {
        return (SkillCategory)category + ""; /*+ " attack";*/
    }

    public string GetTierText(int m_ActiveIndex)
    {
        if(m_ActiveIndex == 0 )
            return "Tier 1";
        if (m_ActiveIndex == 1)
            return "Tier 2";
        if (m_ActiveIndex == 2)
            return "Tier 3";
        else
            return "";
    }

    public string GetNextTierStarText(int star)
    {
        int minStar = MinStar;

        if (star >= minStar)
        {
            return "";
        }


        int nextTierStar = star < 2 ? 2 : star < 5 ?5:5; // Cập nhật logic tùy thuộc vào yêu cầu cụ thể của bạn

        return $"Next tier at requires {nextTierStar} stars.";
    }

    //public bool IsUnlocked(int star)
    //{
    //    return star > MinStar;
    //}

    //public string GetLockText()
    //{
    //    return "Unlocked at Level " + MinStar;
    //}
    // Name Skill
    [TextArea] public string textTemplate;

    //"Deals <color=#775027>" + damagePoints + " Damage</color> points to an enemy every <color=#775027>" + damageTime + " seconds</color>";
    public string GetDamageText()
    {
        return textTemplate;
    }

    //private string FormatColoredBeforeText(string text)
    //{
    //    string Trim = GetStringBeforeSpriteIndex(text);

    //    // Sử dụng regex để tìm tất cả các giá trị số trong chuỗi
    //    System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(Trim, @"\d+(\.\d+)?");

    //    // Lặp qua từng giá trị số và đổi màu
    //    foreach (System.Text.RegularExpressions.Match match in matches)
    //    {
    //        string numericValue = match.Value;
    //        string coloredNumericValue = "<color=#775027>" + numericValue + "</color>";
    //        Trim = Trim.Replace(numericValue, coloredNumericValue);
    //    }

    //    return Trim;
    //}
    //private string FormatColoredAfterText(string text)
    //{
    //    string Trim = GetStringAfterSpriteIndex(text);

    //    // Sử dụng regex để tìm tất cả các giá trị số trong chuỗi
    //    System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(Trim, @"\d+(\.\d+)?");

    //    // Lặp qua từng giá trị số và đổi màu
    //    foreach (System.Text.RegularExpressions.Match match in matches)
    //    {
    //        string numericValue = match.Value;
    //        string coloredNumericValue = "<color=#775027>" + numericValue + "</color>";
    //        Trim = Trim.Replace(numericValue, coloredNumericValue);
    //    }

    //    return Trim;
    //}
    //private string GetStringBeforeSpriteIndex(string input)
    //{
    //    // Sử dụng regex để tìm chuỗi trước <sprite index= >
    //    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, @"(<sprite index= \d+>)");

    //    TMPSprite = match.Success ? match.Value : "";

    //    // Trả về chuỗi trước <sprite index= > hoặc input nếu không tìm thấy
    //    return match.Success ? input.Replace(TMPSprite, "") : input;
    //}


    //public static string GetStringAfterSpriteIndex(string input)
    //{
    //    // Sử dụng regex để tìm chuỗi sau <sprite index= >
    //    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, @"<sprite index= \d+>(.*)$");

    //    // Trả về chuỗi sau <sprite index= > hoặc input nếu không tìm thấy
    //    return match.Success ? match.Groups[1].Value : input;
    //}



}
