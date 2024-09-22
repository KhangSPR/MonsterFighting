using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackMap : MonoBehaviour
{
    [SerializeField] GameObject Map;

    Button btn;
    private void Start()
    {
        btn = transform.GetComponent<Button>();

        btn.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        Time.timeScale = 1;

        Destroy(Map);
        UIManager.Instance.ActiveGameUI();
    }
}
