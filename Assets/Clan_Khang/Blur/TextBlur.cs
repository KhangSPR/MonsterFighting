using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBlur : MonoBehaviour
{
    public List<TMP_Text> textsToBlur; // Danh sách các đối tượng Text cần làm mờ
    public float blurAmount = 0.2f; // Độ mờ muốn áp dụng

    public void BlurTexts()
    {
        // Lặp qua danh sách Text và gọi hàm làm mờ
        foreach (var text in textsToBlur)
        {
            if (text != null)
            {
                BlurText(text);
            }
        }
    }

    public void ClearBlur()
    {
        // Lặp qua danh sách Text và gọi hàm hủy bỏ làm mờ
        foreach (var text in textsToBlur)
        {
            if (text != null)
            {
                ClearTextBlur(text);
            }
        }
    }

    void BlurText(TMP_Text text)
    {
        // Tạo một bản sao của màu sắc hiện tại của văn bản và làm mờ nó
        Color blurredColor = text.color;
        blurredColor.a = blurAmount; // Đặt độ trong suốt để làm mờ văn bản
        text.color = blurredColor;
    }

    void ClearTextBlur(TMP_Text text)
    {
        // Đặt lại màu sắc ban đầu để hủy bỏ hiệu ứng làm mờ
        Color originalColor = text.color;
        originalColor.a = 1f; // Đặt độ trong suốt về mức ban đầu
        text.color = originalColor;
    }
}
