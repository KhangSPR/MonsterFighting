using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BtnUI : SaiMonoBehaviour
{
    [SerializeField] RectTransform[] panelCurcurent;
    public UnityEvent eventCurcurent;
    [SerializeField] RectTransform[] panelGoto;
    public UnityEvent eventGoto;

    [SerializeField] Transform[] transfromCurrent;
    [SerializeField] Transform[] transfromGoto;
    [SerializeField] Animator animator;
    // biến chứa hàm được truyền vào ở đây
    public void OnClickButton()
    {
        foreach (RectTransform rect in panelCurcurent)
        {
            if (rect != null) rect.gameObject.SetActive(false);
            // Chạy hàm được truyền từ insoector ở đây
        }
        foreach (Transform rect in transfromCurrent)
        {
            if (rect != null) rect.gameObject.SetActive(true);
        }
        eventCurcurent.Invoke();
        foreach (RectTransform rect in panelGoto)
        {
            if (panelGoto != null) rect.gameObject.SetActive(true);
        }
        foreach (Transform rect in transfromGoto)
        {
            if (rect != null) rect.gameObject.SetActive(false);
        }
        eventGoto.Invoke();
    }
    [SerializeField] bool conditionToClick = false;
    public void SetConditionToClick(bool condition)
    {
        conditionToClick = condition;
    }
    public bool GetConditionToClick()
    {
        return conditionToClick;
    }
    public void OnClickEvent()
    {
        if (!conditionToClick) return;

        foreach (RectTransform rect in panelCurcurent)
        {
            if (rect != null) rect.gameObject.SetActive(false);
        }
        foreach (RectTransform rect in panelGoto)
        {
            if (panelGoto != null) rect.gameObject.SetActive(true);
        }
    }
    public void PlayAnimationByName(string nameAnimation)
    {
        if (animator == null) return;

        // Đặt animation về trạng thái ban đầu trước khi phát lại
        animator.Play(nameAnimation, -1, 0f);
    }

}
