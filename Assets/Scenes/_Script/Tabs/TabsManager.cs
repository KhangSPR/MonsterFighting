using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    public GameObject[] Tabs;
    public Image[] TabButtons;
    public Sprite InactiveTabBG, ActiveTabBG;
    public Vector2 InactiveTabButtonSize, ActiveTabButtonSize;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform[] scrollRects;

    private void Start()
    {
        SwitchToTab(0);
    }

    public void SwitchToTab(int TabID)
    {
        foreach (GameObject go in Tabs)
        {
            go.SetActive(false);
        }
        Tabs[TabID].SetActive(true);

        foreach (Image im in TabButtons)
        {
            im.sprite = InactiveTabBG;
            im.rectTransform.sizeDelta = InactiveTabButtonSize;
        }
        TabButtons[TabID].sprite = ActiveTabBG;
        TabButtons[TabID].rectTransform.sizeDelta = ActiveTabButtonSize;
    }
    //Set On Inspector
    public void SetScrollRect(int TabID)
    {
        if (scrollRect == null) return;

        scrollRect.content = scrollRects[TabID];
    }
}
