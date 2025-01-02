using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum PVPQuestType
{
    InitiationTrial,     // A basic trial to initiate the player into PvP combat.
    ClanBond,            // PvP quest to strengthen the bond within a clan.
    RivalsDuel,          // A one-on-one duel against a rival player.
    TrialofValor,        // A test of bravery and valor in PvP combat.
    ClashofHeroes        // A large-scale battle between multiple heroes in PvP.
}

[CreateAssetMenu(fileName = "New PVP Quest", menuName = "Quest/PVPQuest")]
public class PVPQuestSO : QuestAbstractSO
{
    [Header("PVPQuestSO Quest Specifics")]
    public PVPQuestType pvpQuestType;
}