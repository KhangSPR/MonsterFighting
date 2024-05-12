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
        //ShopItemSO shopItemData = clickedButton.gameObject.GetComponent<ShopItemComponent>().ShopItemData;

        // starts a chain of events:

        //      ShopItemComponent (click the button) -->
        //      ShopController (buy an item) -->
        //      GameDataManager (verify funds)-->
        //      MagnetFXController (play effect on UI)

        // notify the ShopController (passes ShopItem data + UI Toolkit screen position)
        // Chuy?n ??i Vector3 thành Vector2 sau khi th?c hi?n phép c?ng
        Vector2 screenPos = button.gameObject.GetComponent<RectTransform>().position;



        Debug.Log(screenPos);

        CardClicked?.Invoke(CardManager.Instance.PanelCardHasSelect.CountCardNotSelect(), screenPos);

        //AudioManager.PlayDefaultButtonSound();
    }
}
