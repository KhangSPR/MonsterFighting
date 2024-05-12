using UnityEngine;
using UnityEngine.UI; // Đảm bảo bạn đã thêm thư viện này để sử dụng các thành phần UI

public class Fade : DropDownAbstract
{
    public Transform FadeImage;

    // Other variables...

    protected override void Start()
    {
        // Kiểm tra xem FadeImage đã được thiết lập trong Inspector chưa
        if (FadeImage == null)
        {
            Debug.LogError("FadeImage is not assigned in the Inspector.");
            return;
        }

        // Đăng ký sự kiện nhấp vào cho FadeImage
        Button fadeImageButton = FadeImage.GetComponent<Button>();
        if (fadeImageButton != null)
        {
            fadeImageButton.onClick.AddListener(OnFadeImageClick);
            Destroy(dropDownCtrl.CardInventoryUICtrl.SelectCategoryCtrl.ListInfoSelectCtrl.ButtonClickListener.newObject);
        }
        else
        {
            Debug.LogError("FadeImage is missing Button component.");
        }
    }

    // Phương thức được gọi khi nhấp vào FadeImage
    private void OnFadeImageClick()
    {
        // Kiểm tra và toggle FadeImage
        if (dropDownCtrl.GetOBJFade() != null)
        {
            GameObject fadeGameObject = dropDownCtrl.GetOBJFade().gameObject;
            if (fadeGameObject != null)
            {
                ToggleGameObject(fadeGameObject);            }
        }
    }

    // Các phương thức khác...

    private void ToggleGameObject(GameObject gameObjectToToggle)
    {
        // Sử dụng property Toggle của GameObject để đảo ngược trạng thái
        gameObjectToToggle.SetActive(!gameObjectToToggle.activeSelf);
    }

    // Các phương thức khác...
}
