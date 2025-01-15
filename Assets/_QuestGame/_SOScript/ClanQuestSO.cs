using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum ClanQuestType { InitiationTrial, ClanBond, GuardiansChallenge, SecretsoftheElders, TrialofValor, ForgeofLegends, RivalsDuel, SacredHunt, ClashofHeroes} //.vvv

[CreateAssetMenu(fileName = "New Clan Quest", menuName = "Quest/ClanQuest", order = 2)]
public class ClanQuestSO : QuestAbstractSO
{
    [Header("Clan Quest Specifics")]
    public bool isActive;
    public GuildType guildType;
    public ClanQuestType clanQuestType;
    public static QuestAbstractSO[] GetQuestsByGuildType(ClanQuestSO[] quests, GuildType _guildType)
    {
        // Return only the quests where the GuildType matches the provided value
        return quests.Where(quest => quest.guildType == _guildType).ToArray();
    }
}