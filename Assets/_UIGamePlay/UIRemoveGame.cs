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
    protected override void HandleClickRemoveMove()
    {
        if (!GameManager.Instance.IsClickHover) return;
        if (ObjParabolicMovement.gameObject.activeSelf)
        {
            GameManager.Instance.IsClickHover = false;
            _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
            Hover.Instance.Deactivate();
            GameManager.Instance.IsClickTile = false;

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

        if (isDragging)
        {

            Debug.Log("DetectHover");
        }
        Hover.Instance.FollowMouse(isDragging);
    }

    protected override void HandleObjectCollider()
    {
        Collider2D medicineCollider = Hover.Instance.GetComponent<Collider2D>();

        if (medicineCollider == null)
        {
            Debug.LogWarning("Collider2D không gắn trên _objMedicine.");
        }

        // Tạo một mảng để lưu các collider bị va chạm
        Collider2D[] colliders = new Collider2D[5]; // Có thể thay đổi kích thước tùy theo nhu cầu
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true; // Nếu bạn sử dụng các trigger colliders

        // Kiểm tra va chạm
        int hitCount = Physics2D.OverlapCollider(medicineCollider, contactFilter, colliders);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hitCollider = colliders[i];

            if (hitCollider.transform.name == "ObjRemove")
            {
                Transform hitColider = hitCollider.transform;

                ObjParabolicMovement.objCtrl = hitCollider.transform.parent.GetComponent<ObjectCtrl>();

                if (ObjParabolicMovement.objCtrl == null)
                {
                    Debug.Log("ObjParabolicMovement OBJ == Nulll");
                }

                ObjParabolicMovement.transform.position = hitColider.position;

                ObjParabolicMovement.gameObject.SetActive(true);

                isExpanded = false;
            }
        }
    }
    public void OnClickUIRemove()
    {
        if (ObjParabolicMovement.gameObject.activeSelf) return;
        if (GameManager.Instance.CurrentRemove <= 0) return;

        isExpanded = !isExpanded;

        if (isExpanded)
        {
            GameManager.Instance.IsClickHover = true;
            _ImgUIRemove.DOColor(expandedColor, colorChangeDuration);
            Hover.Instance.Activate(_spriteRemove);
        }
        else
        {
            GameManager.Instance.IsClickHover = false;
            _ImgUIRemove.DOColor(collapsedColor, colorChangeDuration);
            Hover.Instance.Deactivate();
            //GameManager.Instance.IsClickTile = false;
        }
        Debug.Log("OnClick UI Remove: " + isExpanded);
    }
}

