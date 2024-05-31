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

    [Header("Star")]
    [SerializeField] GameObject StarPrefabs;
    [SerializeField] Transform StarHolder;

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
    public void SetCardInfo(GameObject cardObject, CardCharacter card, GameIconsSO gameIconsSO)
    {
        //Frame
        frame.sprite = card.frame;
        background.sprite = card.background;
        nameCard.text = card.name;

        InstantiateStart(card, gameIconsSO);
    }
    void InstantiateStart(CardCharacter card, GameIconsSO gameIconsSO)
    {
        foreach(Transform star in StarHolder)
        {
            Destroy(star.gameObject);
        }

        for (int i = 0; i < card.Star; i++)
        {
            
            GameObject newObject = Instantiate(StarPrefabs, StarHolder);

            newObject.GetComponent<Image>().sprite = gameIconsSO.GetStarIcon(card.rarityCard);
        }
    }
    protected void OnButtonClick()
    {
        CardUIPanelManager.Instance.CheckCardTypeAndProcess(idCard, this);
    }
}
