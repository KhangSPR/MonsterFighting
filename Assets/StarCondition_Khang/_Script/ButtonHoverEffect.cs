using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;
    public float hoverScale = 1.2f;  // Kích thước khi hover vào
    public float duration = 0.3f;    // Thời gian co dãn
    private int tweenId;
    private void Start()
    {
        originalScale = transform.localScale;  // Lưu kích thước ban đầu
    }

    public void OnPointerEnter()
    {
        // Dừng tween nếu có đang chạy
        LeanTween.cancel(tweenId);

        // Phóng to từ kích thước hiện tại đến kích thước lớn nhất
        tweenId = LeanTween.scale(gameObject, originalScale * hoverScale, duration)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong()  // Thực hiện hiệu ứng ping-pong
            .setIgnoreTimeScale(true)  // Sử dụng thời gian thực
            .id;  // Lưu ID của tween
    }

    public void OnPointerExit()
    {
        // Dừng tween nếu có đang chạy và đặt lại kích thước về kích thước ban đầu
        LeanTween.cancel(tweenId);

        // Thu nhỏ về kích thước ban đầu
        LeanTween.scale(gameObject, originalScale, duration)
            .setEase(LeanTweenType.easeInOutSine)
            .setIgnoreTimeScale(true);  // Sử dụng thời gian thực
    }
}
