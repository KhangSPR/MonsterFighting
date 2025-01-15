using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VFXCraft : MonoBehaviour
{
    [SerializeField] Image item;
    public Image Image => item;
    [SerializeField] TMP_Text countText;
    public TMP_Text CountText => countText;

    [SerializeField] ParticleSystem craftParticle;
    [SerializeField] CraftUI craftUI;
    private void Update()
    {
        if (craftUI.LastClickTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void BonusEffect()
    {

        // Đặt scale ban đầu cho item là 0
        item.transform.localScale = Vector3.zero;

        // Hiệu ứng scale cho item (image của item được tạo)
        item.transform.DOScale(
            endValue: Vector3.one, // Scale về kích thước ban đầu
            duration: 0.5f // Thời gian cho hiệu ứng scale
        ).SetEase(Ease.OutBounce); // Easing mượt mà

        // Hiệu ứng scale cho countText
        countText.transform.DOScale(
            endValue: 1f,
            duration: 0.5f // Thời gian cho hiệu ứng scale
        ).From(
            fromValue: 1.2f // Bắt đầu từ kích thước lớn hơn
        ).SetEase(Ease.InCubic); // Kiểu easing
    }



    public void CraftEffect()
    {
        gameObject.SetActive(true);

        craftParticle.Play();
    }
}
