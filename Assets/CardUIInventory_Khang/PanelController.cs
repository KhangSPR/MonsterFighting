using UnityEngine;
using UnityEngine.EventSystems;

public class PanelController : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelBase;
    [SerializeField] GameObject CardStats;

    void Start()
    {
        // Gán hàm xử lý sự kiện cho Panel
        AddEventTrigger(panel, EventTriggerType.PointerClick, OnPanelClick);
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

            // Kiểm tra nếu không bấm vào Panel
            if (!IsPointerOverUIObject(panel, touchPosition))
            {
                if (CardStats.activeSelf == false)
                {
                    panelBase.SetActive(false);
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

    private bool IsPointerOverUIObject(GameObject obj, Vector2 position)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = position
        };
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == obj)
                return true;
        }
        return false;
    }
}
