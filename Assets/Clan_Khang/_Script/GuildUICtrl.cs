using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildUICtrl : MonoBehaviour
{
    [Header("Holder")]
    [SerializeField] Transform GuildChoosing;
    [SerializeField] Transform GuildButton;

    [Header("Prefab")]
    [SerializeField] GameObject PrefabGuildChoosing;
    [SerializeField] GameObject PrefabGuildButton;

    [Header("Script")]
    [SerializeField] Swipe swipe;
    public Swipe Swipe => swipe;
    [SerializeField] PurchaseGuild purchaseGuild;
    public PurchaseGuild PurchaseGuild => purchaseGuild;
    private void OnEnable()
    {

        InstanceGuildChoosing();

        InstanceGuildButton();


        swipe.UpdateButtonInteractivity();

    }
    private void OnDisable()
    {
        swipe.guildChoosings.Clear();
    }
    void InstanceGuildChoosing()
    {
        foreach (Transform child in GuildChoosing)
        {
            Destroy(child.gameObject);
        }
        foreach (GuildSO guildSO in GuildManager.Instance.Guilds)
        {
            GameObject newguildChossing = Instantiate(PrefabGuildChoosing, GuildChoosing);

            GuildChoosing guildChoosing = newguildChossing.GetComponent<GuildChoosing>();


            guildChoosing.SetUI(guildSO, this);
            swipe.guildChoosings.Add(guildChoosing);
        }
    }
    void InstanceGuildButton()
    {
        foreach (Transform child in GuildButton)
        {
            Destroy(child.gameObject);
        }
        foreach (GuildSO guildSO in GuildManager.Instance.Guilds)
        {
            GameObject guildButton = Instantiate(PrefabGuildButton, GuildButton);

            guildButton.GetComponent<ButtonGuild>().SetUI(guildSO, swipe);
        }
    }
    public void IsActivePurchase()
    {
        // Tạo từ điển để lưu trữ GuildChoosings theo GuildSO
        Dictionary<GuildSO, GuildChoosing> guildDictionary = new Dictionary<GuildSO, GuildChoosing>();

        // Điền từ điển với các giá trị từ swipe.GuildChoosings
        foreach (var guild in swipe.guildChoosings)
        {
            guildDictionary[guild.GuildSO] = guild;
        }

        // Duyệt qua các guild trong GuildSOManager.listGuilds
        foreach (GuildSO guildSO in GuildManager.Instance.Guilds)
        {
            // Kiểm tra nếu GuildSO có trong từ điển
            if (guildDictionary.TryGetValue(guildSO, out GuildChoosing guild))
            {
                guild.SetUI(guildSO, this);
            }
        }
    }


}