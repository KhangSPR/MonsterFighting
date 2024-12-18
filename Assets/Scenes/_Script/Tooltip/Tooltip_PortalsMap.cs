/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UIGameDataMap;
using TMPro;

public class Tooltip_PortalsMap : MonoBehaviour
{

    private static Tooltip_PortalsMap instance;

    [SerializeField]
    private Camera uiCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    [Header("Settings")]
    [SerializeField] private TMP_Text power;


    [Header("GameObject")]
    [SerializeField] GameObject checkHasBoss;

    [Header("Prefab")]
    [SerializeField] GameObject EnemyPrefab;

    [Header("Transform")]
    private RectTransform backgroundRectTransform;
    [SerializeField] Transform Holder;
    [SerializeField] RectTransform HolderScale;

    //[Header("MapSO")]
    //[SerializeField] MapSO MapSO;
    private void Awake()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        instance = this;

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();

        HideTooltip();
    }
    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);


        localPoint.y += 100f;
        localPoint.x -= 550f;

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
    private void ShowTooltip(Portals portals)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        // Tạo phần văn bản và phần rarity
        string portalText = "Portal ";
        string rarityText = portals.rarityPortal.ToString();

        // Gán phần văn bản cố định "Portal"
        power.text = portalText + rarityText;

        // Thay đổi màu của phần rarity (được gán sau dấu cách)
        power.color = portals.GetColorForRarityPortal(portals.rarityPortal);



        checkHasBoss.SetActive(portals.hasBoss);

        LoadEnemys(portals);


        Update();
    }        
    private void LoadEnemys(Portals portals)
    {
        foreach (Transform child in Holder)
        {
            Destroy(child.gameObject);
        }
        int index = 0;
        foreach (EnemyType enemy in portals.enemyTypes)
        {
            GameObject enemyObject = Instantiate(EnemyPrefab, Holder);
            //Setting
            enemyObject.transform.Find("Img/avatar").GetComponent<Image>().sprite = enemy.enemyTypeSO.sprite;
            enemyObject.transform.Find("Img/Count").GetComponent<Text>().text = "x" + enemy.countEnemy.ToString();

            enemyObject.transform.Find("Info/Name").GetComponent<TMP_Text>().text = enemy.enemyTypeSO.name.ToString();
            enemyObject.transform.Find("Info/Name").GetComponent<TMP_Text>().color = enemy.GetColorForRarityEnemy(enemy.enemyTypeSO.rarity);
            enemyObject.transform.Find("Info/Des").GetComponent<TMP_Text>().text = "+ " + enemy.enemyTypeSO.attackType.ToString() + "\n+ " + enemy.enemyTypeSO.skillDes.ToString();
            //scale
            HolderScale.sizeDelta += new Vector2(0f, 135f);
            HolderScale.localPosition -= new Vector3(0f, 55f, 0f);


            index++;
        }
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);

        //Return Scale
        HolderScale.sizeDelta = new Vector2(490f, 25f);
        HolderScale.localPosition = new Vector3(275f, -130f, 0f);
    }

    public static void ShowTooltip_Static(Portals portals)
    {
        instance.ShowTooltip(portals);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
    public static void AddTooltip(Transform transform, Portals portals)
    {
        if (transform.GetComponent<Button_UI>() != null)
        {
            transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => ShowTooltip_Static(portals);
            transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => HideTooltip_Static();
        }
    }

}