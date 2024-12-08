using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncLoading : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private UnityEngine.UI.Slider loadingSlider;

    public void LoadLevelBtn(string levelToload)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToload));

        Debug.Log("On Click Level");
    }

    IEnumerator LoadLevelASync(string levelToload)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToload);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
