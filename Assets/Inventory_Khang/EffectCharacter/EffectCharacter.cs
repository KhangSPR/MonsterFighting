using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCharacter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

    }

    public void SetMaterial(Material material)
    {
        Debug.Log("Set Material");
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].material = material;
        }
    }
}
