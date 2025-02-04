using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoading : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private UnityEngine.UI.Slider loadingSlider;
    [SerializeField] private TMP_Text textLoading;
    [SerializeField] private TMP_Text textSliderPercent;

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(AnimateLoadingText());
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;

        float fakeProgress = 0f;

        while (!loadOperation.isDone)
        {
            // Cập nhật tiến trình slider
            fakeProgress = Mathf.MoveTowards(fakeProgress, Mathf.Clamp01(loadOperation.progress / 0.9f), Time.deltaTime * 0.3f);
            loadingSlider.value = fakeProgress;

            // Cập nhật phần trăm
            textSliderPercent.text =  Mathf.RoundToInt(fakeProgress * 100) + "%";

            // Khi đạt 100%, chờ 3 giây rồi chuyển cảnh
            if (fakeProgress >= 1f && loadOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3);
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator AnimateLoadingText()
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (true)
        {
            // Thay đổi số lượng dấu chấm
            dotCount = (dotCount + 1) % 4; // 0 -> 1 -> 2 -> 3 -> 0
            textLoading.text = baseText + new string('.', dotCount);

            yield return new WaitForSeconds(0.5f); // Thay đổi dấu chấm mỗi 0.5 giây
        }
    }
}
