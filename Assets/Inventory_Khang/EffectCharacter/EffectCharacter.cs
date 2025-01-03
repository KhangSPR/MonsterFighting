using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]
    private Transform fontTransfrom;
    public Transform FontTransfrom => fontTransfrom;
    [SerializeField]
    private Transform midleTransfrom;
    public Transform MidleTransfrom => midleTransfrom;
    [SerializeField]
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
        _frontSpriteRenderers.Clear();
        _middleSpriteRenderers.Clear();
        _backSpriteRenderers.Clear();
        if (fontTransfrom != null)
        {
            _frontSpriteRenderers.AddRange(fontTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Font");

        }

        if (midleTransfrom != null)
        {
            //_middleSpriteRenderers = new List<SpriteRenderer>(Middle.GetComponentsInChildren<SpriteRenderer>());
            _middleSpriteRenderers.AddRange(midleTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Midle");

        }

        if (backTransfrom != null)
        {
            //_backSpriteRenderers = new List<SpriteRenderer>(Back.GetComponentsInChildren<SpriteRenderer>());
            _backSpriteRenderers.AddRange(backTransfrom.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Back");

        }

    }
    private void Start()
    {
        // Thiết lập giá trị ban đầu cho dissolve
        SetMaterialsToValue(1);
        elapsedTime = dissolveTime;
        isDissolveComplete = false;
        if(transform.parent.CompareTag("Enemy"))
        {
            Init();
        }    
    }
    private void Update()
    {
        // Kiểm tra và xử lý quá trình dissolve nếu đang diễn ra
        HandleDissolve();

    }
    [SerializeField] private Material[] _materialsFont;

    [SerializeField] private Material[] _materialsMidle;

    [SerializeField] private Material[] _materialsBack;

    private void Init()
    {
        if (materials.Length <= 0) return;

        _materialsFont = new Material[_frontSpriteRenderers.Count];
        _materialsMidle = new Material[_middleSpriteRenderers.Count];
        _materialsBack = new Material[_backSpriteRenderers.Count];

        // Tạo material riêng biệt cho mỗi SpriteRenderer
        for (int i = 0; i < _materialsFont.Length; i++)
        {
            _materialsFont[i] = materials[0];
        }
        for (int i = 0; i < _materialsMidle.Length; i++)
        {
            _materialsMidle[i] = _middleSpriteRenderers[i].material;
        }
        for (int i = 0; i < _materialsBack.Length; i++)
        {
            _materialsBack[i] = _backSpriteRenderers[i].material;
        }

        Debug.Log("Initialized materials for dissolve effect");
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
        // Set material dissolve cho lớp Font
        for (int i = 0; i < _frontSpriteRenderers.Count; i++)
        {
            _frontSpriteRenderers[i].material = _materialsFont[i];
        }

        // Set material dissolve cho lớp Midle
        for (int i = 0; i < _middleSpriteRenderers.Count; i++)
        {
            _middleSpriteRenderers[i].material = _materialsMidle[i];
        }

        // Set material dissolve cho lớp Back
        for (int i = 0; i < _backSpriteRenderers.Count; i++)
        {
            _backSpriteRenderers[i].material = _materialsBack[i];
        }

        Debug.Log("Materials set for dissolve effect");

        Debug.Log($"_frontSpriteRenderers.Count: {_frontSpriteRenderers.Count}, _materialsFont.Count: {_materialsFont.Length}");
        Debug.Log($"_middleSpriteRenderers.Count: {_middleSpriteRenderers.Count}, _materialsMidle.Count: {_materialsMidle.Length}");
        Debug.Log($"_backSpriteRenderers.Count: {_backSpriteRenderers.Count}, _materialsBack.Count: {_materialsBack.Length}");

    }


    // Hàm để bắt đầu quá trình fade nhân vật
    public void StartFadeOut()
    {
        foreach (var spriteRenderer in _frontSpriteRenderers)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f); // Đặt alpha về 0
        }
        _fadeCharacter = true;
    }

    // Hàm để reset alpha (hiển thị lại nhân vật)
    public void ResetAlpha()
    {
        foreach (var spriteRenderer in _frontSpriteRenderers)
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

        // Gọi hàm để set đúng material dissolve cho các lớp font, middle, back
        SetMaterialDissolv();

        // Đặt material ban đầu cho hiệu ứng dissolve
        SetMaterialsToValue(1.0f);
        elapsedTime = 0;
        isDecreasing = true;

        SetVFX_Dissolve(true);
        Debug.Log("Started Dissolve Effect");
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

            if (elapsedTime >= dissolveTime)
            {
                SetMaterialsToValue(0.0f);
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
        foreach (var material in _materialsFont)
        {
            material.SetFloat("_Dissolve_Amount", value);
        }
        foreach (var material in _materialsMidle)
        {
            material.SetFloat("_Dissolve_Amount", value);
        }
        foreach (var material in _materialsBack)
        {
            material.SetFloat("_Dissolve_Amount", value);
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
