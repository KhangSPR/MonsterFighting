using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    [SerializeField] GameObject[] panels; // Mảng các panel
    [SerializeField] ButtonClan buttonClan;
    [SerializeField] GameObject panelBase;
    [SerializeField] GameObject panelGoto;

    void Start()
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
            if (!IsPointerOverAnyPanel(panels, touchPosition))
            {
                if(panelGoto == null)
                {
                    panelBase.SetActive(false);
                }
                if (panelGoto!= null && !panelGoto.activeSelf)
                {
                    panelBase.SetActive(false);
                }
            }
            else
            {
                if (panelGoto != null && !panelGoto.activeSelf)
                {
                    if(buttonClan!= null)
                    {
                        buttonClan.OnclickBtn();
                    }
                }
            }
        }
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

    private void OnPanelClick(BaseEventData eventData)
    {
        // Ngăn chặn việc ẩn Panel khi bấm vào chính Panel
        eventData.Use();
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
