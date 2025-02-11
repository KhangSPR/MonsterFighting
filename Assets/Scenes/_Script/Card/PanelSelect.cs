using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSelect : MonoBehaviour
{
    private void OnEnable()
    {
        CardManager.Instance.InstanceAllCard();
        CardManager.Instance.InstanceCardSlot();
        CardManager.Instance.PanelCardHasSelect.LoadPanelCardHasSelect();
        CardManager.Instance.CheckCardIsWorking(); //Card Play Has Select
        CardManager.Instance.PanelCardHasSelect.CheckForEnoughCard();
        CardManager.Instance.CheckCardActive();
    }

    private void OnDisable()
    {
        CardManager.Instance.RemoALlCard();
        CardManager.Instance.RemoALLSlot();
        CardManager.Instance.PanelCardHasSelect.CardHasSelects.Clear();
        CardManager.Instance.cardSelectTowers.Clear();
    }

}
