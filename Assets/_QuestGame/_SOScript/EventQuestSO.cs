using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum EventQuestType
{
    SeasonalCelebration,  // A special event quest related to a seasonal celebration (e.g., Christmas, Halloween).
    TournamentChallenge,  // A competitive event where players compete for rewards or ranking.
    LimitedTimeAdventure, // A time-limited event that unlocks new adventures or areas to explore.
    WorldBossEvent,       // An event where players must team up to defeat a world boss.
    FestivalOfHeroes      // An event focused on celebrating the heroes of the game with special quests and rewards.
}


[CreateAssetMenu(fileName = "New Event Quest", menuName = "Quest/EventQuest", order = 4)]
public class EventQuestSO : QuestAbstractSO
{
    [Header("PVPQuestSO Quest Specifics")]
    public EventQuestType eventQuestType;
}