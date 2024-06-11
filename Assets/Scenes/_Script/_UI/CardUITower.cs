using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardUITower : MonoBehaviour
{
    [Header("Frame")]
    [SerializeField] Image frame;
    [SerializeField] Image background;
    [SerializeField] TMP_Text nameCard;


    public int idCard;
    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        // Thêm hàm OnButtonClick() vào sự kiện click của Button
        button.onClick.AddListener(OnButtonClick);
    }
    public void SetCardInfo(CardCharacter card)
    {
        //Frame
        frame.sprite = card.frame;
        background.sprite = card.background;
        nameCard.text = card.name;

    }
    protected void OnButtonClick()
    {
        CardUIPanelManager.Instance.CheckCardTypeAndProcess(idCard, this);
    }
}
