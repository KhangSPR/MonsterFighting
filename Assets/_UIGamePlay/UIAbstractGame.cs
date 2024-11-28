using UnityEngine;
using UnityEngine.UI;


public abstract class UIAbstractGame : SaiMonoBehaviour
{
    [Space]
    [Space]
    [Space]
    [Header("UI Remove - Move")]
    [Header("Button Colors")]
    [SerializeField] protected Color expandedColor;
    [SerializeField] protected Color collapsedColor;
    [SerializeField] protected Sprite _spriteRemove;
    [SerializeField] protected Image _ImgUIRemove;

    [SerializeField]
    protected bool isDragging = false;
    protected const float k_LerpTime = 0.3f;
    protected float colorChangeDuration = 0.5f;
    [SerializeField]
    protected bool isExpanded = false;
    protected override void Update()
    {
        base.Update();

        this.HandleClickRemoveMove();
    }
    protected abstract void HandleClickRemoveMove();
    protected abstract void HandleObjectCollider();
}
