using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : UIAbstractGame
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
    [Header("MainSelect")]
    [SerializeField] Button mainButton;
    [SerializeField] Image mainIcon;
    [SerializeField] Image mainIconDefault;
    [SerializeField] Image frameButton;

    Image mainButtonImage;
    SettingsMenuItem[] menuItems;

    [Space]
    [Header("Holder")]
    [SerializeField] GameObject Prefab;
    [SerializeField] Transform Holder;

    [SerializeField] Vector2 mainButtonPosition;
    int itemsCount;

    [SerializeField] TimeObject timeObject;
    [Space]
    [Header("Obj Skill")]
    [SerializeField] private GameObject _magicRing;
    public GameObject MagicRing => _magicRing;

    [SerializeField] protected ItemObject itemObject;
    [SerializeField] protected bool isSelectItem = false;
    private bool isSkillReset = false;
    #region Current Function
    protected override void Start()
    {
        //InGame Inventory

        InventoryManager.Instance.CreateDisplayPlayByType(Holder, Prefab);

        itemsCount = Holder.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = Holder.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButtonImage = mainButton.GetComponent<Image>();
        mainButton.onClick.AddListener(OnClickMainFrame);
        mainButton.transform.SetAsLastSibling();
        ResetPositions();

    }
    void OnDestroy()
    {
        mainButton.onClick.RemoveListener(OnClickMainFrame);
    }
    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = new Vector2(0, 40f);
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
    #endregion
    #region OnClick Main Frame
    public void OnClickMainFrame()
    {
        if (isSkillReset)
        {
            isSkillReset = false; // Reset trạng thái sau khi kiểm tra
            return;
        }


        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickHoverRemove | GameStateFlags.ClickTile | GameStateFlags.ClickHoverMove | GameStateFlags.StarCondition)) return;
        if (isSelectItem) return;
        if (timeObject.ImageRefresh.isCoolingDown) return;

        isExpanded = !isExpanded;

        if (isExpanded)
        {
            ExpandMenuItems();
            mainButtonImage.DOColor(expandedColor, colorChangeDuration);
        }
        else
        {
            CollapseMenuItems();
            mainButtonImage.DOColor(collapsedColor, colorChangeDuration);
            GameManager.Instance.SetFlag(GameStateFlags.ClickInventory, false);
        }

        Debug.Log("OnClick Main Frame");
    }
    #endregion
    #region On Click Item
    public void OnItemSelected(SettingsMenuItem selectedItem)
    {
        mainIcon.sprite = selectedItem.icon.sprite;
        itemObject = selectedItem.SkillComponent.ItemObject;

        GameManager.Instance.SetFlag(GameStateFlags.ClickInventory, true);


        HandleItemSelect();
    }
    private void HandleItemSelect()
    {
        if(itemObject.type == InventoryType.Medicine)
        {
            Hover.Instance.Activate(itemObject.Sprite);
        }
        isExpanded = false;
        CollapseMenuItems();
        mainButtonImage.DOColor(expandedColor, colorChangeDuration);
        isSelectItem = true;

    }
    #endregion

    protected override void HandleClickUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIElement(frameButton) && !isSelectItem)
            {
                isSkillReset = false; // Đặt trạng thái để chặn OnClickMainFrame
                ToggleExitUI();
                Debug.Log("Reset Toggle Skill");
                return;
            }
        }

        if (!GameManager.Instance.AreFlagsSet(GameStateFlags.ClickInventory)) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIElement(frameButton) && isSelectItem)
            {
                isSkillReset = true;
                ToggleExitUI();
                GameManager.Instance.SetFlag(GameStateFlags.ClickInventory, false);
                Hover.Instance.Deactivate();

                Debug.Log("Reset Toggle Skill");
                return;
            }
            Debug.Log("Down");
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Up");
            if(isDragging)
            {
                HandleObjectCollider();
            }
            isDragging = false;
        }

        Debug.Log("Call Skill UI Click");

        if (isSelectItem)
        {
            if(itemObject!= null)
            {
                if(itemObject.type == InventoryType.Skill)
                {
                    _magicRing.SetActive(isDragging);
                }
                else if(itemObject.type == InventoryType.Medicine)
                {
                    Hover.Instance.FollowMouse(isDragging);
                }
            }
        }
    }


    protected override void HandleObjectCollider()
    {
        //else
        if (isSelectItem && !IsPointerOverUIElement(frameButton))
        {
            TryInstantiateSkill();

            Debug.Log("Set");
        }
    }

    public override void ToggleExitUI()
    {
        _magicRing.SetActive(false);
        isExpanded = false;
        CollapseMenuItems();
        mainIcon.sprite = mainIconDefault.sprite;
        itemObject = null;
        mainButtonImage.DOColor(collapsedColor, colorChangeDuration);

        isSelectItem = false;
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
    #region Handle Skill
    public void TryInstantiateSkill()
    {
        if (itemObject is SkillObject skillObject)
        {
            HandleSkillObject(skillObject);
        }
        else if (itemObject is MedicineObject medicineObject)
        {
            HandleMedicineObject(medicineObject);
        }
        GameManager.Instance.SetFlag(GameStateFlags.ClickInventory, false);
        Hover.Instance.Deactivate();

        ToggleExitUI();
    }
    protected void HandleSkillObject(SkillObject skillObject)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Adjust depth as needed
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.y = skillObject.positionSpawn;

        Transform newSkill = SkillSpawner.Instance.Spawn(SkillSpawner.Instance.GetSkillType(skillObject.skillType), worldPosition, Quaternion.identity);

        if (newSkill.gameObject.TryGetComponent(out ParticleCtrl particleCtrl))
        {
            particleCtrl.particleDamesender.Damage = skillObject.damage;
        }

        if (newSkill.gameObject.GetComponentInChildren<ParticleSystem>() is ParticleSystem particleSystem)
        {
            var emission = particleSystem.emission;
            emission.rateOverTime = 0;

            StartCoroutine(EmitParticlesAndDestroy(particleSystem, skillObject.particleCount, skillObject.timeSpawn, newSkill));
        }
        Debug.Log($"Spawn VFX Skill at Y = {worldPosition.y}");

        if (timeObject != null)
        {
            timeObject._Time = skillObject.coolDown;
            timeObject.ImageRefresh.cooldownDuration = skillObject.coolDown;
            timeObject.ImageRefresh.StartCooldown();
        }

        newSkill.gameObject.SetActive(true);
    }
    protected void HandleMedicineObject(MedicineObject medicineObject)
    {
        // Lấy Collider của Hover
        Collider2D hoverCollider = Hover.Instance.GetComponent<Collider2D>();
        if (hoverCollider == null)
        {
            Debug.LogWarning("Hover.Instance không có Collider2D!");
            return;
        }

        // Reset collider để bảo đảm trạng thái chính xác
        hoverCollider.enabled = false;
        hoverCollider.enabled = true;

        // Tạo mảng để lưu các collider bị va chạm
        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D contactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = true
        };
        contactFilter.SetLayerMask(LayerMask.GetMask("ObjMedicine"));

        int hitCount = Physics2D.OverlapCollider(hoverCollider, contactFilter, colliders);

        Debug.Log($"Đã phát hiện {hitCount} collider va chạm.");

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hitCollider = colliders[i];
            if (hitCollider != null && hitCollider.name == "ObjMedicine")
            {
                Vector3 targetPosition = hitCollider.transform.position;

                Transform newSkill = SkillSpawner.Instance.Spawn(SkillSpawner.Instance.GetMedicineType(medicineObject.medicineType), targetPosition, Quaternion.identity);

                if (newSkill.gameObject.GetComponent<ParticleSystem>() is ParticleSystem particleSystem)
                {
                    var emission = particleSystem.emission;
                    emission.rateOverTime = 0;

                    StartCoroutine(DestroyAfterDelay(newSkill, 1f));
                }

                newSkill.gameObject.SetActive(true);

                if (timeObject != null)
                {
                    timeObject._Time = medicineObject.coolDown;
                    timeObject.ImageRefresh.cooldownDuration = medicineObject.coolDown;
                    timeObject.ImageRefresh.StartCooldown();
                }
                PlayerCtrl playerCtrl = hitCollider.transform.parent.GetComponent<PlayerCtrl>();

                playerCtrl.ObjectDamageReceiver.AddPoint(medicineObject.addPoint, medicineObject.medicineType);
            }
        }
    }
    private IEnumerator DestroyAfterDelay(Transform VFX, float delay)
    {
        yield return new WaitForSeconds(delay + 1f); // Thêm 1 giây chờ
        SkillSpawner.Instance.Despawn(VFX);
    }
    private IEnumerator EmitParticlesAndDestroy(ParticleSystem particleSystem, int totalParticles, float duration, Transform VFX)
    {
        float particlesPerSecond = totalParticles / duration;
        float interval = 1f / particlesPerSecond;

        for (int emittedParticles = 0; emittedParticles < totalParticles; emittedParticles++)
        {
            particleSystem.Emit(1);
            Debug.Log("Emitted: " + emittedParticles);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(Mathf.Max(0, duration - (totalParticles * interval)));
        yield return new WaitForSeconds(5f);


        SkillSpawner.Instance.Despawn(VFX);
    }
    #endregion
}

