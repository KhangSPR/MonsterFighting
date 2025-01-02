using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Thêm thư viện DoTween
using Unity.VisualScripting;

public class QuestCtrl : MonoBehaviour
{
    [HideInInspector] public RectTransform rectTrans;

    [SerializeField] Button button;
    public Button ButtonQuest => button;
    [SerializeField] Image buttonImage; // Thêm Image để thay đổi màu của nút
    public TMP_Text TMP_Text;

    QuestAbstractSO questAbstractSO;
    public QuestAbstractSO QuestAbstractSO => questAbstractSO;

    QuestUIDisPlay questUIDisPlay;

    void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnItemClick);
    }

    public void SetQuestUIDisPlay(QuestUIDisPlay questUIDisPlay)
    {
        this.questUIDisPlay = questUIDisPlay;
    }

    public void SetQuestAbstractSO(QuestAbstractSO questAbstractSO)
    {
        this.questAbstractSO = questAbstractSO;
    }

    void OnItemClick()
    {
        Debug.Log("Click Quest Ctrl " + questAbstractSO.questInfoSO.name);

        questUIDisPlay.QuestPress(button);
        questUIDisPlay.ShowQuestDetails(questAbstractSO);
    }

}
