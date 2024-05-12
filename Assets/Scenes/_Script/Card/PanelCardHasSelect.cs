using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class PanelCardHasSelect : SaiMonoBehaviour
{
    public List<CardHasSelect> CardHasSelects = new List<CardHasSelect>();
    //protected override void LoadComponents()
    //{
    //    base.LoadComponents();
    //    LoadPanelCardHasSelect();
    //}
    //private void OnApplicationQuit()
    //{
    //    CardHasSelects.Clear();
    //}

    public void LoadPanelCardHasSelect()
    {
        foreach (Transform childTransform in transform)
        {
            CardHasSelects.Add(childTransform.GetComponent<CardHasSelect>());
        }
    }
    public void CheckForEnoughCard()
    {
        foreach (CardHasSelect cardHasSelect in CardHasSelects)
        {
            if (cardHasSelect.CardTower == null)
            {
                CardManager.Instance.ButtonCard.ButtonUI.SetConditionToClick(false);
                Debug.Log("false");
                return;
            }
        }
        CardManager.Instance.ButtonCard.ButtonUI.SetConditionToClick(true);
        Debug.Log("True");
    }
    public int CountCardNotSelect()
    {
        int count = 0;
        foreach (CardHasSelect cardHasSelect in CardHasSelects)
        {
            if (cardHasSelect.CardTower == null)
            {
                count++;
            }
        }
        Debug.Log(count);
        return count;
    }
}
