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
    [SerializeField] GameObject PanelUI;
    [SerializeField] GameObject Button;
    [SerializeField] private StarHolderCondition starHolderCondition;
    [SerializeField] private Transform StarShowUI;
    [SerializeField] private GameObject StarsShow;

    
    //Animation
    [SerializeField] private RectTransform fade;

    [Space]
    [Space]
    [Header("Cowndown")]
    //CownDownPlay
    bool flagFirst = false;
    [SerializeField] CountDownManager countDownCtrl;
    private void OpenMenuExit()
    {
        
        LeanTween.scale(transform.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.5f)
        .setDelay(0.45f).setEase(LeanTweenType.easeOutBack);

        LeanTween.alpha(fade, 0.5f, 0.8f).setOnComplete(() => {

            GameManager.Instance.TogglePauseGame(); 

        
        });
    }
    private void CloseMenuExit()
    {
        LeanTween.scale(transform.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0.45f);
        LeanTween.alpha(fade, 0f, 0.5f).setOnComplete(() => {
            fade.gameObject.SetActive(false);

            if (!flagFirst)
            {
                countDownCtrl.enabled = true;
                flagFirst = true;
            }

        });

    }
    
    public void Onclick()
    {
        GameManager.Instance.TogglePauseGame();

        CloseMenuExit();
    }

    public void ActiveLevelConditionUI()
    {
        UpdateStarsBasedOnHpPercentage();

        OpenMenuExit();
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
