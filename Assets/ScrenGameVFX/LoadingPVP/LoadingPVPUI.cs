using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPVPUI : MonoBehaviour
{
    [SerializeField] Image Sword_Fill1;
    [SerializeField] Image Sword_Fill2;

    [SerializeField] private float fillDuration = 3f; // Thời gian để đạt 100% (điều chỉnh theo yêu cầu)

    private Coroutine fillCoroutine;

    public static Action OnLoadingPVP;

    protected void OnEnable()
    {
        StartIncreasingImageFillAmount();
    }
    protected void OnDisable()
    {
        ResetImageFillAmount();
    }

    // Bắt đầu tăng fillAmount theo thời gian
    public void StartIncreasingImageFillAmount()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(IncreaseFillOverTime());
    }

    // Coroutine để tăng fillAmount theo thời gian
    private IEnumerator IncreaseFillOverTime()
    {
        float elapsedTime = 0f;
        float startValue1 = Sword_Fill1.fillAmount;
        float startValue2 = Sword_Fill2.fillAmount;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            Sword_Fill1.fillAmount = Mathf.Lerp(startValue1, 1f, elapsedTime / fillDuration);
            Sword_Fill2.fillAmount = Mathf.Lerp(startValue2, 1f, elapsedTime / fillDuration);

            yield return null; // Chờ tới frame tiếp theo
        }

        // Đảm bảo đạt 100% khi kết thúc
        Sword_Fill1.fillAmount = 1f;
        Sword_Fill2.fillAmount = 1f;

        // Kích hoạt sự kiện khi hoàn tất
        OnLoadingPVP?.Invoke();

        gameObject.SetActive(false);
    }



    // Reset fillAmount
    public void ResetImageFillAmount()
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }

        Sword_Fill1.fillAmount = 0f;
        Sword_Fill2.fillAmount = 0f;
    }
}
