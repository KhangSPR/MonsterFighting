using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashCurve;

    [SerializeField] private List<SpriteRenderer> _spriteRenderers;
    [SerializeField] private Material[] _materials;

    private Coroutine _damageFlashCoroutine;
    public void StopCoroutieSlash()
    {
        // Dừng coroutine khi đối tượng parent được kích hoạt
        if (_damageFlashCoroutine != null)
        {
            Debug.Log("StopCoroutie Slash");
            // Reset the flash amount to 0 when done
            SetFlashAmount(0f);

            StopCoroutine(_damageFlashCoroutine);
            _damageFlashCoroutine = null;
        }
    }

    private void Start()
    {
        //_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        SetSpriteRenderer();

        Init();
    }
    private void SetSpriteRenderer()
    {
        Transform Font = transform.Find("Textures/Font");
        if (Font == null)
        {
            Font = transform.Find("Character/Font");
        }
        //Character -- Co the gan luon khoi can auto
        if (Font != null)
        {
            _spriteRenderers.AddRange(Font.GetComponentsInChildren<SpriteRenderer>());

            Debug.Log("Set Font");

        }
    }
    // Hàm để set material cho nhân vật
    public void SetMaterialDamageFlash()
    {
        // Gán materials từ _materials vào spriteRenderers
        for (int i = 0; i < _spriteRenderers.Count; i++)
        {
            _spriteRenderers[i].material = _materials[i]; // Gán lại material cho từng SpriteRenderer
        }
    }
    private void Init()
    {
        _materials = new Material[_spriteRenderers.Count];

        // Assign sprite renderer materials to _materials
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }

    }


    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        Debug.Log("Dame Flasher Of: " + transform.parent.name);

        //Set the color
        SetFlashColor();

        //Lerp the flash amount
        float curreentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            //iteratee elapsedTime
            elapsedTime += Time.deltaTime;

            //lerp the flash amount
            curreentFlashAmount = Mathf.Lerp(1f, _flashCurve.Evaluate(elapsedTime), (elapsedTime / _flashTime));
            SetFlashAmount(curreentFlashAmount);
            yield return null;
        }

        // Reset the flash amount to 0 when done
        SetFlashAmount(0f);
    }

    private void SetFlashColor()
    {
        //set the color
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        //SSet the flassh amount
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
