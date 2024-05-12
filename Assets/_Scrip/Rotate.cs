using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : SaiMonoBehaviour
{
    [SerializeField] protected float speedRotate = 9f;
    protected virtual void FixedUpdate()
    {
        this.RotateObject();
    }
    protected virtual void RotateObject()
    {
        Vector3 eulers = new Vector3(0, 0, 1);
        gameObject.transform.parent.Rotate(eulers * this.speedRotate * Time.fixedDeltaTime);
    }
}
