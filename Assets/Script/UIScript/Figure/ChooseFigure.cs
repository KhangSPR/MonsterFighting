using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFigure : MonoBehaviour
{
    [SerializeField] Figure mapList;
    [SerializeField] GameObject NextBtn;
    [SerializeField] GameObject PreviousBtn;

    private void Start()
    {
        SetActiveBtn();
    }
    public void SetActiveBtn()
    {
        if (mapList.mapCurrent == 0)
        {
            PreviousBtn.SetActive(false);
        }
        else { PreviousBtn.SetActive(true); }

        if (mapList.mapCurrent == mapList.mapsList.Count - 1)
        {
            NextBtn.SetActive(false);
        }
        else { NextBtn.SetActive(true); }
    }
    public void Next()
    {

        ++mapList.mapCurrent;
        //mapList.SetMapActive();
        StartCoroutine(Delay());
        mapList.SetMapPos();
        SetActiveBtn();
        mapList.UpdateStateBar();
    }
    public void Previous()
    {

        --mapList.mapCurrent;
        //mapList.SetMapActive();
        StartCoroutine(Delay());
        mapList.SetMapPos();
        SetActiveBtn();
        mapList.UpdateStateBar();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.2f);
        mapList.SetMapActive();
    }
}
