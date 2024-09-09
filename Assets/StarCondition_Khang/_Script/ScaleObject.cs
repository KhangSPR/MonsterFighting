using DG.Tweening;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    void Start()
    {
        // Đặt scale ban đầu của GameObject về 0
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);

        // Sử dụng DoTween để tăng scale lên 300 trên trục X trong 0.3 giây
        transform.DOScaleX(300, 0.25f);
    }
}
