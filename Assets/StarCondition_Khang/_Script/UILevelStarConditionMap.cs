using TMPro;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class UILevelStarConditionMap : MonoBehaviour
{
    [SerializeField] TMP_Text DesDiff;
    [SerializeField] TMP_Text DesStar_1;
    [SerializeField] TMP_Text DesStar_2;
    [SerializeField] TMP_Text DesStar_3;
    [SerializeField] Button btnOke;
    private void Start()
    {
        if(btnOke!= null)
        {
            btnOke.onClick.AddListener(OnClick);
        }
    }
    private void OnEnable()
    {
        UpdateUIWithLevelSettings(MapManager.Instance.LoadCurrentLevelSettings());
    }
    private void OnDisable()
    {
        
    }
    public void OnClick()
    {
        gameObject.SetActive(false);
    }
    private void UpdateUIWithLevelSettings(LevelSettings levelSettings)
    {
        DesDiff.text = levelSettings.levelName;
        string levelConditions = levelSettings.GetLevelSettings();
        SetTitleStarConditionUI(levelConditions);
    }

    private void SetTitleStarConditionUI(string levelConditions)
    {
        string[] conditions = levelConditions.Split(',');

        DesStar_1.text = conditions.Length > 0 ? conditions[0].Trim() : string.Empty;
        DesStar_2.text = conditions.Length > 1 ? conditions[1].Trim() : string.Empty;
        DesStar_3.text = conditions.Length > 2 ? conditions[2].Trim() : string.Empty;
    }
}
