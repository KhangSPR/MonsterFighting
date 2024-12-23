using UnityEngine;
using TMPro;
using DG.Tweening; // Thêm thư viện DoTween
using UIGameDataManager;
using System;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI ui; // Hiển thị năng lượng
    [SerializeField] RectTransform energyUI; // UI năng lượng
    [SerializeField] float animationDuration = 0.5f; // Thời gian hiệu ứng

    private bool isAnimating = false; // Kiểm tra hiệu ứng có đang chạy không

    public static Action OnClickEnergy;

    public static event Action<Vector2> LevelInfoClicked; 
    private void OnEnable()
    {
        // Lắng nghe sự kiện khi năng lượng thay đổi
        GameDataManager.Instance.OnEnergyChanged += UpdateEnergyTextCurrent;
    }

    private void OnDisable()
    {
        // Hủy lắng nghe sự kiện khi đối tượng bị tắt
        GameDataManager.Instance.OnEnergyChanged -= UpdateEnergyTextCurrent;
    }
    private void Start()
    {
        UpdateEnergyTextCurrent();
    }
    // Gọi khi nhấn nút OnClick
    public void OnEnergyButtonClick() //Repair
    {
        if(GameDataManager.Instance.CurrentEnergyAmount <=0)
        {
            TriggerAction();
        }

        if (isAnimating) return; // Nếu đang chạy animation thì không thực hiện thêm
        if (GameDataManager.Instance.CurrentEnergyAmount <= 0) return; // Không đủ năng lượng

        energyUI.gameObject.SetActive(true);
        // Hiển thị UI năng lượng với hiệu ứng
        isAnimating = true;
        energyUI.DOScale(Vector3.one, animationDuration) // Mở rộng UI
                .From(Vector3.zero) // Bắt đầu từ kích thước 0
                .SetEase(Ease.OutBack) // Hiệu ứng bật nhẹ
                .OnComplete(() =>
                {
                    energyUI.gameObject.SetActive(false);

                    OnClickEnergy?.Invoke();

                    isAnimating = false; // Kết thúc hiệu ứng

                });

        Debug.Log("OnEnergyButtonClick");
        // Cập nhật UI text
    }
    private void TriggerAction()
    {
        // Lấy vị trí trên màn hình của RectTransform
        Vector2 screenPos = energyUI.GetComponent<RectTransform>().position;

        // Gửi sự kiện CardClicked
        LevelInfoClicked?.Invoke(screenPos);
    }
    private void UpdateEnergyTextCurrent()
    {
        ui.text = string.Format("{0}/5", GameDataManager.Instance.CurrentEnergyAmount);
    }
}
