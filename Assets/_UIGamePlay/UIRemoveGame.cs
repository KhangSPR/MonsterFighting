using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIRemoveGame : UIAbstractGame
{
    [SerializeField] private ParabolicMovement ObjParabolicMovement;
    protected override void Start()
    {
        base.Start();
        if (ObjParabolicMovement.gameObject.activeSelf)
        {
            ObjParabolicMovement.gameObject.SetActive(false);
        }
        transform.GetComponent<Button>().onClick.AddListener(OnClickUIRemove);
    }
    protected override void HandleClickUI()
    {
        if (!GameManager.Instance.AreFlagsSet(GameStateFlags.ClickHoverRemove)) return;
        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickSetting)) return;

        if (ObjParabolicMovement.gameObject.activeSelf)
        {
            ToggleExitUI();
            Hover.Instance.Deactivate();
        }

        if (Input.GetMouseButtonDown(0)) // Bắt đầu drag
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0)) // Kết thúc drag
        {
            if (isDragging)
            {
                HandleObjectCollider();
            }

            isDragging = false;
        }
        if(isDragging)
        {
            _IconUI.enabled = false;
        }
        else
        {
            _IconUI.enabled = true;

        }
        Hover.Instance.FollowMouse(isDragging);
    }

    protected override void HandleObjectCollider()
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
        contactFilter.SetLayerMask(LayerMask.GetMask("ObjUI"));

        int hitCount = Physics2D.OverlapCollider(hoverCollider, contactFilter, colliders);

        Debug.Log($"Đã phát hiện {hitCount} collider va chạm.");

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hitCollider = colliders[i];
            if (hitCollider != null && hitCollider.name == "ObjUI")
            {
                Transform hitColider = hitCollider.transform;

                ObjParabolicMovement.objCtrl = hitCollider.transform.parent.GetComponent<ObjectCtrl>();

                if (ObjParabolicMovement.objCtrl == null)
                {
                    Debug.Log("ObjParabolicMovement OBJ == Nulll");
                }

                ObjParabolicMovement.transform.position = hitColider.position;

                ObjParabolicMovement.gameObject.SetActive(true);

            }
        }
    }
    public void OnClickUIRemove()
    {
        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickInventory | GameStateFlags.ClickTile | GameStateFlags.ClickHoverMove | GameStateFlags.StarCondition)) return;
        if (ObjParabolicMovement.gameObject.activeSelf) return;
        if (GameManager.Instance.CurrentRemove <= 0) return;

        isExpanded = !isExpanded;

        if (isExpanded)
        {
            GameManager.Instance.SetFlag(GameStateFlags.ClickHoverRemove, true);
            _ImgUIRemove.DOColor(expandedColor, colorChangeDuration);
            Hover.Instance.Activate(_spriteUI);
            Hover.Instance.transform.position = transform.position;

        }
        else
        {
            GameManager.Instance.SetFlag(GameStateFlags.ClickHoverRemove, false);
            _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
            Hover.Instance.Deactivate();
            _IconUI.enabled = true;

            //GameManager.Instance.IsClickTile = false;
        }
        Debug.Log("OnClick UI Remove: " + isExpanded);
    }

    public override void ToggleExitUI()
    {

        _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
        GameManager.Instance.SetFlag(GameStateFlags.ClickHoverRemove, false);
        isExpanded = false;

    }
}

