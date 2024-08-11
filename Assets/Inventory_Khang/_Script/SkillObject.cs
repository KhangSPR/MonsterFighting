using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Meteorite,
    Ice,
    Electricity,
    Stone,
    Poison

}
[CreateAssetMenu(fileName = "New skills Object", menuName = "Inventory System/Items/Skills")]

public class SkillObject : ItemObject
{
    public float timeSpawn;
    public int damage;
    public int count;
    public int coolDown;
    public Vector2 activityAction;
    public SkillType skillType;
    public GameObject gameobjectVFX;
    public void Awake()
    {
        type = ItemType.Skill;
    }
}
