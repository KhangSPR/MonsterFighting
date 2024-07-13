using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Debuff debuff;
    public Transform target;
    private void Awake()
    {
        debuff.ApplyDebuff(target);
    }
}
