using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Meteorite,
    Ice,
    Lightning,
    Stone,
    Poison

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
    public void Awake()
    {
        type = ItemType.Skill;
    }
}
