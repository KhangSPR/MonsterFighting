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
    [SerializeField] private GameObject[] starsShow;
    public GameObject[] StarsShow => starsShow;
    public RectTransform targetUI;

    [SerializeField] SpriteRenderer emptySpriteRenderer;
    public SpriteRenderer EmptySpriteRenderer => emptySpriteRenderer;
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
    //Slider Star Condition
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
    // Update Star - 0 -> 1 -> 2
    private void UpdateStarsBasedOnHpPercentage()
    {
        float[] starThresholds = GameManager.Instance.GetCurrentHpPercentageArrays();

        // Tạo sao mới dựa trên phần trăm HP
        int i = 0;
        foreach (float thresholdPercentage in starThresholds)
        {
            Vector3 newPos = CalculateUIPosition(thresholdPercentage);
            CreateFlagAnimationAtPosition(newPos,i);

            Debug.Log("starThresholds :" + thresholdPercentage);
            i++;
        }
        targetUI.gameObject.SetActive(true);
    }
    private Vector3 CalculateUIPosition(float percentage)
    {
        float minX = -370 / 2f;
        float maxX = 370 / 2f;

        // Kiểm tra giá trị của minX và maxX
        Debug.Log("minX: " + minX);
        Debug.Log("maxX: " + maxX);

        // Kiểm tra giá trị của percentage
        Debug.Log("Percentage: " + percentage);

        // Tính toán vị trí dựa trên percentage
        float positionXTarget = Mathf.Lerp(minX, maxX, percentage / 100f);

        // Kiểm tra giá trị của positionXTarget
        Debug.Log("Vector Target (X): " + positionXTarget);

        return new Vector3(positionXTarget, targetUI.localPosition.y, targetUI.localPosition.z);
    }


    private void CreateFlagAnimationAtPosition(Vector3 position, int i)
    {
        starsShow[i].transform.localPosition = position;

        starsShow[i].SetActive(true);
    }
}
