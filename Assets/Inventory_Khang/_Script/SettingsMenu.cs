using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SettingsMenu : MonoBehaviour
{
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

    [Space]
    [Header("Button Colors")]
    [SerializeField] Color expandedColor;
    [SerializeField] Color collapsedColor;
    [SerializeField] float colorChangeDuration = 0.5f;

    [Space]
    [Header("MainSelect")]
    [SerializeField] Button mainButton;
    [SerializeField] Image mainIcon;
    [SerializeField] Image mainIconDefault;
    [SerializeField] ImageRefresh imageRefresh;
    public ImageRefresh ImageRefresh { get { return imageRefresh; } set { imageRefresh = value; } }
    [SerializeField] TMP_Text m_Time; // Reference to the time Text component
    float time;
    public float _Time { get { return time; } set { time = value; } }

    Image mainButtonImage;
    SettingsMenuItem[] menuItems;

    [Space]
    [Header("Holder")]
    [SerializeField] GameObject Prefab;
    [SerializeField] Transform Holder;

    bool isExpanded = false;

    [SerializeField] Vector2 mainButtonPosition;
    int itemsCount;

    public ItemType displayType;

    void Start()
    {
        InventoryManager.Instance.CreateDisplayPlayByType(displayType, Holder, Prefab);

        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButtonImage = mainButton.GetComponent<Image>();
        mainButton.onClick.AddListener(ToggleMenu);
        mainButton.transform.SetAsLastSibling();
        ResetPositions();

    }
    private void Update()
    {
        if(imageRefresh.isCoolingDown)
        {
            UpdateTimeText();
        }
    }
    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = new Vector2(0, 40f);
        }
    }

    public void ToggleSelect()
    {
        if (SelectManager.Instance._MagicRing.activeSelf)
        {
            Debug.Log("ActiveSelf True");
            mainIcon.sprite = mainIconDefault.sprite;
            mainButtonImage.DOColor(collapsedColor, 0f);
            SelectManager.Instance.ItemObject = null;
            SelectManager.Instance.DeactiveSkill();
        }
        CollapseMenuItems();
        imageRefresh.StartCooldown();
        mainButtonImage.DOColor(collapsedColor, colorChangeDuration);
    }

    public void ToggleMenu()
    {
        if (imageRefresh.isCoolingDown) return;

        isExpanded = !isExpanded;

        if (SelectManager.Instance._MagicRing.activeSelf)
        {
            Debug.Log("ActiveSelf True");
            mainIcon.sprite = mainIconDefault.sprite;
            mainButtonImage.DOColor(collapsedColor, 0f);
            SelectManager.Instance.ItemObject = null;
            SelectManager.Instance.DeactiveSkill();
        }

        if (isExpanded)
        {
            ExpandMenuItems();
            mainButtonImage.DOColor(expandedColor, colorChangeDuration);
        }
        else
        {
            CollapseMenuItems();
            mainButtonImage.DOColor(collapsedColor, colorChangeDuration);
        }
    }

    void ExpandMenuItems()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
            menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
        }
    }

    void CollapseMenuItems()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
            menuItems[i].img.DOFade(0f, collapseFadeDuration);
        }
    }

    void CollapseChangeMenuItems()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, 0.1f).SetEase(collapseEase);
            menuItems[i].img.DOFade(0f, 0f);
        }
    }

    public void OnItemSelected(SettingsMenuItem selectedItem)
    {
        isExpanded = !isExpanded;

        mainIcon.sprite = selectedItem.icon.sprite;

        if (!SelectManager.Instance._MagicRing.activeSelf)
        {
            Debug.Log("ActiveSelf False");
            CollapseChangeMenuItems();
        }

        SelectManager.Instance.ActiveSkill();
        SelectManager.Instance.ItemObject = selectedItem.SkillComponent.ItemObject;
    }

    void OnDestroy()
    {
        mainButton.onClick.RemoveListener(ToggleMenu);
    }

    public void UpdateTimeText()
    {
        time -= Time.deltaTime;
        m_Time.text = "" + (int)time;
        if (time < 0)
        {
            time = 0;
            m_Time.text = "";
        }
    }
}

