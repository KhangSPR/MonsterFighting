using UnityEngine;

[CreateAssetMenu(fileName = "New Clan Quest", menuName = "Quest/ClanQuest")]
public class ClanQuestSO : QuestAbstractSO
{
    [Header("Clan Quest Specifics")]
    public bool isActive;
}
