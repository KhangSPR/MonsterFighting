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
    [SerializeField] protected Sprite _spriteUI;
    [SerializeField] protected Image _ImgUIRemove;
    [SerializeField] protected Image _IconUI;

    [SerializeField]
    protected bool isDragging = false;
    protected const float k_LerpTime = 0.3f;
    protected float colorChangeDuration = 0.5f;
    [SerializeField]
    protected bool isExpanded = false;
    protected override void Update()
    {
        base.Update();

        this.HandleClickUI();
    }
    protected abstract void HandleClickUI();
    protected abstract void HandleObjectCollider();
    public abstract void ToggleExitUI();

}
