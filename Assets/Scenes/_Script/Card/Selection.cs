using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Selection : MonoBehaviour
{
    [SerializeField] float mapDistance;
    [SerializeField] float mapScale;
    public int mapCurrent = 0;
    public List<RectTransform> mapsList = new List<RectTransform>();
    internal static GameObject activeGameObject;
    internal static object objects;
    internal static readonly object[] gameObjects;

    void Awake()
    {
        //mapsList.AddRange(GetComponentsInChildren<RectTransform>());
        //mapsList.Remove(gameObject.GetComponent<RectTransform>());
        foreach (RectTransform i in this.transform)
        {
            mapsList.Add(i);
        }
        SetMapPos();
        SetMapActive();

    }
    public void SetMapPos()
    {
        foreach (RectTransform i in mapsList)
        {
            if (mapsList.IndexOf(i) == mapCurrent)
            {
                i.DOAnchorPos(new Vector3(0, 0, 0), 0.5f);
                if (i.transform.localScale == Vector3.one) continue;
                i.transform.DOScale(Vector3.one, 0.5f);
            }
            if (mapsList.IndexOf(i) > mapCurrent || mapsList.IndexOf(i) < mapCurrent)
            {
                //i.DOAnchorPosX((Distance * (maps.IndexOf(i) - mapCurrent), 0.5f)
                i.DOAnchorPosX(mapDistance * (mapsList.IndexOf(i) - mapCurrent), 0.5f);
                if (i.transform.localScale == Vector3.one * mapScale) continue;
                i.DOScale(Vector3.one * mapScale, 0.5f);
            }
        }
    }
    public void SetMapActive()
    {
        foreach (RectTransform i in mapsList)
        {
            if (Mathf.Abs(mapsList.IndexOf(i) - mapCurrent) >= 1)
            {
                i.gameObject.SetActive(false);
            }
            else
            {
                i.gameObject.SetActive(true);
            }
        }
    }
}
