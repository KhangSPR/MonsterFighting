using UnityEngine;

public enum TypeQuestMain { main, clan, } //.vvv

public abstract class QuestAbstractSO : ScriptableObject
{
    [Header("Quest Main Information")]
    public uint id; //Sort
    public TypeQuestMain typeQuest;
    public QuestInfoSO questInfoSO;

}
