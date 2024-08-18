using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] Image frameButton;
    public Image FrameButton { get { return frameButton; } set { frameButton = value; } }


    Image mainButtonImage;
    SettingsMenuItem[] menuItems;

    [Space]
    [Header("Holder")]
    [SerializeField] GameObject Prefab;
    [SerializeField] Transform Holder;

    [SerializeField] bool isExpanded = false;

    [SerializeField] Vector2 mainButtonPosition;
    int itemsCount;

    public ItemType ItemType;

    [SerializeField] TimeObject timeObject;

    void Start()
    {
        InventoryManager.Instance.CreateDisplayPlayByType(ItemType, Holder, Prefab);

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
    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = new Vector2(0, 40f);
        }
    }

    public void ToggleSelect()
    {
        if (SelectManager.Instance.SelectItem)
        {
            Debug.Log("ActiveSelf True");
            mainIcon.sprite = mainIconDefault.sprite;
            mainButtonImage.DOColor(collapsedColor, 0f);
            SelectManager.Instance.ItemObject = null;
            if (ItemType == ItemType.Skill)
            {
                SelectManager.Instance.DeactivateSkill();
            }
            if (ItemType == ItemType.Medicine)
            {
                SelectManager.Instance.DeactivateMedicine();
            }
        }
        CollapseMenuItems();
        timeObject.ImageRefresh.StartCooldown();
        mainButtonImage.DOColor(collapsedColor, colorChangeDuration);
    }
    public void ToggleOutSide()
    {
        isExpanded = false;
        CollapseMenuItems();
        mainButtonImage.DOColor(collapsedColor, colorChangeDuration);

    }
    public void ToggleMenu()
    {
        
        SelectManager.Instance.SettingsMenu = this;

        if (timeObject.ImageRefresh.isCoolingDown) return; 

        isExpanded = !isExpanded;

        if (SelectManager.Instance.SelectItem)
        {
            Debug.Log("ActiveSelf True");
            mainIcon.sprite = mainIconDefault.sprite;
            mainButtonImage.DOColor(collapsedColor, 0f);
            SelectManager.Instance.ItemObject = null;
            if (ItemType == ItemType.Skill)
            {
                SelectManager.Instance.DeactivateSkill(); //
            }
            if (ItemType == ItemType.Medicine)
            {
                SelectManager.Instance.DeactivateMedicine(); //
            }
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
        isExpanded = false;

        Debug.Log("isExpanded: "+ isExpanded);

        mainIcon.sprite = selectedItem.icon.sprite;

        mainButtonImage.DOColor(expandedColor, colorChangeDuration);
        if(ItemType == ItemType.Skill)
        {
            SelectManager.Instance.ActiveSkill(); //
        }
        if(ItemType == ItemType.Medicine)
        {
            SelectManager.Instance.ActiveMedicine(); //

        }
        SelectManager.Instance.ItemObject = selectedItem.SkillComponent.ItemObject;

    }

    void OnDestroy()
    {
        mainButton.onClick.RemoveListener(ToggleMenu);
    }
    public bool IsPointerOverUIElement(Image targetImage)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == targetImage.gameObject)
            {
                return true; // Chuột đang nằm trên Image FrameButton
            }
        }

        return false; // Chuột không nằm trên bất kỳ UI element nào hoặc không phải FrameButton
    }
}

