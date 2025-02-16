using UnityEngine;

public enum SkillType
{
    Default,
    Fire,
    Glace,
    Electric,
    Stone,
    Poison,
    Dark,
    Miss
}
[CreateAssetMenu(fileName = "New skills Object", menuName = "Inventory System/Items/Skills")]

public class SkillObject : ItemObject
{
    public float timeSpawn;
    public int damage;
    public int particleCount;
    public int coolDown;
    public SkillType skillType;
    public GameObject gameobjectVFX;
    public float positionSpawn;
    public void Awake()
    {
        type = InventoryType.Skill;
    }
}
