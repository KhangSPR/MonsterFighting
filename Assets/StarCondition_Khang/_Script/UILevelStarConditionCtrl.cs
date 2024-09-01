using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class UILevelStarConditionCtrl : MonoBehaviour
{
    [SerializeField] TMP_Text DesStar_1;
    [SerializeField] TMP_Text DesStar_2;
    [SerializeField] TMP_Text DesStar_3;
    [SerializeField] GameObject Fade;
    [Space]
    [Space]
    [Header("Tween")]
    [SerializeField] GameObject PanelUI;
    [SerializeField] GameObject Button;
    public void ActiveLevelConditionUI(bool active)
    {

        if (!active)
        {
            GameManager.Instance.TogglePauseGame();

            Debug.Log("Da set false");

            LeanTween.moveLocal(PanelUI, new Vector3(0f, 850f, 0f), 0.45f).setDelay(0f).setEase(LeanTweenType.easeOutCirc).setOnComplete(Setfade);

            return;
        }
        Fade.SetActive(true);
        gameObject.SetActive(true);
        Debug.Log("Da set true");

        LeanTween.moveLocal(PanelUI, new Vector3(0f, 0f, 0f), 0.6f).setDelay(0.3f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.alpha(Button.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f).setOnComplete(GameManager.Instance.TogglePauseGame);

    }
    void Setfade()
    {
        Fade.SetActive(false);
    }
    public void UpdateUIWithLevelSettings(LevelSettings levelSettings)
    {
        // Lấy chuỗi điều kiện từ LevelSettings
        string levelConditions = levelSettings.GetLevelSettings();

        // Gọi hàm để gán các điều kiện lên UI
        SetStarConditionUI(levelConditions);
    }

    public void SetStarConditionUI(string levelConditions)
    {
        // Tách chuỗi dựa trên dấu phẩy
        string[] conditions = levelConditions.Split(',');

        // Gán giá trị cho các TMP_Text
        DesStar_1.text = conditions.Length > 0 ? conditions[0].Trim() : string.Empty;
        DesStar_2.text = conditions.Length > 1 ? conditions[1].Trim() : string.Empty;
        DesStar_3.text = conditions.Length > 2 ? conditions[2].Trim() : string.Empty;
    }

}

