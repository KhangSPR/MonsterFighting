using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager : SaiMonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance => _instance;

    [SerializeField] protected Material[] effectCharacter;
    public Material[] EffectCharacter => effectCharacter;
    [SerializeField] private Material _materialDefault;
    public Material MaterialDefault => _materialDefault;
    protected override void Awake()
    {
        base.Awake();
        _instance = this;
    }
    public Material GetMaterialByName(string materialName)
    {
        switch (materialName.ToLower())
        {
            case "glace":
                return effectCharacter[0];

            case "poison":
                return effectCharacter[1];

            case "fire":
                return effectCharacter[2];

            case "electric":
                return effectCharacter[3];

            default:
                Debug.LogWarning($"Material with name '{materialName}' not found. Returning default material.");
                return MaterialDefault; // Trả về material mặc định nếu không tìm thấy tên
        }
    }
}
