using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelStarConditionCtrl : MonoBehaviour
{
    [SerializeField] TMP_Text DesDiff;
    [SerializeField] TMP_Text DesStar_1;
    [SerializeField] TMP_Text DesStar_2;
    [SerializeField] TMP_Text DesStar_3;
    [SerializeField] GameObject Fade;
    [SerializeField] GameObject PanelUI;
    [SerializeField] GameObject Button;
    [SerializeField] private StarHolderCondition starHolderCondition;
    [SerializeField] private Transform StarShowUI;
    [SerializeField] private GameObject StarsShow;
    public void Onclick()
    {
        GameManager.Instance.TogglePauseGame();

        LeanTween.moveLocal(PanelUI, new Vector3(0f, 850f, 0f), 0.45f)
                 .setDelay(0f)
                 .setEase(LeanTweenType.easeOutCirc)
                 .setOnComplete(() =>
                 {
                     Setfade();
                 });
    }

    public void ActiveLevelConditionUI()
    {
        Fade.SetActive(true);
        gameObject.SetActive(true);
        Debug.Log("Da set true");

        UpdateStarsBasedOnHpPercentage();

        LeanTween.moveLocal(PanelUI, new Vector3(0f, 0f, 0f), 0.5f)
                 .setDelay(0.1f)
                 .setEase(LeanTweenType.easeInCirc);
        LeanTween.alpha(Button.GetComponent<RectTransform>(), 1f, 0.5f)
                 .setDelay(1f)
                 .setOnComplete(() =>
                 {
                     GameManager.Instance.TogglePauseGame();
                 });
                 
    }

    private void Setfade()
    {
        Fade.SetActive(false);
    }

    public void UpdateUIWithLevelSettings(LevelSettings levelSettings)
    {
        DesDiff.text = levelSettings.levelName;
        string levelConditions = levelSettings.GetLevelSettings();
        SetTitleStarConditionUI(levelConditions);
    }

    public void SetTitleStarConditionUI(string levelConditions)
    {
        string[] conditions = levelConditions.Split(',');

        DesStar_1.text = conditions.Length > 0 ? conditions[0].Trim() : string.Empty;
        DesStar_2.text = conditions.Length > 1 ? conditions[1].Trim() : string.Empty;
        DesStar_3.text = conditions.Length > 2 ? conditions[2].Trim() : string.Empty;
    }

    private void UpdateStarsBasedOnHpPercentage()
    {
        float[] starThresholds = GameManager.Instance.GetCurrentHpPercentageArrays();

        // Xóa các sao cũ
        foreach (Transform child in StarShowUI)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        // Tạo sao mới dựa trên phần trăm HP
        foreach (float thresholdPercentage in starThresholds)
        {
            int roundedPercentage = Mathf.RoundToInt(thresholdPercentage / 10f);

            // Lấy Transform từ GetPosition
            Transform positionTransform = GetPosition(roundedPercentage);

            if (positionTransform != null)
            {
                GameObject star = Instantiate(StarsShow, positionTransform.position, Quaternion.identity, StarShowUI);

            }
            i++;
        }
    }

    private Transform GetPosition(int index)
    {
        // Đảm bảo index nằm trong phạm vi hợp lệ
        if (index >= 0 && index < starHolderCondition.StartHolderConition.Count)
        {
            return starHolderCondition.StartHolderConition[index];
        }
        return null;
    }
}
