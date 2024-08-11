using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataMap;
using UnityEngine;

public class Tooltip_Item : MonoBehaviour
{
    private static Tooltip_Item instance;

    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    [Header("Setting")]
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;


    [Header("Transform")]
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        instance = this;

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();

        HideTooltip();
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;


        Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;

        }
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

    }

    private void ShowTooltip(ItemObject itemObject)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        LoadItem(itemObject);


        Update();
    }
    private void LoadItem(ItemObject itemObject)
    {
        Name.text = itemObject.Name;
        Description.text = itemObject.description;
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);

        Description.color = Color.white;
    }

    public static void ShowTooltip_Static(ItemObject itemObject)
    {
        instance.ShowTooltip(itemObject);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
    public static void AddTooltip(Transform transform, ItemObject itemObject)
    {
        if (transform.GetComponent<Button_UI>() != null)
        {
            transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => Tooltip_Item.ShowTooltip_Static(itemObject);
            transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => Tooltip_Item.HideTooltip_Static();
        }
    }
}
