using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RewardClaimManager : MonoBehaviour
{
    [SerializeField] private GameObject itemObject;
    public GameObject ItemObject => itemObject;

    [SerializeField] private GameObject itemReward;
    public GameObject ItemReward => itemReward;

    private static RewardClaimManager instance; // Instance variable
    public static RewardClaimManager Instance { get => instance; } // Instance getter

    [SerializeField] private Material materialPrefab; // Material prefab (template)
    private Queue<Material> materialPool; // Pool for materials
    [SerializeField] private int initialPoolSize = 10; // Initial size of the pool

    private Dictionary<ItemRarity, Color> rarityColors; // Mapping Rarity to Colors

    private float animationDuration = 1.3f; // Thời gian animation, chỉnh trong Inspector

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only 1 RewardClaimManager Warning");
            return;
        }
        instance = this;

        InitializeMaterialPool();
        InitializeRarityColors();
    }

    // Initialize the pool with a set number of materials
    private void InitializeMaterialPool()
    {
        materialPool = new Queue<Material>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            Material newMaterial = Instantiate(materialPrefab);
            newMaterial.name = $"Material_{i}";
            newMaterial.hideFlags = HideFlags.HideAndDontSave; // Optional: Keep it hidden in the hierarchy
            materialPool.Enqueue(newMaterial);
        }
    }

    // Initialize rarity colors
    private void InitializeRarityColors()
    {
        rarityColors = new Dictionary<ItemRarity, Color>
        {
            { ItemRarity.Common, Color.gray },
            { ItemRarity.Rare, Color.blue },
            { ItemRarity.Epic, new Color(0.5f, 0, 0.5f) }, // Purple
            { ItemRarity.Legendary, Color.yellow }
        };
    }

    // Get a material from the pool with rarity
    public Material GetMaterial(ItemRarity rarity)
    {
        Material material;

        if (materialPool.Count > 0)
        {
            material = materialPool.Dequeue();
        }
        else
        {
            // If pool is empty, create a new material instance
            material = Instantiate(materialPrefab);
            material.name = $"Material_{materialPool.Count + 1}";
        }

        // Apply rarity color
        if (rarityColors.TryGetValue(rarity, out Color color))
        {
            material.color = color;
        }
        if (material.HasProperty("_Stencil"))
        {
            material.SetFloat("_Stencil", 1);
        }
        else
        {
            Debug.LogWarning($"Material {material.name} does not have _Stencil property.");
        }


        return material;
    }

    // Return a material back to the pool
    public void ReturnMaterial(Material material)
    {
        if (material == null) return;

        // Optional: Reset material properties if needed
        material.color = Color.white; // Reset color to default
        materialPool.Enqueue(material);
    }
    public void PlayItemAnimation(Transform itemTransform)
    {
        itemTransform.localScale = Vector3.one * 1.3f; // Đặt scale ban đầu
        itemTransform.DOScale(1, animationDuration)   // Animation scale
            .SetEase(Ease.OutBack);
    }
    public void PlayItemsWithAnimation(List<GameObject> items, float interval)
    {
        var sequence = DOTween.Sequence();

        foreach (var item in items)
        {
            sequence.AppendCallback(() =>
            {
                // Animation xuất hiện
                item.SetActive(true);
                PlayItemAnimation(item.transform);
            });

            sequence.AppendInterval(interval); // Khoảng cách giữa các lần xuất hiện
        }
    }
}
