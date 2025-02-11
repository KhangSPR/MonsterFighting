using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCtrl : SkillCtrl
{
    [Header("Laser Settings")]
    public LayerMask targetLayer;
    public float laserRange = 5f;

    [Header("Line Renderer Settings")]
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private BoxCollider2D boxCollider;
    protected override void OnEnable()
    {
        base.OnEnable();
        SkillAction();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

    }
    public override void SkillAction()
    {
        CallEmitLaserObject();
    }

    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }

        if (damageReceiver.IsDead) return;


        //Add Skill
        this.DamageSender.SendFXImpact(damageReceiver, objectCtrl);

        //Effect
        objectCtrl.ObjectDamageReceiver.StartStun();
    }
    private void CallEmitLaserObject()
    {
        laserLine.enabled = false;
        FireLaser();
    }

    private void FireLaser()
    {
        Collider2D target = GetClosestTarget();
        if (target == null)
        {
            boxCollider.enabled = false; // Tắt collider nếu không có mục tiêu
            return;
        }
        Vector2 start = transform.position;
        Vector2 end = target.transform.position;
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // Nhân chiều dài thêm 50 lần
        Vector2 extendedEnd = start + direction * (distance * 20f);

        laserLine.enabled = true;
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, extendedEnd);

        // Cập nhật BoxCollider2D và đặt vị trí trùng với laser beam
        UpdateCollider(start, extendedEnd);
    }

    private void UpdateCollider(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        // Nhân chiều dài lên 50 lần giống với LineRenderer
        float scaledDistance = distance;

        // Đặt vị trí BoxCollider nằm chính giữa Line Laser Beam
        boxCollider.transform.position = (start + end) / 2f;

        // Cập nhật kích thước BoxCollider2D
        boxCollider.size = new Vector2(scaledDistance, 0.2f); // 0.2f là chiều rộng (có thể chỉnh)

        // Xoay collider theo hướng laser
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        boxCollider.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Bật Collider
        boxCollider.enabled = true;
    }

    private Collider2D GetClosestTarget()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, laserRange, targetLayer);
        Collider2D closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D col in nearbyColliders)
        {
            ObjectCtrl objectCtrl = col.transform.parent?.GetComponent<ObjectCtrl>();

            // Bỏ qua nếu không có ObjectCtrl hoặc khác LandIndex
            if (objectCtrl == null || objectCtrl.ObjLand.LandIndex != this.objectCtrl.ObjLand.LandIndex)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col;
            }
        }
        return closest;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, laserRange);
    }
}
