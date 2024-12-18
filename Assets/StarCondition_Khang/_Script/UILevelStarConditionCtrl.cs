using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILevelStarConditionCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text DesDiff;
    [SerializeField] private TMP_Text DesStar_1;
    [SerializeField] private TMP_Text DesStar_2;
    [SerializeField] private TMP_Text DesStar_3;
    [SerializeField] private GameObject PanelUI;
    [SerializeField] private GameObject Button;
    [SerializeField] private GameObject[] starsShow;
    public GameObject[] StarsShow => starsShow;
    public RectTransform targetUI;

    [SerializeField] private SpriteRenderer emptySpriteRenderer;
    public SpriteRenderer EmptySpriteRenderer => emptySpriteRenderer;

    [SerializeField] private CanvasGroup fade;
    [Header("Countdown")]
    private bool flagFirst = false;
    [SerializeField] private CountDownManager countDownCtrl;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OpenMenuExit()
    {
        if (fade == null) return;

        fade.gameObject.SetActive(true);
        fade.DOFade(0.5f, 0.25f).OnComplete(() =>
        {
            GameManager.Instance?.TogglePauseGame();
        });
    }

    public void OnClickIconStar()
    {
        if (rectTransform == null) return;

        rectTransform.localScale = Vector3.one;
        OpenMenuExit();
    }

    private void CloseMenuExit()
    {
        if (rectTransform == null || fade == null) return;

        rectTransform.DOScale(Vector3.zero, 0.35f);
        fade.DOFade(0f, 0.5f).OnComplete(() =>
        {
            fade.gameObject.SetActive(false);

            if (!flagFirst)
            {
                countDownCtrl.enabled = true;
                flagFirst = true;
            }
        });
    }

    public void OnClick()
    {
        GameManager.Instance?.TogglePauseGame();
        CloseMenuExit();
    }

    public void ActiveLevelConditionUI()
    {
        UpdateStarsBasedOnHpPercentage();

        if (SettingManager.Instance?.currentSettings.starPoint == true)
        {
            fade?.gameObject.SetActive(false);
            Debug.Log("Fade set to false");

            if (!flagFirst)
            {
                countDownCtrl.enabled = true;
                flagFirst = true;
            }
        }
        else
        {
            fade?.gameObject.SetActive(true);
            rectTransform.DOScale(Vector3.one, 0.45f).SetDelay(0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                GameManager.Instance?.TogglePauseGame();
            });
        }
    }

    public void UpdateUIWithLevelSettings(LevelSettings levelSettings)
    {
        if (levelSettings == null) return;

        DesDiff.text = levelSettings.levelName;
        string levelConditions = levelSettings.GetLevelSettings();
        SetTitleStarConditionUI(levelConditions);
    }

    public void SetTitleStarConditionUI(string levelConditions)
    {
        if (string.IsNullOrEmpty(levelConditions)) return;

        string[] conditions = levelConditions.Split(',');

        DesStar_1.text = conditions.Length > 0 ? conditions[0].Trim() : string.Empty;
        DesStar_2.text = conditions.Length > 1 ? conditions[1].Trim() : string.Empty;
        DesStar_3.text = conditions.Length > 2 ? conditions[2].Trim() : string.Empty;
    }

    private void UpdateStarsBasedOnHpPercentage()
    {
        float[] starThresholds = GameManager.Instance?.GetCurrentHpPercentageArrays();
        if (starThresholds == null || targetUI == null) return;

        for (int i = 0; i < starThresholds.Length && i < starsShow.Length; i++)
        {
            Vector3 newPos = CalculateUIPosition(starThresholds[i]);
            CreateFlagAnimationAtPosition(newPos, i);
        }

        targetUI.gameObject.SetActive(true);
    }

    private Vector3 CalculateUIPosition(float percentage)
    {
        float minX = -185f;
        float maxX = 185f;

        float positionXTarget = Mathf.Lerp(minX, maxX, percentage / 100f);
        return new Vector3(positionXTarget, targetUI.localPosition.y, targetUI.localPosition.z);
    }

    private void CreateFlagAnimationAtPosition(Vector3 position, int i)
    {
        if (starsShow[i] == null) return;

        starsShow[i].transform.localPosition = position;
        starsShow[i].SetActive(true);
    }
}
