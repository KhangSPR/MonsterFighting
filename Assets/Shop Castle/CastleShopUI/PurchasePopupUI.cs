using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePopupUI : MonoBehaviour
{
    [SerializeField]Transform purchase_confirm_popup;
    [SerializeField] Image imageFade;
    public void Open()
    {
        purchase_confirm_popup.gameObject.SetActive(true);
        imageFade.gameObject.SetActive(true);
    }

    public void Close()
    {
        purchase_confirm_popup.gameObject.SetActive(false);
        imageFade.gameObject.SetActive(false);

    }
    public void EnableGameObject()
    {
        purchase_confirm_popup.gameObject.SetActive(false);
    }
}
