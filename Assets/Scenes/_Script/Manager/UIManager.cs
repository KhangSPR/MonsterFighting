using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UICamera;
    public GameObject MainUI;

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

    public void ActiveGameUI()
    {
        MainUI.SetActive(true);
        UICamera.SetActive(true);
    }
    public void DetiveGameUI()
    {
        MainUI.SetActive(false);
        UICamera.SetActive(false);
        //DifficultHolder.SetActive(false);
    }
}
