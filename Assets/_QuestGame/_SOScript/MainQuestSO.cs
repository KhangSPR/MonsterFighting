using UnityEngine;

[CreateAssetMenu(fileName = "New Main Quest", menuName = "Quest/MainQuest", order = 1)]
public class MainQuestSO : QuestAbstractSO
{
    [Header("Main Quest Specifics")]
    public int requiredLevel;
    public enum lvType { HeroLv1, HeroLv2, HeroLv3, HeroLv4, HeroLv5, HeroLv6, HeroLv7, HeroLv8, HeroLv9, HeroLv10 }
    public lvType LVType;
}
