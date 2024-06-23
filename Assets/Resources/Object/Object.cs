using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "SO/Object")]
public class Object : ScriptableObject
{
    [Header("Object")]
    public string ObjName = "Object";
    public ObjectType objectType;
    public int hpMax;
    public int manaMax;
    public int damage;
    [Header("Card")]
    public float speed = 1.5f;
    public float attackRange;

    [Header("Bulleet")]
    public float speedFly;
    //public float speed = 1;
    //public List<Droprate> drop
    //List;
}
