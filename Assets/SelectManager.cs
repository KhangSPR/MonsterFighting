using System;
using System.Collections;
using UIGameDataMap;
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

    bool Selectitem;


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
        if(Input.GetMouseButtonDown(0))
        {
            if (!Selectitem)
            {
                if (!settingsMenu.IsPointerOverUIElement(settingsMenu.FrameButton))
                {
                    settingsMenu.ToggleOutSide();
                }
            }
            else
            {
                if (!settingsMenu.IsPointerOverUIElement(settingsMenu.FrameButton))
                {
                    if (_magicRing.activeSelf)
                    {
                        TryInstantiateSkill();
                    }
                }
            }
        }   

    }
    private void TryInstantiateSkill()
    {
        if (_item is SkillObject skillObject)
        {
            // Lấy vị trí chuột trong không gian thế giới
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Độ sâu của camera, điều chỉnh theo nhu cầu
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Điều chỉnh vị trí theo trục Y
            float fixedYPosition = skillObject.positionSpawn;


            worldPosition.y = fixedYPosition;

            // Instance gameobjectVFX tại vị trí đã điều chỉnh
            GameObject VFX = Instantiate(skillObject.gameobjectVFX, worldPosition, Quaternion.identity);

            //Set Damage
            ParticleCtrl particleCtrl = VFX.GetComponent<ParticleCtrl>();
            if (particleCtrl != null)
            {
                particleCtrl.particleDamesender.Damage = skillObject.damage;
            }
            // Access the ParticleSystem component
            ParticleSystem particleSystem = VFX.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                // Disable continuous emission
                var emission = particleSystem.emission;
                emission.rateOverTime = 0;

                // Start a coroutine to emit particles over time and destroy the object after
                StartCoroutine(EmitParticlesAndDestroy(particleSystem, skillObject.particleCount, skillObject.timeSpawn, VFX));
            }

            VFX.SetActive(true);
            Debug.Log("Spawn VFX Skill at Y = " + fixedYPosition);

            settingsMenu._Time = skillObject.coolDown;
            settingsMenu.ImageRefresh.cooldownDuration = skillObject.coolDown;
            settingsMenu?.ToggleSelect();
        }
    }

    private IEnumerator EmitParticlesAndDestroy(ParticleSystem particleSystem, int totalParticles, float duration, GameObject VFX)
    {
        float particlesPerSecond = totalParticles / duration;
        float interval = 1f / particlesPerSecond;

        int emittedParticles = 0;

        while (emittedParticles < totalParticles)
        {
            particleSystem.Emit(1);
            emittedParticles++;
            Debug.Log("Emitted: " + emittedParticles);
            yield return new WaitForSeconds(interval);
        }

        // Wait for the remaining time if any particles were emitted faster than expected
        float remainingTime = duration - (emittedParticles * interval);
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // Wait an additional 5 seconds before destroying the VFX object
        yield return new WaitForSeconds(5f);
        // Now it's safe to destroy the VFX object
        Destroy(VFX);
    }


    public void ActiveSkill()
    {
        _MagicRing.SetActive(true);
        Selectitem = true;
    }

    public void DeactiveSkill()
    {
        _MagicRing.SetActive(false);

        Selectitem = false;
    }

}
