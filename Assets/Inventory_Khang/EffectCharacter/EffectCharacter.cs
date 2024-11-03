using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EffectCharacter : MonoBehaviour
{
    [Header("Effect Fade")]
    [SerializeField] private List<SpriteRenderer> _spriteRenderers;
    [Header("Effect Dissolve")]
    [SerializeField] private List<SpriteRenderer> _frontSpriteRenderers;
    [SerializeField] private List<SpriteRenderer> _middleSpriteRenderers;
    [SerializeField] private List<SpriteRenderer> _backSpriteRenderers;
    [SerializeField]
    private bool _fadeCharacter = false;
    public bool FadeCharacter => _fadeCharacter;

    [Space(3)]
    [Header("Effect Dead")]
    [SerializeField] GameObject _VFX_Dissolve;
    public GameObject VFX_Dissolve => _VFX_Dissolve;
    [SerializeField] Material[] materials; // Các vật liệu sử dụng để dissolve
    [SerializeField] float dissolveTime;

    [SerializeField]
    private bool isDecreasing = false;
    private float elapsedTime;
    [SerializeField]
    private bool isDissolveComplete = false;
    public bool IsDissolveComplete => isDissolveComplete;


    [Space(3)]
    [Header("Effect Stun")]
    [SerializeField] GameObject vfx_Stun;
    public GameObject Vfx_Stun => vfx_Stun;

    private void Awake()
    {

        _spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());

        //SetSpriteRenderer();
    }

    private Dictionary<SpriteRenderer, int> _initialSortingOrders = new Dictionary<SpriteRenderer, int>();

    public void SetOrderLayerRenderer(int landIndex)
    {
        SetSpriteRenderer();

        string layerName = "Land" + landIndex;

        for (int i = 0; i < _frontSpriteRenderers.Count; i++)
        {
            var renderer = _frontSpriteRenderers[i];

            // Nếu SpriteRenderer không bật, lưu lại sortingOrder ban đầu và thiết lập sorting layer
            if (!renderer.enabled)
            {
                if (!_initialSortingOrders.ContainsKey(renderer))
                {
                    _initialSortingOrders[renderer] = renderer.sortingOrder;
                }
            }

            // Thiết lập sortingLayerName cho renderer
            renderer.sortingLayerName = layerName;
            //Debug.Log($"Set sorting layer {layerName} for {renderer.name}");
        }
    }

    private Transform fontTransfrom;
    public Transform FontTransfrom => fontTransfrom;
    private Transform midleTransfrom;
    public Transform MidleTransfrom => midleTransfrom;
    private Transform backTransfrom;
    public Transform BackTransfrom => backTransfrom;
    public void SetActiveModle(bool active)
    {
        if(fontTransfrom!= null)
        {
            fontTransfrom.gameObject.SetActive(active);
        }
        if (midleTransfrom != null)
        {
            midleTransfrom.gameObject.SetActive(active);
        }
        if(backTransfrom!=null)
        {
            backTransfrom.gameObject.SetActive(active); 
        }

    }
    private void SetSpriteRenderer()
    {
        fontTransfrom = transform.Find("Textures/Font");
        midleTransfrom = transform.Find("Textures/Midle");
        backTransfrom = transform.Find("Textures/Back");

        if(fontTransfrom== null)
        {
            fontTransfrom = transform.Find("Character/Font");
        }
        if (midleTransfrom == null)
        {
            midleTransfrom = transform.Find("Character/Midle");
        }
        if (backTransfrom == null)
        {
            backTransfrom = transform.Find("Character/Back");
        }
        //Character -- Co the gan luon khoi can auto

        if (fontTransfrom != null)
        {
            _frontSpriteRenderers.AddRange(fontTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Font");

        }

        if (midleTransfrom != null)
        {
            //_middleSpriteRenderers = new List<SpriteRenderer>(Middle.GetComponentsInChildren<SpriteRenderer>());
            _middleSpriteRenderers.AddRange(fontTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Midle");

        }

        if (backTransfrom != null)
        {
            //_backSpriteRenderers = new List<SpriteRenderer>(Back.GetComponentsInChildren<SpriteRenderer>());
            _backSpriteRenderers.AddRange(fontTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Back");

        }

    }
    private void Start()
    {
        // Thiết lập giá trị ban đầu cho dissolve
        SetMaterialsToValue(1);
        elapsedTime = dissolveTime;
        isDissolveComplete = false;
    }
    private void Update()
    {
        // Kiểm tra và xử lý quá trình dissolve nếu đang diễn ra
        HandleDissolve();
    }
    // Hàm để set material cho nhân vật
    public void SetMaterial(Material material)
    {
        Debug.Log("Set Material: " + material.name);
        foreach (var spriteRenderer in _frontSpriteRenderers)
        {
            if (spriteRenderer.enabled)
            {
                spriteRenderer.material = material;
            }
        }
    }
    public void SetMaterialDissolv()
    {
        foreach (var spriteRenderer in _frontSpriteRenderers)
        {
            spriteRenderer.material = materials[0];
        }
        foreach (var spriteRenderer in _middleSpriteRenderers)
        {
            spriteRenderer.material = materials[1];
        }
        foreach (var spriteRenderer in _backSpriteRenderers)
        {
            spriteRenderer.material = materials[2];
        }
    }

    // Hàm để bắt đầu quá trình fade nhân vật
    public void StartFadeOut()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f); // Đặt alpha về 0
        }
        _fadeCharacter = true;
    }

    // Hàm để reset alpha (hiển thị lại nhân vật)
    public void ResetAlpha()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1f); // Đặt alpha về 1
        }

        _fadeCharacter = false;
    }

    // Hàm gọi VFX tan biến kẻ thù
    public void CallVFXDeadEnemy()
    {
        if (isDecreasing) return;
        //if (!isDissolveComplete) return;

        SetMaterialDissolv();
        // Bắt đầu quá trình tan biến (dissolve)
        SetMaterialsToValue(1);
        elapsedTime = 0;
        isDecreasing = true;

        SetVFX_Dissolve(true);
    }

    // Handling the dissolution process
    private void HandleDissolve()
    {
        if (isDissolveComplete)
        {
            Debug.Log("Dissolve is already complete, skipping HandleDissolve.");
            return;
        }

        if (isDecreasing)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(1.0f, 0.0f, elapsedTime / dissolveTime);

            SetMaterialsToValue(currentValue);
            //Debug.Log("Dissolving... CurrentValue: " + currentValue);

            if (elapsedTime >= dissolveTime)
            {
                // Quá trình dissolve hoàn tất
                SetMaterialsToValue(0);
                elapsedTime = 0;
                isDecreasing = false;

                isDissolveComplete = true;
                Debug.Log("Dissolve Complete");
            }
        }
    }


    // Hàm để đặt giá trị dissolve vào các material
    private void SetMaterialsToValue(float value)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_Dissolve_Amount", value);
        }
    }
    public void SetDissolveCompleteFalse()
    {
        isDissolveComplete = false;
    }
    public void SetVFX_Dissolve(bool active)
    {
        if (_VFX_Dissolve == null) return;

        _VFX_Dissolve.SetActive(active);

        Debug.Log("Set SetVFX_Dissolve");

    }
}
