using System.Collections;
using UnityEngine;

public class ObjMoveInTheCity : AbstractCtrl
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 housePosition; 
    [SerializeField] protected bool isMoveInTheCity = false;
    public bool IsMoveInTheCity { get { return isMoveInTheCity; } set { isMoveInTheCity = value; } }

    protected override void Update()
    {
        base.Update();

        // Di chuyển đến mục tiêu nếu isMoveInTheCity là true
        if (isMoveInTheCity)
        {
            MoveToTargetY();
        }
    }
    public void SetTargetMoveCity(Transform targetCity)
    {
        this.housePosition = targetCity.position;
    }
    private void MoveToTargetY()
    {
        // Lấy tọa độ Y của vị trí nhà mục tiêu
        float targetY = housePosition.y;

        // Tính toán hướng di chuyển dọc trục Y (lên hoặc xuống)
        float directionY = targetY > transform.position.y ? 1f : -1f;

        // Di chuyển theo trục Y về phía targetY
        transform.parent.Translate(Vector3.up * directionY * moveSpeed * Time.deltaTime, Space.World);

        // Nếu đạt tới vị trí Y mục tiêu, chuyển sang giai đoạn 2
        if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
        {
            Debug.Log("Đạt vị trí Y, chuyển sang di chuyển tới nhà.");

            isMoveInTheCity = false;

            // Reset Movement Default
            this.enemyCtrl.ObjMovement.MoveSpeed = 0.65f;

            GameManager.Instance.SetLayerWallCity();


            StartCoroutine(DelayedSetLayerWallCity());
        }
    }
    private IEnumerator DelayedSetLayerWallCity()
    {
        // Đợi 3 giây trước khi gọi SetLayerWallCity
        yield return new WaitForSeconds(3f);

        //GameManager.Instance.CameraMoveLeft.isMoveLeft = true;

        GameManager.Instance.GameLoss();

    }
}
