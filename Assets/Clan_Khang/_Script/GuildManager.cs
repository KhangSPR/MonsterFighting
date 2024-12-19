using UnityEngine;

public class GuildManager : MonoBehaviour
{
    private static GuildManager instance;
    public static GuildManager Instance => instance;

    [SerializeField] GuildSOManager guildSOManager;
    public GuildSOManager GuildSOManager => guildSOManager;
    private void Awake()
    {
        if (GuildManager.instance != null)
        {
            Debug.LogError("Only 1 GuildManager Warning");
        }
        GuildManager.instance = this;
    }

}
