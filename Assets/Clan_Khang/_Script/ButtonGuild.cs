using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGuild : MonoBehaviour
{
    [SerializeField] private Button btnGuild;
    public Button BtnGuild => btnGuild;
    [SerializeField] private Image image;
    [SerializeField] private Image icon;

    [SerializeField] private Swipe swipeInstance;

    private void Start()
    {
        // Chỉ cần kiểm tra xem button có component Button hay không
        if (btnGuild == null)
        {
            btnGuild = GetComponent<Button>();
        }
    }

    public void SetUI(GuildSO guildSO, Swipe swipe)
    {
        image.sprite = guildSO.GuildImage;
        icon.sprite = guildSO.GuildIcon;
        swipeInstance = swipe; // Lưu tham chiếu đến swipe instance

        // Đăng ký sự kiện click
        btnGuild.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        if (swipeInstance != null)
        {
            swipeInstance.WhichBtnClicked(btnGuild);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện click để tránh lỗi nếu object bị hủy
        btnGuild.onClick.RemoveListener(OnButtonClick);
    }
}
