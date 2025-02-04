using UnityEngine;

public enum GuildType
{
    //Hero,
    Hunters,
    ArcaneMystics,
    SilverArrows
}

[CreateAssetMenu(fileName = "Assets/Resources/Guild/GuildSO", menuName = "UIGuild/GuildSO", order = 1)]
public class GuildSO : ScriptableObject
{
    [Header("public variable")]
    public string ID;
    public int SortOrder; // Biến dùng để sắp xếp

    public uint Cost;
    public string GuildName;
    [TextArea] public string GuildDescription;

    public Sprite GuildIcon;
    public Sprite GuildImage;
    public bool Joined = false;
    public GuildType guildType;

    public GuildMoreStatsAbilitySO abilitySO;

}