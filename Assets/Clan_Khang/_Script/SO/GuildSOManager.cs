using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Guild/GuildSOManager", menuName = "UIGuild/GuildSOManager", order = 2)]
public class GuildSOManager : ScriptableObject
{
    public List<GuildSO> listGuilds;
    public GuildSO GuildJoined;

    [SerializeField] private GuildDefaultStatsSO GuildStatsDefault;
    public GuildDefaultStatsSO GuildAbilitySO => GuildStatsDefault;

    const string resPathGuildSO = "Guild/GuildSO";

    private void OnEnable()
    {
        LoadAllGuildSO();
        LoadDataCard();
    }

    private void OnApplicationQuit()
    {
        listGuilds.Clear();
    }

    private void LoadAllGuildSO()
    {
        listGuilds.Clear();
        GuildSO[] loadGuilds = Resources.LoadAll<GuildSO>(resPathGuildSO);
        listGuilds = new List<GuildSO>(loadGuilds);

        Debug.Log("Loaded " + listGuilds.Count + " Guild ScriptableObjects from " + resPathGuildSO);
    }

    private void LoadDataCard()
    {
        // Chỉ sắp xếp danh sách nếu cần thiết (ví dụ: nếu dữ liệu thay đổi thường xuyên)
        listGuilds = SortGuildByID(listGuilds);

        LoadGuildJoinedState();
        SetJoinedGuild();
    }

    private List<GuildSO> SortGuildByID(List<GuildSO> originalList)
    {
        // Nếu không cần thiết phải sắp xếp mỗi lần, có thể bỏ qua việc này
        return originalList.OrderBy(x => x.id).ToList();
    }

    private void SetJoinedGuild()
    {
        // Tìm guild đã tham gia trong danh sách
        GuildJoined = listGuilds.FirstOrDefault(guild => guild.Joined);
    }

    public void IsActiveJoined(GuildSO guildSO)
    {
        var currentGuild = listGuilds.FirstOrDefault(guild => guild.Joined);
        if (currentGuild != null)
        {
            currentGuild.Joined = false;
        }

        guildSO.Joined = true;
        GuildJoined = guildSO;

        // Lưu trạng thái của tất cả guilds sau khi thay đổi
        SaveGuildState();
    }

    private void SaveGuildState()
    {
        // Lưu trạng thái Joined của tất cả guilds vào PlayerPrefs
        foreach (var guild in listGuilds)
        {
            PlayerPrefs.SetInt(guild.name + "_Joined", guild.Joined ? 1 : 0);
        }
        PlayerPrefs.Save();  // Chỉ gọi Save một lần sau khi tất cả đã được lưu
    }

    private void LoadGuildJoinedState()
    {
        // Tải trạng thái Joined từ PlayerPrefs cho tất cả guilds
        foreach (var guild in listGuilds)
        {
            int joinedState = PlayerPrefs.GetInt(guild.name + "_Joined", 0);
            guild.Joined = joinedState == 1;
        }
    }
}
