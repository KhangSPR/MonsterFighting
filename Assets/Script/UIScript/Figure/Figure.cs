using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class Figure : MonoBehaviour
{

    [SerializeField] RectTransform paginationIcon;
    [SerializeField] RectTransform PaginationPanel;
    [SerializeField] float paginationDistance;
    public List<RectTransform> paginationLst = new List<RectTransform>();

    [Space]
    [Space]
    [Space]
    [Space]
    [Space]

    [SerializeField] float mapDistance;
    [SerializeField] float mapScale;
    public int mapCurrent = 0;
    public List<RectTransform> mapsList = new List<RectTransform>();
    void Awake()
    {
        //mapsList.AddRange(GetComponentsInChildren<RectTransform>());
        //mapsList.Remove(gameObject.GetComponent<RectTransform>());
        foreach (RectTransform i in this.transform)
        {
            mapsList.Add(i);
        }
        SetMapPos();
        SetPaginationPos();
        SetMapActive();

    }
    private void Update()
    {
        //Debug.Log(mapsList[0].anchoredPosition);
    }
    public void SetMapPos()
    {
        foreach (RectTransform i in mapsList)
        {
            if (mapsList.IndexOf(i) == mapCurrent)
            {
                i.DOAnchorPos(new Vector3(0, 0, 0), 0.5f);
                //if (i.transform.localScale == Vector3.one) continue;
                //i.transform.DOScale(Vector3.one, 0.5f);
            }
            if (mapsList.IndexOf(i) > mapCurrent || mapsList.IndexOf(i) < mapCurrent)
            {
                //i.DOAnchorPosX((Distance * (maps.IndexOf(i) - mapCurrent), 0.5f)
                i.DOAnchorPosX(mapDistance * (mapsList.IndexOf(i) - mapCurrent), 0.5f);
                //if (i.transform.localScale == Vector3.one * mapScale) continue;
                //i.DOScale(Vector3.one * mapScale, 0.5f);
            }
        }
        UpdateStateBar();
    }
    public void SetMapActive()
    {
        foreach (RectTransform i in mapsList)
        {
            if (mapsList.IndexOf(i) != mapCurrent)
            {
                i.gameObject.SetActive(false);
            }
            else
            {
                i.gameObject.SetActive(true);
            }


        }
    }

    public void SetPaginationPos()
    {

        float startPoint = -((paginationDistance) / 2) * (mapsList.Count - 1);
        foreach (RectTransform i in mapsList)
        {
            RectTransform Pagination = Instantiate(paginationIcon, this.PaginationPanel.transform);
            if (mapsList.IndexOf(i) == 0)
            {
                Pagination.anchoredPosition = new Vector2(startPoint, 0);
            }
            else
            {
                Pagination.anchoredPosition = new Vector2(startPoint + paginationDistance * mapsList.IndexOf(i), 0);
            }
        }
        paginationLst.AddRange(PaginationPanel.GetComponentsInChildren<RectTransform>());
        paginationLst.Remove(PaginationPanel.gameObject.GetComponent<RectTransform>());
        UpdateStateBar();
    }
    public void UpdateStateBar()
    {
        foreach (RectTransform i in paginationLst)
        {
            if (paginationLst.IndexOf(i) == mapCurrent)
            {
                if (i.lossyScale == Vector3.one * 1.5f) continue;
                i.transform.DOScale(Vector3.one * 1.5f, 0.2f);
            }
            else
            {
                if (i.lossyScale == Vector3.one) continue;
                i.transform.DOScale(Vector3.one, 0.2f);
            }
        }
    }
}

