using UnityEngine;
using DG.Tweening; // Đảm bảo bạn đã thêm thư viện DoTween

public class MoveOnButtonClick : MonoBehaviour
{
    public RectTransform transform1; // Đối tượng đầu tiên
    public RectTransform transform2; // Đối tượng thứ hai
    public CanvasGroup canvas1;


    public float duration = 0.4f;    // Thời gian di chuyển

    public void OnButtonClick()
    {
        // Di chuyển transform1 từ vị trí hiện tại tới -300 từ phải qua trái
        transform1.DOAnchorPosX(-300, duration).SetEase(Ease.InOutQuad);

        transform2.gameObject.SetActive(true);

        // Di chuyển transform2 từ vị trí hiện tại tới 500 từ trái qua phải
        transform2.DOAnchorPosX(500, duration).SetEase(Ease.InOutQuad);

        canvas1.DOFade(1, 0.5f).SetEase(Ease.InOutQuad);

    }
    public void OnClickComPact()
    {
        transform2.DOAnchorPosX(0, duration)
            .SetEase(Ease.InOutQuad);

        transform1.DOAnchorPosX(0, duration).SetEase(Ease.InOutQuad);

        canvas1.DOFade(0, 0.5f).OnComplete(() => canvas1.gameObject.SetActive(false));

    }

}