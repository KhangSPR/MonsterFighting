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

        guildSOManager.LoadAllGuildSO();
        guildSOManager.LoadDataCard();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs deleted successfully!");
        }
    }


}
