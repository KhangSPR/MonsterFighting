using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    void Start()
    {
        StartJumping();
    }

    public void StartJumping()
    {
        transform.LeanMoveLocalY(50f, 0.5f).setEaseLinear().setLoopPingPong();
    }
}
