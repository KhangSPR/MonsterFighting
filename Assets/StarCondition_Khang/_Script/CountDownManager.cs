using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownDisplay;
    public float waitDuration = 0.8f;   // Thời gian chờ cho mỗi trạng thái

    [SerializeField] private RectTransform fade;


    private void Start()
    {
        gameObject.SetActive(true);
        fade.gameObject.SetActive(true);
        Time.timeScale = 0;
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        // Hiển thị chữ "READY..."
        yield return StartCoroutine(DisplayText("READY...", true));

        // Hiển thị chữ "SET..."
        yield return StartCoroutine(DisplayText("SET...", true));

        // Hiển thị chữ "CHARACTER!!"
        yield return StartCoroutine(DisplayText("CHARACTER!!", false));

        // Kết thúc đếm ngược
        Debug.Log("Count Down Finished");
        gameObject.SetActive(false);
        fade.gameObject.SetActive(false);

        Time.timeScale = 1;

        //Set Ready Timer Portal
        GameManager.Instance.ReadyTimer = true;
    }

    IEnumerator DisplayText(string text, bool applyEffects)
    {
        countdownDisplay.text = text;


        if (applyEffects)
        {
            countdownDisplay.rectTransform.localScale = Vector3.one;
            countdownDisplay.rectTransform.DOScale(1.3f, waitDuration)
                .SetUpdate(true);

            yield return new WaitForSecondsRealtime(waitDuration);
        }
        else
        {
            // Hiển thị văn bản với kích thước cố định là 1.3
            countdownDisplay.rectTransform.localScale = Vector3.one * 1.3f;

            // Đợi mà không có hiệu ứng
            yield return new WaitForSecondsRealtime(0.8f);
        }
    }
}
