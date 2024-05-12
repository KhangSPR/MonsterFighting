using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(BoxCollider2D))]
public class BaseImpact : SaiMonoBehaviour
{

    [Header("Base Impart")]
    [SerializeField] protected Rigidbody2D _rigidbody;
    //[SerializeField] protected BoxCollider2D _collder;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigibody();
        //this.LoadCollider2D();
    }
    protected virtual void LoadRigibody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log(transform.name + ": LoadRigibody", gameObject);
    }
    //protected virtual void LoadCollider2D()
    //{
    //    if (this._collder != null) return;
    //    this._collder = GetComponent<BoxCollider2D>();
    //    this._collder.isTrigger = true;
    //    this._collder.offset = new Vector2(-0.5f, 0.39f);
    //    this._collder.size = new Vector2(4.33f, 2.52f);

    //    Debug.Log(transform.name + ": LoadRigibody", gameObject);
    //}

}
