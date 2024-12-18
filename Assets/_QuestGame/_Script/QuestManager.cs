using System;
using UnityEngine;
using UIGameDataMap;
using System.Linq;

public class QuestManager : SaiMonoBehaviour
{
    // Singleton instance of QuestManager
    private static QuestManager _instance;
    public static QuestManager Instance => _instance;

    // Path to the folder containing quest ScriptableObjects
    private const string QuestMainFolderPath = "Quest/MainQuest";

    // Array to hold loaded QuestAbstractSO objects
    [SerializeField]
    private QuestAbstractSO[] _questList;
    public QuestAbstractSO[] QuestList => _questList;

    // Ensures only one instance of QuestManager exists
    protected override void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("There can only be one instance of QuestManager in the scene.");
            return;
        }
        _instance = this;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        // Load quests from the specified folder
        _questList = LoadQuestsFromFolder(QuestMainFolderPath);
    }
    // Hàm trả về mảng QuestAbstractSO theo TypeQuestMain
    public QuestAbstractSO[] GetQuestsByType(TypeQuestMain questType)
    {
        return _questList.Where(quest => quest.typeQuest == questType).ToArray();
    }

    // Hàm trả về mảng QuestAbstractSO theo lvType
    public QuestAbstractSO[] GetQuestsByLvType(MainQuestSO.lvType lvType)
    {
        return _questList.OfType<MainQuestSO>() // Chỉ lấy các đối tượng MainQuestSO
                         .Where(quest => quest.LVType == lvType) // Kiểm tra lvType
                         .ToArray();
    }

    /// <summary>
    /// Loads all QuestAbstractSO assets from the specified folder and sorts them by quest type.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing QuestAbstractSO assets.</param>
    /// <returns>An array of sorted QuestAbstractSO objects.</returns>
    private QuestAbstractSO[] LoadQuestsFromFolder(string folderPath)
    {
        // Load all QuestAbstractSO assets from the folder
        QuestAbstractSO[] quests = UnityEngine.Resources.LoadAll<QuestAbstractSO>(folderPath);



        // Sort the quests by their type
        Array.Sort(quests, (quest1, quest2) => quest1.id.CompareTo(quest2.id));

        return quests;
    }

}
