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

public class Tooltip_PortalsInGame : MonoBehaviour
{

    private static Tooltip_PortalsInGame instance;

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

    private void ShowTooltip(MapSO mapSO, int portalsIndex)
    {
        UIGameDataMap.Portals[] portalsLst = mapSO.GetPortals(mapSO.difficult);
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        Portals portals = portalsLst[portalsIndex];


        power.text = portals.rarityPortal.ToString();
        power.color = mapSO.GetColorForRarityPortal(portals.rarityPortal); //Change Color


        checkHasBoss.SetActive(portals.hasBoss);

        LoadEnemys(mapSO, portals);


        Update();
    }
    private void LoadEnemys(MapSO mapSO, Portals portals)
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
            enemyObject.transform.Find("Img/avatar").GetComponent<Image>().sprite = enemy.Sprite;
            enemyObject.transform.Find("Img/Count").GetComponent<Text>().text = "x" + enemy.countEnemy.ToString();

            enemyObject.transform.Find("Info/Rarity").GetComponent<TMP_Text>().text = enemy.rarity.ToString();
            enemyObject.transform.Find("Info/Rarity").GetComponent<TMP_Text>().color = mapSO.GetColorForRarity(enemy.rarity);

            enemyObject.transform.Find("Info/Des").GetComponent<Text>().text = "Name: " + enemy.name + "\nSkill: " + enemy.skillType[0].skill;
            enemyObject.transform.Find("Info/Des").GetComponent<Text>().color = mapSO.GetColorForRarity(enemy.rarity);

            //scale
            HolderScale.sizeDelta += new Vector2(0f, 100f);
            HolderScale.localPosition -= new Vector3(0f, 50f, 0f);


            index++;
        }
    }
    private void HideTooltip()
    {
        gameObject.SetActive(false);

        //Return Scale
        HolderScale.sizeDelta = new Vector2(300f, 25f);
        HolderScale.localPosition = new Vector3(180f, -110f, 0f);
    }

    public static void ShowTooltip_Static(MapSO mapSO, int portalsIndex)
    {
        instance.ShowTooltip(mapSO, portalsIndex);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
    public static void AddTooltip(Transform transform, MapSO mapSO, int portalsIndex)
    {
        if (transform.GetComponent<Button_UI>() != null)
        {
            transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => Tooltip_PortalsInGame.ShowTooltip_Static(mapSO, portalsIndex);
            transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => Tooltip_PortalsInGame.HideTooltip_Static();
        }
    }

}