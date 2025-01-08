using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PaneltemReward : MonoBehaviour
{
    List<GameObject> panels; // Mảng các panel
    [SerializeField] GameObject panelBase;
    [SerializeField] Image fade;

    public static Action<bool> OnActivePanelItemReward;
    public static Action OnResetCompleteQuest;

    void OnEnable()
    {
        // Gán hàm xử lý sự kiện cho tất cả các panel
        foreach (GameObject panel in panels)
        {
            AddEventTrigger(panel, EventTriggerType.PointerClick, OnPanelClick);
        }
    }
    void Update()
    {
        // Kiểm tra nếu chuột hoặc màn hình cảm ứng được bấm
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Đối với màn hình cảm ứng, kiểm tra lần chạm đầu tiên
            Vector2 touchPosition;
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
            }
            else
            {
                touchPosition = Input.mousePosition;
            }

            // Kiểm tra nếu không bấm vào bất kỳ panel nào trong mảng panels
            if (!IsPointerOverAnyPanel(panels.ToArray(), touchPosition))
            {
                OnActivePanelItemReward?.Invoke(true);
                OnResetCompleteQuest?.Invoke();
                panels.Clear();
                panelBase.SetActive(false);
            }
        }
    }
    public void SetFade(int A)
    {
        fade.color = new Color(0, 0, 0, A / 255f);
    }
    private void OnPanelClick(BaseEventData eventData)
    {
        // Ngăn chặn việc ẩn Panel khi bấm vào chính Panel
        eventData.Use();
    }
    public void SetPanels(List<GameObject> panels)
    {
        this.panels = panels;
    }
    private void AddEventTrigger(GameObject obj, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    private bool IsPointerOverAnyPanel(GameObject[] panelArray, Vector2 position)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            foreach (GameObject panel in panelArray)
            {
                if (result.gameObject == panel)
                {
                    return true; // Con trỏ đang ở trên một trong các panel
                }
            }
        }
        return false; // Không bấm vào bất kỳ panel nào
    }
}
