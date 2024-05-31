using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListSelectCard : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Tabs;
    public Image[] TabButtons;
    public Vector2 InactiveTabButtonSize, ActiveTabButtonSize;
    [SerializeField] private Color ActiveTabColor;
    [SerializeField] private Color InactiveTabColor;

    void Start()
    {
        SetSelectObject();
        SwitchToTab(0);
    }
    private void SetSelectObject()
    {
        for (int i = 0; i < Tabs.Length; i++)
        {
            int currentIndex = i; //Bien Cuc Bo

            Button button = Tabs[i].GetComponent<Button>();

            // add Button
            button.onClick.AddListener(() => OnButtonClick(currentIndex));
        }
    }
    private void OnButtonClick(int clickedIndex)
    {
        Debug.Log("OnButtonSelectCard clickedIndex: " + clickedIndex);

        UnityAction<int, int> selectedAction = null;

        //Default = 0
        int objLoading = 0;

        switch (clickedIndex)
        {
            case 0:
                CardUIPanelManager.Instance.OnTowerButtonClickedTower(0);
                SwitchToTab(0);
                break;
            case 1:
                selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyAttackType;
                SwitchToTab(1);
                break;
            case 2:
                selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyAttackType;
                SwitchToTab(2);
                break;
            case 3:
                selectedAction = CardUIPanelManager.Instance.OnTowerButtonClickbyAttackType;
                SwitchToTab(3);
                break;
            default:
                break;
        }

        // Call Event
        selectedAction?.Invoke(objLoading, clickedIndex);
    }
    public void SwitchToTab(int TabID)
    {

        foreach (Image im in TabButtons)
        {
            im.rectTransform.sizeDelta = InactiveTabButtonSize;
            im.color = InactiveTabColor; 
        }

        TabButtons[TabID].rectTransform.sizeDelta = ActiveTabButtonSize;
        TabButtons[TabID].color = ActiveTabColor; 
    }

}
