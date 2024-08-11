using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{
    [HideInInspector] public Image img;
    [HideInInspector] public RectTransform rectTrans;
    [HideInInspector] public Image icon;
    [SerializeField] SkillComponent skillComponent;
    public SkillComponent SkillComponent => skillComponent;

    //SettingsMenu reference
    SettingsMenu settingsMenu;

    //item button
    Button button;

    void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        img = GetComponent<Image>();
        rectTrans = GetComponent<RectTransform>();

        settingsMenu = rectTrans.parent.GetComponent<SettingsMenu>();

        //add click listener
        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }

    void OnItemClick()
    {
        settingsMenu.OnItemSelected(this);
    }

    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        button.onClick.RemoveListener(OnItemClick);
    }
}
