using TMPro;
using UnityEngine;
using DG.Tweening;

public enum TextDamageAnimationType
{
    FadeInBigFirst,
    FadeOutBigFirst,
    FadeInSmallFirst,
    FadeOutSmallFirst
}

public class TextDamage : SaiMonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] private float textMoveDistance;
    [SerializeField] private float animationDuration;

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

    private void SetDamage(double damage)
    {
        string damageNumber = LargeNumber.ToString(damage);
        this.text.text = damageNumber;
    }

    private Vector3 hitPos;
    private float scaleStart = 1;
    private float scaleEnd = 2;
    private float alphaTextStart = 0;
    private float alphaTextEnd = 1;

    public void DoAnimation(double damage, SkillType typeColor)
    {
        this.hitPos = transform.position;

        // Gán damage và thông số hoạt hình trước khi bắt đầu hoạt hình
        SetDamage(damage);
        SetTextAnimationParameter(typeAnimation);

        transform.DOMoveY(transform.position.y + textMoveDistance, animationDuration)
            .OnStart(() =>
            {
                transform.position = transform.position;
                text.color = GetTextColorByType(typeColor);
                Debug.Log("On Start at : " + transform.position);
            })
            .OnComplete(() =>
            {
                Debug.Log("On Complete at:" + (transform.position.y + textMoveDistance));
                transform.position = transform.position;
                gameObject.SetActive(false);
            })
            .SetEase(Ease.Linear);

        transform.DOScale(scaleStart, 0);
        transform.DOScale(scaleEnd, animationDuration);

        text.DOFade(alphaTextStart, 0);
        text.DOFade(alphaTextEnd, animationDuration);
    }

    void SetTextAnimationParameter(TextDamageAnimationType type)
    {
        switch (type)
        {
            case TextDamageAnimationType.FadeInBigFirst:
                scaleStart = 1.5f;
                scaleEnd = 1;
                alphaTextStart = 0;
                alphaTextEnd = 1;
                break;

            case TextDamageAnimationType.FadeOutBigFirst:
                scaleStart = 1.5f;
                scaleEnd = 1;
                alphaTextStart = 1;
                alphaTextEnd = 0;
                break;

            case TextDamageAnimationType.FadeInSmallFirst:
                scaleStart = 1;
                scaleEnd = 1.5f;
                alphaTextStart = 0;
                alphaTextEnd = 1;
                break;

            case TextDamageAnimationType.FadeOutSmallFirst:
                scaleStart = 1;
                scaleEnd = 1.5f;
                alphaTextStart = 1;
                alphaTextEnd = 0;
                break;
        }
    }

    public Color GetTextColorByType(SkillType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case SkillType.Fire: color = Color.red; break;
            case SkillType.Glace: color = Color.cyan; break;
            case SkillType.Stone: color = Color.gray + Color.red + Color.yellow; break;
            case SkillType.Poison: color = Color.green; break;
            case SkillType.Electric: color = Color.yellow; break;
            case SkillType.Default: color = Color.white; break;
            default: color = Color.white; break;
        }
        Debug.Log("Type:" + type + ",Color:" + color);
        return color;
    }
}