using CodeMonkey.Utils;
using System;
using UnityEngine;

public class ButtonClan : MonoBehaviour
{
    [SerializeField] private BtnUI buttonUI;   // Logic UI liên kết với nút
    public BtnUI ButtonUI => buttonUI;

    public static event Action<Vector2> CardClicked; // Sự kiện được kích hoạt khi nút được nhấn

    public void OnclickBtn()
    {
        SetButtonState(GuildManager.Instance.GuildJoined);
        HandleButtonClick();
    }
    private void HandleButtonClick()
    {
        buttonUI.OnClickEvent();

        // Kiểm tra nếu đã tham gia Guild
        if (GuildManager.Instance.GuildJoined == null)
        {
            TriggerAction();
        }
    }

    private void SetButtonState(bool isClickable)
    {
        buttonUI.SetConditionToClick(isClickable);
    }

    private void TriggerAction()
    {
        // Lấy vị trí trên màn hình của RectTransform
        Vector2 screenPos = buttonUI.GetComponent<RectTransform>().position;

        // Gửi sự kiện CardClicked
        CardClicked?.Invoke(screenPos);
    }
}
