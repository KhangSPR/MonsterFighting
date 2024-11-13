using UnityEngine;

public class CameraMoveLeft : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Tốc độ di chuyển của camera
    public float targetX = -1f;       // Vị trí trục X mà camera sẽ dừng lại
    public bool isMoveLeft = false;

    void Update()
    {
        if (isMoveLeft)
        {
            // Di chuyển camera từ vị trí hiện tại đến minX
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            // Kiểm tra nếu camera đã đạt đến vị trí minX
            if (transform.position.x <= targetX)
            {
                isMoveLeft = false; // Dừng di chuyển khi đạt đến minX
            }

            Debug.Log("Move Left");
        }
    }
}
