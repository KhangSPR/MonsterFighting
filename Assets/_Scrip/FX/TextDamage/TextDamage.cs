
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum TextColorType
{
    none , fire , water , ground , axit 
}

public enum TextDamageAnimationType
{
    FadeInBigFirst, FadeOutBigFirst, FadeInSmallFirst, FadeOutSmallFirst
}
public class TextDamage : SaiMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] private float textMoveDistance;
    [SerializeField] private float animationDuration;
    [SerializeField] private TextColorType typeColor;
    [SerializeField] private TextDamageAnimationType typeAnimation;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadText();
    }

    protected virtual void LoadText()
    {
        if (this.text != null) return;
        this.text = GetComponentInChildren<TextMeshProUGUI>();


        Debug.LogWarning(transform.name + ": LoadText", gameObject);
    }

    public virtual void SetDamage(double damage)
    {
        string damageNumber = LargeNumber.ToString(damage);
        this.text.text = damageNumber;

    }

    private Vector3 hitPos;
    float scaleStart = 1;
    float scaleEnd = 2;
    float alphaTextStart = 0;
    float alphaTextEnd = 1;

    public void DoAnimation(Vector3 startPosition)
    {
        this.hitPos = startPosition;

        SetTextAnimationParameter(typeAnimation);
        transform.DOMoveY(transform.position.y + textMoveDistance, animationDuration)
            .OnStart(() =>
        {
            transform.position = startPosition;
            text.color = GetTextColorByType(typeColor);
            Debug.Log("On Start at : "+startPosition);
        })
            .OnComplete(() =>
        {
            Debug.Log("On Complete at:"+ transform.position.y + textMoveDistance);
            transform.position = startPosition;
            gameObject.SetActive(false);
        })
            .SetEase(Ease.Linear);
        transform.DOScale(scaleStart, 0);
        transform.DOScale(scaleEnd, animationDuration);
        transform.GetComponentInChildren<TextMeshProUGUI>()
                .DOFade(alphaTextStart, 0);
        transform.GetComponentInChildren<TextMeshProUGUI>()
            .DOFade(alphaTextEnd, animationDuration);

    }
    void SetTextAnimationParameter(TextDamageAnimationType type)
    {
        switch (type)
        {
            case TextDamageAnimationType.FadeInBigFirst: {
                    scaleStart = 3;
                    scaleEnd = 1;
                    alphaTextStart = 0;
                    alphaTextEnd = 1;
                    break;
                }
            case TextDamageAnimationType.FadeOutBigFirst:
                {
                    scaleStart = 3;
                    scaleEnd = 1;
                    alphaTextStart = 1;
                    alphaTextEnd = 0;
                    break;
                }
            case TextDamageAnimationType.FadeInSmallFirst:
                {
                    scaleStart = 1;
                    scaleEnd = 3;
                    alphaTextStart = 0;
                    alphaTextEnd = 1;
                    break;
                }
            case TextDamageAnimationType.FadeOutSmallFirst:
                {
                    scaleStart = 1;
                    scaleEnd = 3;
                    alphaTextStart = 1;
                    alphaTextEnd = 0;
                    break;
                }
            default:
                break;
        }
    }
    public Color GetTextColorByType(TextColorType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case TextColorType.fire: color = Color.red; break;
            case TextColorType.water: color = Color.cyan; break;
            case TextColorType.ground: color = Color.gray + Color.red + Color.yellow; break;
            case TextColorType.axit: color = Color.green; break;
            default: color = Color.white; break;

        }
        Debug.Log("Type:"+type+",Color:"+color);
        return color;
    }

}
