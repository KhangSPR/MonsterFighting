using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainQuestCtrl : MonoBehaviour
{
    public Vector2 currentPosition;

    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    [SerializeField]
    LvQuestCtrl[] menuItems;
    public LvQuestCtrl[] MenuItems => menuItems;

    bool isExpanded = false;
    [SerializeField] int itemsCount;
    public Button _Button;
    public Transform HolderMain;
    public TMP_Text TMP_Text;
    public GameObject lockObj;
    private bool isAnimating = false; // Trạng thái hiệu ứng

    //[SerializeField]
    QuestUIDisPlay questUIDisPlay;
    public QuestUIDisPlay QuestUIDisPlay
    {
        set
        {
            questUIDisPlay = value;

        }
        get => questUIDisPlay;

    }
    void Start()
    {
        // Khởi tạo menu items
        // Thêm sự kiện click vào nút
        _Button.onClick.AddListener(ToggleMenu);
    }
    public void InitializeMenuItems()
    {
        // Lấy số lượng các menu items từ Holder
        itemsCount = HolderMain.childCount;
        menuItems = new LvQuestCtrl[itemsCount];

        // Khởi tạo menu items
        for (int i = 0; i < itemsCount; i++)
        {
            var child = HolderMain.GetChild(i);
            var lvQuestCtrl = child.GetComponent<LvQuestCtrl>();
            if (lvQuestCtrl != null)
            {
                menuItems[i] = lvQuestCtrl;
            }
        }
    }
    private List<Vector2> savedPositions = new List<Vector2>();

    private void SaveMenuPositions()
    {
        savedPositions.Clear();

        for (int i = 0; i < itemsCount; i++)
        {
            if (menuItems[i].rectTrans != null) // Kiểm tra null trước khi sử dụng rectTrans
            {
                savedPositions.Add(menuItems[i].rectTrans.anchoredPosition);
            }
            else
            {
                Debug.LogWarning($"rectTrans for item {i} is null!");
            }
        }
    }

    private void LoadMenuPositionsWithAnimation()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            if (menuItems[i].rectTrans != null)
            {
                menuItems[i].rectTrans.DOAnchorPos(savedPositions[i], expandDuration).SetEase(expandEase);
            }
            else
            {
                Debug.LogWarning($"rectTrans for item {i} is null!");
            }
        }
    }


    void ToggleMenu()
    {
        // Hiệu ứng nhấn nút với thay đổi màu và thu/phóng
        PlayButtonPressEffect(() =>
        {
            isExpanded = !isExpanded; // Đảo trạng thái mở rộng
            HalderToggle(isExpanded);

            if (isExpanded)
            {
                // Hiển thị các menu items
                HolderMain.gameObject.SetActive(true);

                if (savedPositions.Count <= 0)
                {
                    for (int i = 0; i < itemsCount; i++)
                    {
                        menuItems[i].rectTrans.DOAnchorPos(currentPosition + spacing * i, expandDuration).SetEase(expandEase);
                    }
                }
                else
                {
                    LoadMenuPositionsWithAnimation();
                }
            }
            else
            {
                SaveMenuPositions();
                // Ẩn các menu items
                for (int i = 0; i < itemsCount; i++)
                {
                    menuItems[i].rectTrans.DOAnchorPos(currentPosition, collapseDuration).SetEase(collapseEase);

                    if (i == itemsCount - 1)
                    {
                        HolderMain.gameObject.SetActive(false);
                    }
                }
            }
        });
    }


    private void HalderToggle(bool isHandle)
    {
        if (isHandle)
        {
            bool checkLVQuest = false;
            float totalHeight = 0f; // Biến lưu trữ tổng cộng chiều cao

            gameObject.SetActive(false);

            foreach (var lvQuest in menuItems)
            {
                if (lvQuest.IsExpanded)
                {
                    checkLVQuest = true;
                    float currentHeight = (spacing.y * lvQuest.MenuItems.Length);
                    totalHeight += currentHeight; // Cộng dồn chiều cao

                    Debug.Log("LVQuest: " + lvQuest.MenuItems.Length);
                    Debug.Log("CurrentHeight: " + currentHeight);
                }
            }

            // Nếu có LVQuest được mở rộng, áp dụng kích thước tổng
            if (checkLVQuest)
            {
                PlushAdjustSize(840f, totalHeight + (spacing.y * itemsCount));
            }
            else
            {
                AdjustSize(840f, -spacing.y * itemsCount + 60f);
            }

            gameObject.SetActive(true);
        }
        else
        {
            AdjustSize(840f, 60f);
        }
    }

    void PlushAdjustSize(float width, float height)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(width, -height);

        Debug.Log("PlushAdjustSize: " + height);
    }
    void AdjustSize(float width, float height)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            gameObject.SetActive(false);
            rectTransform.sizeDelta = new Vector2(width, height);

            RectTransform holderRect = HolderMain.GetComponent<RectTransform>();
            holderRect.anchoredPosition = new Vector2(0, -150f); //Repair Holder 10 = 150px

            //Holder.position = new Vector3(width, currentPosition.y * itemsCount,0);

            gameObject.SetActive(true);

        }
    }

    public void AdJustSizeBackground(bool active, float width, float height)
    {
        Debug.Log("AdJustSizeBackground: " + active);

        gameObject.SetActive(false);

        if (active)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            rectTransform.sizeDelta += new Vector2(width, -height);
        }
        else
        {

            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta -= new Vector2(width, -height);
        }
        gameObject.SetActive(true);

    }
    void OnDestroy()
    {
        // Xóa listener để tránh rò rỉ bộ nhớ
        _Button.onClick.RemoveListener(ToggleMenu);
    }
    void PlayButtonPressEffect(System.Action onComplete)
    {
        if (isAnimating) return; // Nếu hiệu ứng đang chạy, không thực hiện gì thêm

        if (TMP_Text == null)
        {
            Debug.LogWarning("TMP_Text is not assigned!");
            onComplete?.Invoke();
            return;
        }
        onComplete?.Invoke();

        Color originalTextColor = TMP_Text.color; // Lưu màu gốc của chữ
        Color highlightColor = new Color(1f, 215f / 255f, 0f); // Vàng Rực Rỡ

        isAnimating = true; // Đánh dấu hiệu ứng đang chạy

        // Hiệu ứng thay đổi màu chữ
        TMP_Text.DOColor(highlightColor, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            TMP_Text.DOColor(originalTextColor, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                isAnimating = false; // Hiệu ứng hoàn thành, cho phép nhấn lại
            });
        });
    }
}
