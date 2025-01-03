using System;
using UnityEngine;
using System.Linq;
using static MainQuestSO;
using static QuestInfoSO;


public class QuestManager : SaiMonoBehaviour
{
    // Singleton instance of QuestManager
    private static QuestManager _instance;
    public static QuestManager Instance => _instance;

    public static Action<Objective.Type, string> OnEnemyKilled;


    // Path to the folder containing quest ScriptableObjects
    private const string QuestMainFolderPath = "Quest/MainQuest";
    private const string ClanFolderPath = "Quest/ClanQuest";
    private const string PVPFolderPath = "Quest/PVPQuest";
    private const string QuestDesFolderPath = "Quest/QuestDes";


    // Array to hold loaded QuestAbstractSO objects
    private QuestAbstractSO[] _questListMain;
    public QuestAbstractSO[] QuestListMain => _questListMain;
    private QuestAbstractSO[] _questListClan;
    public QuestAbstractSO[] QuestListClan => _questListClan;
    private QuestAbstractSO[] _questListPVP;
    public QuestAbstractSO[] QuestListPVP => _questListPVP;
    [SerializeField]
    private QuestInfoSO[] _questInfos;
    public QuestInfoSO[] QuestInfos => _questInfos;

    // Ensures only one instance of QuestManager exists
    protected override void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("QuestManager != null");
            return;
        }
        _instance = this;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerManager.OnQuestUpdate += OnActiveReceivedQuestMain;
        GuildManager.OnGuildJoined += OnActiveReceivedQuestClan;
        //ADD Quest Action
        OnEnemyKilled += OnQuestObjective;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerManager.OnQuestUpdate -= OnActiveReceivedQuestMain;
        GuildManager.OnGuildJoined -= OnActiveReceivedQuestClan;
        //REMOVE Quest Action
        OnEnemyKilled -= OnQuestObjective;
    }
    protected override void Start()
    {
        base.Start();
        // Load quests from the specified folder
        _questListMain = LoadQuestsFromFolder(QuestMainFolderPath);
        _questListClan = LoadQuestsFromFolder(ClanFolderPath);
        _questListPVP = LoadQuestsFromFolder(PVPFolderPath);
        _questInfos = LoadQuestInfosFromFolder(QuestDesFolderPath);

        this.OnActiveReceivedQuestMain();
        this.OnActiveReceivedQuestClan();
    }
    private void OnQuestObjective(Objective.Type type, string id)
    {
        Debug.Log("OnQuestObjective: " + id);

        foreach (QuestInfoSO quest in _questInfos)
        {
            if (quest.isFinishQuest)
            {
                continue; // Bỏ qua các quest đã hoàn thành
            }

            foreach (Objective objective in quest.objectives)
            {
                if (objective.type == type && objective.ID == id) // Kiểm tra đúng type và id
                {
                    objective.UpdateObjectives(type, id);
                    return; // Thoát khỏi hàm ngay khi cập nhật xong
                }
            }
        }
    }

    private void OnActiveReceivedQuestMain()
    {
        if (_questListMain == null) return;

        foreach(var _quest in _questListMain)
        {
            if(_quest is MainQuestSO mainQuestSO)
            {
                if(mainQuestSO.requiredLevel <= PlayerManager.Instance.LvPlayer)
                {
                    _quest.isReceived = true;
                }
            }
        }
    }
    private void OnActiveReceivedQuestClan()
    {
        if (_questListClan == null) return;

        GuildSO guildJoined = GuildManager.Instance.GuildJoined;

        if (guildJoined == null) return;

        foreach (var _quest in _questListClan)
        {
            if (_quest is ClanQuestSO clanQuestSO)
            {
                if (clanQuestSO.guildType == guildJoined.guildType)
                {
                    _quest.isReceived = true;
                }
            }
        }
    }
    public QuestAbstractSO[] GetQuestsByLvType(Enum @enum)
    {
        if (@enum is lvType lvType)
        {
            return _questListMain.OfType<MainQuestSO>()
                                 .Where(quest => quest.LVType == lvType)
                                 .ToArray();
        }
        else if (@enum is ClanQuestType clanQuestType)
        {
            return _questListClan.OfType<ClanQuestSO>() 
                                 .Where(quest => quest.clanQuestType == clanQuestType)
                                 .ToArray();
        }
        else if(@enum is PVPQuestType pvpQuestType)
        {
            return _questListPVP.OfType<PVPQuestSO>()
                     .Where(quest => quest.pvpQuestType == pvpQuestType)
                     .ToArray();
        }
        //In case no enum type matches
        return Array.Empty<QuestAbstractSO>();
    }
    #region Load Resources
    //LOAD Resources
    private QuestAbstractSO[] LoadQuestsFromFolder(string folderPath)
    {
        // Load all QuestAbstractSO assets from the folder
        QuestAbstractSO[] quests = UnityEngine.Resources.LoadAll<QuestAbstractSO>(folderPath);



        // Sort the quests by their type
        Array.Sort(quests, (quest1, quest2) => quest1.id.CompareTo(quest2.id));

        return quests;
    }
    private QuestInfoSO[] LoadQuestInfosFromFolder(string folderPath)
    {
        // Load all QuestAbstractSO assets from the folder
        QuestInfoSO[] quests = UnityEngine.Resources.LoadAll<QuestInfoSO>(folderPath);

        return quests;
    }
    #endregion
}
