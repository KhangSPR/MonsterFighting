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

    private void Awake()
    {
        if (RewardClaimManager.instance != null)
        {
            Debug.LogError("Only 1 RewardClaimManager Warning");
            return;
        }
        RewardClaimManager.instance = this;

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
        else
        {
            Debug.LogWarning($"Rarity {rarity} not found in rarityColors dictionary.");
            material.color = Color.white; // Default color
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
}
