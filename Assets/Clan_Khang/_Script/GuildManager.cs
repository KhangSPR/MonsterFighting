using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    private static GuildManager instance;
    public static GuildManager Instance => instance;

    public GuildSO[] Guilds;
    public GuildSO GuildJoined;

    [SerializeField] private GuildDefaultStatsSO GuildStatsDefault;
    public GuildDefaultStatsSO GuildAbilitySO => GuildStatsDefault;

    public static Action OnGuildJoined;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only 1 GuildManager Warning");
        }
        instance = this;
    }
    private void Start()
    {
        LoadDataCard();
    }

    private void Update()
    {
        // Lắng nghe phím "Z" để thoát guild
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ResetJoinedGuild(); // Thực hiện việc reset khi nhấn phím Z
        }
    }

    public void LoadDataCard()
    {
        LoadGuildsFromResources();

        // Sắp xếp danh sách Guilds theo SortOrder
        Guilds = SortGuildByID(Guilds.ToList());

        // Tải trạng thái Guild đã tham gia
        LoadGuildJoinedState();

        // Cập nhật Guild đã tham gia
        SetJoinedGuild();
    }

    private void LoadGuildsFromResources()
    {
        Guilds = null;
        // Tải tất cả các GuildSO từ Resources/Guilds
        var loadedGuilds = Resources.LoadAll<GuildSO>("Guild/GuildSO");
        if (loadedGuilds != null && loadedGuilds.Length > 0)
        {
            Guilds = loadedGuilds;
        }
        else
        {
            Debug.LogWarning("No GuildSO found in Resources/Guilds");
        }
    }

    private GuildSO[] SortGuildByID(List<GuildSO> originalList)
    {
        return originalList.OrderBy(guild => guild.Cost).ToArray();
    }

    private void SetJoinedGuild()
    {
        GuildJoined = Guilds.FirstOrDefault(guild => guild.Joined);
        if (GuildJoined == null)
        {
            //Debug.LogWarning("No joined guild found.");
        }
    }

    public void IsActiveJoined(GuildSO guildSO)
    {
        if (guildSO == null)
        {
            Debug.LogWarning("GuildSO is null.");
            return;
        }

        // Bỏ trạng thái Joined của Guild hiện tại
        var currentGuild = Guilds.FirstOrDefault(guild => guild.Joined);
        if (currentGuild != null)
        {
            currentGuild.Joined = false;
        }

        // Cập nhật trạng thái Joined cho Guild mới
        guildSO.Joined = true;
        GuildJoined = guildSO;

        OnGuildJoined?.Invoke();
        // Lưu trạng thái
        SaveGuildState();
    }

    private void SaveGuildState()
    {
        foreach (var guild in Guilds)
        {
            PlayerPrefs.SetInt(guild.name + "_Joined", guild.Joined ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadGuildJoinedState()
    {
        foreach (var guild in Guilds)
        {
            int joinedState = PlayerPrefs.GetInt(guild.name + "_Joined", 0);
            guild.Joined = joinedState == 1;
        }
    }

    public GuildSO FindGuildByID(string id)
    {
        return Guilds.FirstOrDefault(guild => guild.ID == id);
    }

    // Hàm Reset Guild và lưu lại trạng thái không gia nhập nữa
    private void ResetJoinedGuild()
    {
        if (GuildJoined != null)
        {
            // Đặt trạng thái Guild hiện tại không còn gia nhập nữa
            GuildJoined.Joined = false;
            // Lưu lại trạng thái
            SaveGuildState();

            // Đặt GuildJoined là null
            GuildJoined = null;

            Debug.Log("You have exited from the guild.");
        }
    }
}
