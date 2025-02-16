using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataMap;
using Unity.VisualScripting;
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

        localPoint.y += 100f;
        localPoint.x -= 630f;

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

    private void ShowTooltipResource(ItemReward resources)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        LoadItemResources(resources);


        Update();
    }
    private void ShowTooltipResources(ItemReward resources)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        LoadItemResources(resources);


        Update();
    }
    private void LoadItem(ItemObject itemObject)
    {
        Name.text = itemObject.Name;
        Description.text = itemObject.description;
    }
    private void LoadItemResources(ItemReward resources)
    {
        Name.text = resources.ItemName;
        Description.text = resources.ItemDescription;
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);

        Description.color = Color.white;
    }

    public static void ShowTooltip_StaticInventory(ItemObject itemObject)
    {
        if (instance == null)
        {
            Debug.LogError("Tooltip_Item instance is null.");
            return;
        }
        if (itemObject == null)
        {
            Debug.LogError("ItemObject is null.");
            return;
        }
        instance.ShowTooltip(itemObject);
    }

    public static void HideTooltip_Static()
    {
        if (instance == null)
        {
            Debug.LogError("Tooltip_Item instance is null.");
            return;
        }
        instance.HideTooltip();
    }

    public static void ShowTooltip_StaticResources(ItemReward resources)
    {
        instance.ShowTooltipResources(resources);
    }
    public static void AddTooltip(Transform transform, ItemObject itemObject, ItemReward itemReward)
    {
        if (transform.GetComponent<Button_UI>() != null)
        {
            if (itemObject != null)
            {
                transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () =>
                {
                    ShowTooltip_StaticInventory(itemObject);

                    ItemTooltipInventory itemTooltipInventory = transform.GetComponent<ItemTooltipInventory>();
                    if (itemTooltipInventory == null) return;
                    if (itemTooltipInventory.DisplayInventory == null) return;

                    transform.GetComponent<ItemTooltipInventory>().SetLableItem(true);
                };
                transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () =>
                {
                    HideTooltip_Static();

                    ItemTooltipInventory itemTooltipInventory = transform.GetComponent<ItemTooltipInventory>();
                    if (itemTooltipInventory == null) return;
                    if (itemTooltipInventory.DisplayInventory == null) return;

                    transform.GetComponent<ItemTooltipInventory>().SetLableItem(false);

                };
            }
            else if (itemReward != null)
            {
                transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => ShowTooltip_StaticResources(itemReward);
                transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => HideTooltip_Static();
            }
        }
    }

}
