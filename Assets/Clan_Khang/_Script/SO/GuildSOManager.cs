using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Guild/GuildSOManager", menuName = "UIGuild/GuildSOManager", order = 2)]
public class GuildSOManager : ScriptableObject
{
    public List<GuildSO> listGuilds = new List<GuildSO>();
    public GuildSO GuildJoined;

    [SerializeField] GuildDefaultStatsSO GuildStatsDefault;
    public GuildDefaultStatsSO GuildAbilitySO => GuildStatsDefault;
    private void OnEnable()
    {
        LoadAllCardTowerScriptableObjects();
        LoadDataCard();
    }

    private void OnApplicationQuit()
    {
        listGuilds.Clear();
    }

    private void LoadAllCardTowerScriptableObjects()
    {
        string resPath = "Guild/GuildSO";
        GuildSO[] loadGuilds = Resources.LoadAll<GuildSO>(resPath);
        listGuilds = new List<GuildSO>(loadGuilds);
        Debug.Log("Loaded " + listGuilds.Count + " Card ScriptableObjects from " + resPath);
    }

    private void LoadDataCard()
    {
        listGuilds = SortGuildByID(listGuilds);
        SetJoinedGuild();
    }

    List<GuildSO> SortGuildByID(List<GuildSO> originalList)
    {
        return originalList.OrderBy(x => x.id).ToList();
    }

    private void SetJoinedGuild()
    {
        foreach (var guild in listGuilds)
        {
            if (guild.Joined)
            {
                GuildJoined = guild;
                break; // Nếu đã tìm thấy guild đã tham gia, thoát khỏi vòng lặp
            }
        }
    }
    public void IsActiveJoined(GuildSO guildSO)
    {
        foreach (var guild in listGuilds)
        {
            if (guild.Joined)
            {
                GuildJoined = guildSO;
                guild.Joined = false;
                guildSO.Joined = true;
                break; //Pause For
            }
        }
    }
}
