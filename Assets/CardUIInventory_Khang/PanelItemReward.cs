using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UIGameDataManager;

public class PanelItemReward : MonoBehaviour
{
    List<GameObject> panels; // Mảng các panel
    [SerializeField] GameObject panelBase;
    [SerializeField] Image fade;

    public static Action<bool> OnActivePanelItemReward;
    public static Action OnResetCompleteQuest;

    void Start()
    {
        if (panels.Count <= 0) return;
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
    public void ShowRewards(int[] itemRewardSpins, SpinLevelItemSO[] spinLevelItem)
    {
        gameObject.SetActive(true);

        Transform holder = transform.Find("ListItemReward");

        // Xóa các item cũ trong danh sách
        foreach (Transform o in holder)
        {
            Destroy(o.gameObject);
        }

        var createdItems = new List<GameObject>();
        var rewardSpinsList = new List<int>(itemRewardSpins); // Chuyển mảng thành danh sách

        // Lặp qua rewardSpinsList và xử lý từng phần tử
        for (int spinIndex = 0; spinIndex < rewardSpinsList.Count;)
        {
            bool itemProcessed = false; // Đánh dấu khi xử lý được phần tử
            int currentRewardIndex = rewardSpinsList[spinIndex];

            foreach (SpinLevelItemSO spinLevel in spinLevelItem)
            {
                foreach (UIGameDataMap.Resources resource in spinLevel.ResourceItems)
                {
                    if (currentRewardIndex == 1)
                    {
                        // Tạo mới item reward
                        GameObject objNew = Instantiate(RewardClaimManager.Instance.ItemReward, holder).gameObject;
                        objNew.SetActive(false);

                        ItemTooltipReward itemTooltip = objNew.GetComponent<ItemTooltipReward>();
                        itemTooltip.Avatar.sprite = resource.item.Image;
                        itemTooltip.CountTxt.text = $"x{resource.Count}";
                        itemTooltip.ItemReward = resource.item;

                        // Rairity Material
                        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resource.item.itemRarity);

                        // ADD Item
                        GameDataManager.Instance.OnReceiverRewardResources(resource);
                        Debug.Log("Call On Receiver Resources");

                        // Thêm vào danh sách để xử lý animation
                        createdItems.Add(objNew);
                        Debug.Log("Processed reward index: " + currentRewardIndex);

                        // Đánh dấu đã xử lý
                        itemProcessed = true;
                        break;
                    }
                    currentRewardIndex--;
                }

                if (itemProcessed) break;

                foreach (InventoryItem item in spinLevel.InventoryItems)
                {
                    if (currentRewardIndex == 1)
                    {
                        // Tạo mới item object
                        GameObject objNew = Instantiate(RewardClaimManager.Instance.ItemObject, holder).gameObject;
                        objNew.SetActive(false);

                        ItemTooltipInventory itemTooltip = objNew.GetComponent<ItemTooltipInventory>();
                        itemTooltip.Avatar.sprite = item.itemObject.Sprite;
                        itemTooltip.CountTxt.text = $"x{item.count}";
                        itemTooltip.ItemObject = item.itemObject;
                        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(item.itemObject.itemRarity);

                        InventoryManager.Instance.inventory.AddItem(new Item(item.itemObject), item.count);

                        // Thêm vào danh sách để xử lý animation
                        createdItems.Add(objNew);
                        Debug.Log("Processed inventory index: " + currentRewardIndex);

                        // Đánh dấu đã xử lý
                        itemProcessed = true;
                        break;
                    }
                    currentRewardIndex--;
                }

                if (itemProcessed) break;
            }

            // Nếu xử lý xong phần tử hiện tại, xóa nó khỏi danh sách
            if (itemProcessed)
            {
                rewardSpinsList.RemoveAt(spinIndex);
            }
            else
            {
                // Nếu không xử lý được, tăng chỉ số để tránh vòng lặp vô hạn
                spinIndex++;
            }
        }

        // Phát hoạt ảnh
        RewardClaimManager.Instance.PlayItemsWithAnimation(createdItems, 1.3f / 2);

        // Cập nhật giao diện
        SetFade(80);
        SetPanels(createdItems);
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
