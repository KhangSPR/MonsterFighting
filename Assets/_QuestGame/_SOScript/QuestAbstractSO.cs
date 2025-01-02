using UnityEngine;

public enum TypeQuestMain { MainQuest, ClanQuest, PvPQuest, EventQuest} //.vvv

public abstract class QuestAbstractSO : ScriptableObject
{
    [Header("Quest Main Information")]
    public uint id; //Sort
    public TypeQuestMain typeQuest;
    public QuestInfoSO questInfoSO;
    public bool isReceived;
}
