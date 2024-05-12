using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackMap : MonoBehaviour
{
    [SerializeField] Button btnBackMap;
    [SerializeField] GameObject Map;


    private void Start()
    {
        btnBackMap.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Destroy(Map);
        UIManager.Instance.ActiveGameUI();
    }
}
