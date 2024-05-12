using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCard : MonoBehaviour
{
    [SerializeField] Selection ListCard;
    [SerializeField] GameObject NextBtn;
    [SerializeField] GameObject PreviousBtn;

    private void Start()
    {
        SetActiveBtn();
    }
    public void SetActiveBtn()
    {
        if (ListCard.mapCurrent == 0)
        {
            PreviousBtn.SetActive(false);
        }
        else { PreviousBtn.SetActive(true); }

        if (ListCard.mapCurrent == ListCard.mapsList.Count - 1)
        {
            NextBtn.SetActive(false);
        }
        else { NextBtn.SetActive(true); }
    }
    public void Next()
    {

        ++ListCard.mapCurrent;
        ListCard.SetMapActive();
        ListCard.SetMapPos();
        SetActiveBtn();
    }
    public void Previous()
    {

        --ListCard.mapCurrent;
        ListCard.SetMapActive();
        ListCard.SetMapPos();
        SetActiveBtn();
    }
}
