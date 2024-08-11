using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : SaiMonoBehaviour
{
    private static SelectManager _instance;
    public static SelectManager Instance => _instance;

    [SerializeField] private ItemObject _item;
    public ItemObject ItemObject { get { return _item; } set { _item = value; } }

    [SerializeField] private GameObject _magicRing;
    public GameObject _MagicRing => _magicRing;

    [SerializeField] private SettingsMenu settingsMenu;


    protected override void Awake()
    {
        base.Awake();
        if (_instance != null)
        {
            Debug.LogError("Only one instance of SelectManager is allowed!");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    protected override void Update()
    {
        if (_MagicRing.activeSelf && Input.GetMouseButtonDown(0))
        {
            TryInstantiateSkill();
        }
    }

    private void TryInstantiateSkill()
    {
        if (_item is SkillObject skillObject)
        {
            //SkillObject SkillObject = (SkillObject)_item;
            // Lấy vị trí chuột trong không gian thế giới
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Độ sâu của camera, điều chỉnh theo nhu cầu
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Điều chỉnh vị trí theo trục Y
            float fixedYPosition = 5f; // Bạn có thể thay đổi giá trị Y cố định này
            worldPosition.y = fixedYPosition;
            //worldPosition.x = 0;

            // Instance gameobjectVFX tại vị trí đã điều chỉnh
            GameObject VFX = Instantiate(skillObject.gameobjectVFX, worldPosition, Quaternion.identity);

            VFX.SetActive(true);
            Debug.Log("Spawn VFX Skill at Y = " + fixedYPosition);


            Destroy(VFX, skillObject.timeSpawn);

            settingsMenu._Time = skillObject.coolDown;
            settingsMenu.ImageRefresh.cooldownDuration = skillObject.coolDown;
            settingsMenu?.ToggleSelect();
        }
    }

    public void ActiveSkill()
    {
        _MagicRing.SetActive(true);
    }

    public void DeactiveSkill()
    {
        _MagicRing.SetActive(false);
    }
}
