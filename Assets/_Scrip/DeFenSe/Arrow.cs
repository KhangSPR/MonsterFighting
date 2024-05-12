using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : SaiMonoBehaviour
{
    Rigidbody2D rb;
    bool hasHit;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        base.Update();
        if(hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
    //Unable Freeze Rotation Z equal true
}
