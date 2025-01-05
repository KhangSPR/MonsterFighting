using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LvQuestCtrl : MonoBehaviour
{
    public Vector2 currentPosition;
    public RectTransform rectTrans;

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
    private QuestCtrl[] menuItems = new QuestCtrl[0]; // Mảng bắt đầu rỗng
    public QuestCtrl[] MenuItems => menuItems;

    bool isExpanded = false;
    public bool IsExpanded => isExpanded;
    public int itemsCount;
    public Transform Holder;
    public GameObject lockLv;
    public TMP_Text tmp_Text;
    public Button _Button;

    private bool isAnimating = false; // Trạng thái hiệu ứng                                    
    MainQuestCtrl mainQuestCtrl; // SettingsMenu reference

    //Check button
    [SerializeField]
    bool isCheckLv;
    public bool IsCheckLv
    {
        set { isCheckLv = value; }
        get { return isCheckLv; }
    }
    //Level
    private int level; // Backing field for the property

    public int Level
    {
        set { level = value; }
        get { return level; }
    }

    public static event Action<Vector2, int> LvQuestCtrlClicked;
    void Start()
    {
        _Button.onClick.AddListener(OnClickButton);

        mainQuestCtrl = transform.parent.parent.GetComponent<MainQuestCtrl>();
    }
    void OnClickButton()
    {
        if(mainQuestCtrl.typeQuestMain != TypeQuestMain.MainQuest)
        {
            ToggleMenu();
            return;
        }
        if (isCheckLv)
        {
            ToggleMenu();
        }
        else
        {
            LvQuestCtrlClicked?.Invoke(transform.position, level);
        }
    }
    public void AddMenuItem(QuestCtrl newItem)
    {
        if (newItem != null)
        {
            // Tạo một mảng mới với kích thước lớn hơn
            QuestCtrl[] newMenuItems = new QuestCtrl[menuItems.Length + 1];

            // Sao chép các phần tử cũ vào mảng mới
            for (int i = 0; i < menuItems.Length; i++)
            {
                newMenuItems[i] = menuItems[i];
            }

            // Thêm phần tử mới vào cuối mảng
            newMenuItems[menuItems.Length] = newItem;

            // Gán lại mảng đã được mở rộng
            menuItems = newMenuItems;
        }
    }
    void ToggleMenu()
    {
        PlayButtonPressEffect(() =>
        {
            isExpanded = !isExpanded;
            ScaleBackGround(isExpanded);
            AdjustSize();
            AdJustSizeLvQuest();
            if (isExpanded)
            {
                // Hiển thị các menu items
                Holder.gameObject.SetActive(true);
                for (int i = 0; i < itemsCount; i++)
                {
                    menuItems[i].rectTrans.DOAnchorPos(currentPosition + spacing * i, expandDuration).SetEase(expandEase);
                }
            }
            else
            {
                // Ẩn các menu items
                for (int i = 0; i < itemsCount; i++)
                {
                    menuItems[i].rectTrans.DOAnchorPos(currentPosition, collapseDuration).SetEase(collapseEase);

                    if (i == itemsCount - 1)
                    {
                        Holder.gameObject.SetActive(false);
                    }
                }
            }
        });
    }
    void AdjustSize()
    {
        RectTransform holderRect = Holder.GetComponent<RectTransform>();
        holderRect.anchoredPosition = new Vector2(0, -130f); // Điều chỉnh vị trí
    }
    private void ScaleBackGround(bool active)
    {
        if (mainQuestCtrl != null)
        {
            mainQuestCtrl.AdJustSizeBackground(active, 0, spacing.y * itemsCount); // Điều chỉnh nền
        }
    }
    private void AdJustSizeLvQuest()
    {
        if (isExpanded)
        {
            for (int i = 0; i < mainQuestCtrl.MenuItems.Length; i++)
            {
                if (mainQuestCtrl.MenuItems[i] == this) continue;

                int thisIndex = System.Array.IndexOf(mainQuestCtrl.MenuItems, this);

                if (i < thisIndex) continue;

                RectTransform rectTransform = mainQuestCtrl.MenuItems[i].GetComponent<RectTransform>();
                rectTransform.anchoredPosition += new Vector2(0, -menuItems.Length * 75f);
            }
        }
        else
        {
            for (int i = 0; i < mainQuestCtrl.MenuItems.Length; i++)
            {
                if (mainQuestCtrl.MenuItems[i] == this) continue;

                int thisIndex = System.Array.IndexOf(mainQuestCtrl.MenuItems, this);

                if (i < thisIndex) continue;

                RectTransform rectTransform = mainQuestCtrl.MenuItems[i].GetComponent<RectTransform>();
                rectTransform.anchoredPosition -= new Vector2(0, -menuItems.Length * 75f);
            }
        }
    }
    void PlayButtonPressEffect(System.Action onComplete)
    {
        if (isAnimating) return; // Nếu hiệu ứng đang chạy, không thực hiện gì thêm

        if (tmp_Text == null)
        {
            Debug.LogWarning("TMP_Text is not assigned!");
            onComplete?.Invoke();
            return;
        }
        onComplete?.Invoke();

        Color originalTextColor = tmp_Text.color; // Lưu màu gốc của chữ
        Color highlightColor = new Color(1f, 215f / 255f, 0f); // Vàng Rực Rỡ

        isAnimating = true; // Đánh dấu hiệu ứng đang chạy

        // Hiệu ứng thay đổi màu chữ
        tmp_Text.DOColor(highlightColor, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            tmp_Text.DOColor(originalTextColor, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                isAnimating = false; // Hiệu ứng hoàn thành, cho phép nhấn lại
            });
        });
    }
    public void CheckPlayerLV(int level, LvQuestCtrl lvQuestCtrl)
    {
        bool isLocked = level > PlayerManager.Instance.LvPlayer;
        Color textColor = isLocked
            ? new Color(207 / 255f, 198 / 255f, 198 / 255f, 1f)
            : Color.white;

        lvQuestCtrl.UpdateLockState(isLocked, textColor);
    }
    private void UpdateLockState(bool isLocked, Color textColor)
    {
        lockLv.SetActive(isLocked);
        tmp_Text.color = textColor;
        IsCheckLv = !isLocked;
    }
}
