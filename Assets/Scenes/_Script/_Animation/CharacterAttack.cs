using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : SaiMonoBehaviour
{
    private Animator animator;
    protected override void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    public void StartAttacking()
    {
        animator.Play("Attack");
    }
}
