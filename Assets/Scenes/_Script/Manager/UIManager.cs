using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UICamera;
    public GameObject MainUI;
    public RectTransform HouseUI;
    [Space]
    [Header("UI Element")]
    [SerializeField] GameObject UiResourcesInChooseMap;
    [SerializeField] GameObject UISetting;
    [SerializeField] GameObject UILeft;
    [SerializeField] GameObject UIStatsPlayer;

    [SerializeField] Animator animatorCastle;

    private static UIManager instance;                             //instance variable
    public static UIManager Instance { get => instance; }          //instance getter

    public void Awake()
    {
        if (UIManager.instance != null)
        {
            Debug.LogError("Only 1 UIManager Warning");
        }
        UIManager.instance = this;
        //LoadAreas();
    }
    private void Start()
    {
        if (!PlayerManager.Instance.IsDiaLog) return;

        this.CallWaitAndShowAnimator(); //Call Animation Castle After 1 second
    }
    public void ActiveGameUI()
    {
        MainUI.SetActive(true);
        UICamera.SetActive(true);
    }
    public void DetiveGameUI()
    {
        MainUI.SetActive(false);
        UICamera.SetActive(false);
    }
    private void CallWaitAndShowAnimator()
    {
        StartCoroutine(WaitAndShowAnimator());
    }
    private IEnumerator WaitAndShowAnimator()
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(1f);

        // Bật Animator
        ShowAnimator();
    }
    private void ShowAnimator()
    {
        animatorCastle.enabled = true;
    }
    private void DeActiveAnimator()
    {
        animatorCastle.enabled = false;
    }
    public void DeActiveUI()
    {
        DeActiveAnimator();

        UiResourcesInChooseMap.SetActive(false);
        UISetting.SetActive(false);
        UILeft.SetActive(false);
        UIStatsPlayer.SetActive(false);
    }
    public void ShowActiveUI()
    {
        Debug.Log("ShowActiveUI");

        CallWaitAndShowAnimator();

        UiResourcesInChooseMap.SetActive(true);
        UISetting.SetActive(true);
        UILeft.SetActive(true);
        UIStatsPlayer.SetActive(true);
    }
}
