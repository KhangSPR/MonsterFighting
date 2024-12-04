using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveGame : UIAbstractGame
{
    [SerializeField] GameObject tileEffect;
    protected override void Start()
    {
        base.Start();
        transform.GetComponent<Button>().onClick.AddListener(OnClickUIRemove);
    }
    protected override void OnEnable()
    {
        GridHoverEffect.UIMoveGame += ToggleExitUI;
    }
    protected override void OnDisable()
    {
        GridHoverEffect.UIMoveGame -= ToggleExitUI;

    }
    protected override void HandleClickUI()
    {
        if (!GameManager.Instance.AreFlagsSet(GameStateFlags.ClickHoverMove)) return;
        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickSetting)) return;


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
        if (isDragging)
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
                Debug.Log($"Va chạm với {hitCollider.name} có tag Player");

                PlayerCtrl playerCtrl = hitCollider.GetComponentInParent<PlayerCtrl>();
                if (playerCtrl != null)
                {
                    // Đổi sprite của hover
                    Hover.Instance.Activate(playerCtrl.CardCharacter.avatar);

                    // Cập nhật trạng thái
                    GameManager.Instance.ObjSwapMove = playerCtrl.gameObject;
                    GameManager.Instance.SetFlag(GameStateFlags.ClickTile, true);

                    tileEffect.transform.position = playerCtrl.ObjTile.TileTower.transform.position;
                    // Thay đổi màu UI
                    tileEffect.SetActive(true);
                    Debug.Log("Đã xử lý collider của Player.");
                }
            }
        }
    }

    public void OnClickUIRemove()
    {

        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickInventory | GameStateFlags.ClickHoverRemove | GameStateFlags.StarCondition /*| GameStateFlags.ClickTile) */) ) return;
        if (GameManager.Instance.CurrentMove <= 0) return;

        if(GameManager.Instance.AreFlagsSet(GameStateFlags.ClickTile))
        {
            if (GameManager.Instance.ClickBtn != null) return;
            ToggleExitUI();
            GameManager.Instance.SetFlag(GameStateFlags.ClickTile, false);
            Hover.Instance.Deactivate();

            return;
        }

        isExpanded = !isExpanded;

        if (isExpanded)
        {
            GameManager.Instance.SetFlag(GameStateFlags.ClickHoverMove, true);
            _ImgUIRemove.DOColor(expandedColor, colorChangeDuration);
            Hover.Instance.transform.position = transform.position;
            Hover.Instance.Activate(_spriteUI);

        }
        else
        {
            GameManager.Instance.SetFlag(GameStateFlags.ClickHoverMove, false);
            _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
            Hover.Instance.Deactivate();
            _IconUI.enabled = true;

        }
        Debug.Log("OnClick UI Remove: " + isExpanded);
    }

    public override void ToggleExitUI()
    {
        _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
        GameManager.Instance.SetFlag(GameStateFlags.ClickHoverMove, false);
        isExpanded = false;
        _IconUI.enabled = true;
        tileEffect.SetActive(false);

    }
}
