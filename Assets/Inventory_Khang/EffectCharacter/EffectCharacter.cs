using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCharacter : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _spriteRenderers;

    private bool _fadeCharacter = false;
    public bool FadeCharacter => _fadeCharacter;

    private void Awake()
    {
        // Lấy tất cả SpriteRenderer trong các con của GameObject này
        _spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
    }

    public void SetMaterial(Material material)
    {
        Debug.Log("Set Material");
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.material = material;
        }
    }

    public void StartFadeOut()
    {
        // Đảm bảo tất cả sprite đã fade hoàn toàn
        foreach (var spriteRenderer in _spriteRenderers)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }

        _fadeCharacter = true;
    }
    public void ResetAlpha()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1f); // Đặt alpha về 1
        }

        _fadeCharacter = false;
    }
}
