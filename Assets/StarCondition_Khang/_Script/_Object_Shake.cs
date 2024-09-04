using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Object_Shake : MonoBehaviour
{
    public float shakeAmount = 0.3f;
    private Vector2 originalPos;
    public bool _Is_Shake = false;

    // Sử dụng RectTransform thay vì Transform
    private RectTransform rectTransform;

    void Start()
    {
        // Lấy RectTransform của đối tượng UI
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (_Is_Shake)
        {
            // Rung đối tượng bằng cách thay đổi anchoredPosition
            rectTransform.anchoredPosition = originalPos + (Vector2)Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            // Đưa vị trí về trạng thái ban đầu
            rectTransform.anchoredPosition = originalPos;
        }
    }

    public void _Shake(bool _Is)
    {
        _Is_Shake = _Is;
    }
}
