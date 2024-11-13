using DG.Tweening;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UILoseGameCtrl : MonoBehaviour
{
    public RectTransform explosionVFX; 


    public float appearScale = 1.5f;     // Tỷ lệ phóng to khi xuất hiện
    public float animationDuration = 0.5f; // Thời gian để hoàn tất hiệu ứng (2.5 giây)
    public RectTransform headSkull;
    public RectTransform cartoonFire;
    public RectTransform RewardHolder;
    public RectTransform TitleGameFinish;
    public RectTransform HolderBtn;
    private void Awake()
    {
        UIGameStart();
    }
    void UIGameStart()
    {
        cartoonFire.gameObject.SetActive(false);
        RewardHolder.gameObject.SetActive(false);
        TitleGameFinish.gameObject.SetActive(false);
        HolderBtn.gameObject.SetActive(false);
        DoAnimation();
    }
    [ContextMenu("DoAnimation")]
    protected void DoAnimation()
    {
        StartCoroutine(PlayAppearAnimation());
    }

    private IEnumerator PlayAppearAnimation()
    {
        // Bật hiệu ứng VFX ngay trước khi phóng to
        if (explosionVFX != null)
        {
            explosionVFX.gameObject.SetActive(true);
        }

        // Tạo tween phóng to đối tượng lên appearScale và đợi hoàn tất
        yield return headSkull.DOScale(appearScale, animationDuration)
            .SetEase(Ease.OutBack) // Chọn easing để có hiệu ứng bật lại
            .WaitForCompletion();

        // Sau khi phóng to, tạo tween thu nhỏ lại về tỷ lệ 1 (Vector3.one) với thời gian nhanh hơn
        yield return headSkull.DOScale(Vector3.one, 0.35f)
            .SetEase(Ease.OutBack) // Hiệu ứng thu nhỏ về lại
            .WaitForCompletion();

        // Thực hiện di chuyển sau khi thu nhỏ xong
        yield return headSkull.DOMove(headSkull.transform.position + headSkull.up * 2.3f, 0.3f)
            .SetEase(Ease.Linear)
            .SetDelay(0.2f)
            .WaitForCompletion();

        cartoonFire.gameObject.SetActive(true);
        RewardHolder.gameObject.SetActive(true);

        RewardHolder.localScale = new Vector3(0, 1, 1);
        yield return RewardHolder.DOScale(Vector3.one, 1.3f).WaitForCompletion();
        TitleGameFinish.gameObject.SetActive(true);
        HolderBtn.gameObject.SetActive(true);
    }
}
