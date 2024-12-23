using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCard : MonoBehaviour
{
    [SerializeField] Button_UI button;
    public Button_UI Button => button;
    [SerializeField] BtnUI buttonUI;
    public BtnUI ButtonUI => buttonUI;

    public static event Action<int, Vector2> CardClicked;

    private void Start()
    {
        SetupButton();
    }
    private void SetupButton()
    {
        button.ClickFunc = () =>
        {
            buttonUI.OnClickEvent();

            if (buttonUI.GetConditionToClick()) return;
            Action();
        };
    }
    void Action()
    {
        Debug.Log("Action");
        Vector2 screenPos = button.gameObject.GetComponent<RectTransform>().position;



        Debug.Log(screenPos);

        CardClicked?.Invoke(CardManager.Instance.PanelCardHasSelect.CountCardNotSelect(), screenPos);
    }
}
