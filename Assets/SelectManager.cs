using System.Collections;
using UnityEngine;
using UIGameDataMap;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class SelectManager : SaiMonoBehaviour
{
    private static SelectManager _instance;
    public static SelectManager Instance => _instance;

    [SerializeField] private ItemObject _item;
    public ItemObject ItemObject
    {
        get => _item;
        set => _item = value;
    }

    [SerializeField] private GameObject _magicRing;
    public GameObject MagicRing => _magicRing;

    [SerializeField] private GameObject _objMedicine;
    public GameObject ObjMedicine => _objMedicine;

    [SerializeField] private SettingsMenu settingsMenu = null;
    public SettingsMenu SettingsMenu
    {
        get => settingsMenu;
        set => settingsMenu = value;
    }

    private bool selectItem;
    public bool SelectItem => selectItem;

    protected override void Awake()
    {
        base.Awake();

        //if (_instance != null)
        //{
        //    Debug.LogError("Only one instance of SelectManager is allowed!");
        //    Destroy(gameObject);
        //    return;
        //}
        _instance = this;
    }

    protected override void Update()
    {
        if (settingsMenu == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!selectItem && !settingsMenu.IsPointerOverUIElement(settingsMenu.FrameButton))
            {
                settingsMenu.ToggleOutSide();
            }
            else if (selectItem && !settingsMenu.IsPointerOverUIElement(settingsMenu.FrameButton))
            {
                TryInstantiateSkill();
            }
        }
    }
    protected void TryInstantiateSkill()
    {
        if (_item is SkillObject skillObject)
        {
            HandleSkillObject(skillObject);
        }
        else if (_item is MedicineObject medicineObject)
        {
            HandleMedicineObject(medicineObject);
        }

        settingsMenu?.ToggleSelect();
    }
    protected void HandleSkillObject(SkillObject skillObject)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Adjust depth as needed
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.y = skillObject.positionSpawn;

        Transform newSkill = SkillSpawner.Instance.Spawn(SkillSpawner.Instance.GetSkillType(skillObject.skillType), worldPosition, Quaternion.identity);

        if (newSkill.gameObject.TryGetComponent(out ParticleCtrl particleCtrl))
        {
            particleCtrl.particleDamesender.Damage = skillObject.damage;
        }

        if (newSkill.gameObject.GetComponentInChildren<ParticleSystem>() is ParticleSystem particleSystem)
        {
            var emission = particleSystem.emission;
            emission.rateOverTime = 0;

            StartCoroutine(EmitParticlesAndDestroy(particleSystem, skillObject.particleCount, skillObject.timeSpawn, newSkill));
        }
        Debug.Log($"Spawn VFX Skill at Y = {worldPosition.y}");

        if (settingsMenu.TryGetComponent(out TimeObject timeSkill))
        {
            timeSkill._Time = skillObject.coolDown;
            timeSkill.ImageRefresh.cooldownDuration = skillObject.coolDown;
            timeSkill.ImageRefresh.StartCooldown();
        }

        newSkill.gameObject.SetActive(true);
    }
    protected void HandleMedicineObject(MedicineObject medicineObject)
    {
        Collider2D medicineCollider = _objMedicine.GetComponent<Collider2D>();

        if (medicineCollider == null)
        {
            Debug.LogWarning("Collider2D không gắn trên _objMedicine.");
        }

        // Tạo một mảng để lưu các collider bị va chạm
        Collider2D[] colliders = new Collider2D[5]; // Có thể thay đổi kích thước tùy theo nhu cầu
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true; // Nếu bạn sử dụng các trigger colliders

        // Kiểm tra va chạm
        int hitCount = Physics2D.OverlapCollider(medicineCollider, contactFilter, colliders);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hitCollider = colliders[i];
            MedicineTarget targetScript = hitCollider.GetComponent<MedicineTarget>();

            if (targetScript != null)
            {
                Debug.Log("Đã va chạm với đối tượng có script TargetVFX.");

                // Lấy vị trí của đối tượng mà _objMedicine va chạm
                Vector3 targetPosition = targetScript.transform.position;

                Transform newSkill = SkillSpawner.Instance.Spawn(SkillSpawner.Instance.GetMedicineType(medicineObject.medicineType), targetPosition, Quaternion.identity);



                if (newSkill.gameObject.GetComponent<ParticleSystem>() is ParticleSystem particleSystem)
                {
                    var emission = particleSystem.emission;
                    emission.rateOverTime = 0;

                    StartCoroutine(DestroyAfterDelay(newSkill, 1f));
                }

                newSkill.gameObject.SetActive(true);

                if (settingsMenu.TryGetComponent(out TimeObject timeSkill))
                {
                    timeSkill._Time = medicineObject.coolDown;
                    timeSkill.ImageRefresh.cooldownDuration = medicineObject.coolDown;
                    timeSkill.ImageRefresh.StartCooldown();
                }

                targetScript.PlayerCtrl.ObjectDamageReceiver.AddPoint(medicineObject.addPoint, medicineObject.medicineType);
            }
        }
    }
    private IEnumerator DestroyAfterDelay(Transform VFX, float delay)
    {
        yield return new WaitForSeconds(delay + 1f); // Thêm 1 giây chờ
        SkillSpawner.Instance.Despawn(VFX);
    }
    private IEnumerator EmitParticlesAndDestroy(ParticleSystem particleSystem, int totalParticles, float duration, Transform VFX)
    {
        float particlesPerSecond = totalParticles / duration;
        float interval = 1f / particlesPerSecond;

        for (int emittedParticles = 0; emittedParticles < totalParticles; emittedParticles++)
        {
            particleSystem.Emit(1);
            Debug.Log("Emitted: " + emittedParticles);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(Mathf.Max(0, duration - (totalParticles * interval)));
        yield return new WaitForSeconds(5f);


        SkillSpawner.Instance.Despawn(VFX);
    }
    public void ActiveSkill()
    {
        MagicRing.SetActive(true);
        SetSelectItem(true);
    }

    public void DeactivateSkill()
    {
        MagicRing.SetActive(false);
        SetSelectItem(false);
    }
    public void ActiveMedicine()
    {
        _objMedicine.SetActive(true);
        SetSelectItem(true);

    }
    public void DeactivateMedicine()
    {
        _objMedicine.SetActive(false);
        SetSelectItem(false);
    }
    private void SetSelectItem(bool value)
    {
        selectItem = value;
    }
}
