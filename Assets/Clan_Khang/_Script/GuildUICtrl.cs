using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildUICtrl : MonoBehaviour
{
    [Header("Holder")]
    [SerializeField] Transform GuildChoosing;
    [SerializeField] Transform GuildButton;

    [Header("Prefab")]
    [SerializeField] Transform PrefabGuildChoosing;
    [SerializeField] Transform PrefabGuildButton;

    [Header("Script")]
    [SerializeField] Swipe swipe;
    public Swipe Swipe=> swipe;
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
        swipe.GuildChoosings.Clear();
    }
    void InstanceGuildChoosing()
    {
        foreach(Transform child in GuildChoosing)
        {
            Destroy(child.gameObject);
        }
        //foreach(GuildSO guildSO in GuildManager.Instance.GuildSOManager.listGuilds)
        //{
        //    GameObject newguildChossing = Instantiate(PrefabGuildChoosing.gameObject, GuildChoosing);

        //    GuildChoosing guildChoosing = newguildChossing.GetComponent<GuildChoosing>();

        //    guildChoosing.SetUI(guildSO,this);
        //    swipe.GuildChoosings.Add(guildChoosing);
        //}
        foreach (GuildSO guildSO in GuildManager.Instance.GuildSOManager.listGuilds)
        {
            GameObject newguildChossing = Instantiate(PrefabGuildChoosing.gameObject, GuildChoosing);

            GuildChoosing guildChoosing = newguildChossing.GetComponent<GuildChoosing>();

            guildChoosing.SetUI(guildSO, this);
            swipe.GuildChoosings.Add(guildChoosing);
        }
        for (int i = 0; i<3;i++)
        {
            GameObject newguildChossing = Instantiate(PrefabGuildChoosing.gameObject, GuildChoosing);

            GuildChoosing guildChoosing = newguildChossing.GetComponent<GuildChoosing>();

            guildChoosing.SetUI(GuildSO, this);

        }
    }
    [SerializeField] GuildSO GuildSO;
    void InstanceGuildButton()
    {
        foreach (Transform child in GuildButton)
        {
            Destroy(child.gameObject);
        }
        foreach (GuildSO guildSO in GuildManager.Instance.GuildSOManager.listGuilds)
        {
            GameObject guildButton = Instantiate(PrefabGuildButton.gameObject, GuildButton);

            guildButton.GetComponent<ButtonGuild>().SetUI(guildSO, swipe);
        }
    }
    public void IsActivePurchase()
    {
        // Tạo từ điển để lưu trữ GuildChoosings theo GuildSO
        Dictionary<GuildSO, GuildChoosing> guildDictionary = new Dictionary<GuildSO, GuildChoosing>();

        // Điền từ điển với các giá trị từ swipe.GuildChoosings
        foreach (var guild in swipe.GuildChoosings)
        {
            guildDictionary[guild.GuildSO] = guild;
        }

        // Duyệt qua các guild trong GuildSOManager.listGuilds
        foreach (GuildSO guildSO in GuildManager.Instance.GuildSOManager.listGuilds)
        {
            // Kiểm tra nếu GuildSO có trong từ điển
            if (guildDictionary.TryGetValue(guildSO, out GuildChoosing guild))
            {
                guild.SetUI(guildSO, this);
            }
        }
    }


}
