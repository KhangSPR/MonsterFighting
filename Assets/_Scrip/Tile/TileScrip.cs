using UnityEngine;

public abstract class TileScrip : SaiMonoBehaviour
{
    [SerializeField]
    private bool isEmpty;
    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
    [SerializeField]
    protected GameObject newObjSet;

    protected override void Start()
    {
        base.Start();
        isEmpty = true;
    }
}
