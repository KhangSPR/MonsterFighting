using System.Collections;
using UnityEngine;

public class Object_ShakeTransfrom : MonoBehaviour
{
    public Transform shakeTarget; // Đối tượng mà bạn muốn rung
    public float shakeAmount = 0.3f;
    public float shakeDuration = 0.5f;
    public float recoverySpeed = 2f; // Tốc độ hồi phục
    private Vector3 originalPos;
    private Coroutine shakeCoroutine;

    void Start()
    {
        if (shakeTarget == null)
        {
            shakeTarget = transform; // Nếu chưa chỉ định, sử dụng chính đối tượng chứa script này
        }

        originalPos = shakeTarget.position; // Lưu vị trí ban đầu của đối tượng
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShakeAndRecover();
            Debug.Log("Đã Nhấn Chuột");
        }
    }

    public void ShakeAndRecover()
    {
        // Đảm bảo rằng GameObject đang active trước khi bắt đầu coroutine
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("GameObject đang inactive và không thể chạy Coroutine.");
            return;
        }

        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(shakeAndRecover());
    }

    private IEnumerator shakeAndRecover()
    {
        float elapsed = 0f;

        // Giai đoạn rung
        while (elapsed < shakeDuration)
        {
            shakeTarget.position = originalPos + (Vector3)Random.insideUnitSphere * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Giai đoạn hồi phục về vị trí ban đầu
        elapsed = 0f;
        while (shakeTarget.position != originalPos)
        {
            shakeTarget.position = Vector3.Lerp(shakeTarget.position, originalPos, elapsed * recoverySpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeTarget.position = originalPos; // Đảm bảo vị trí cuối cùng là vị trí ban đầu
    }
}
