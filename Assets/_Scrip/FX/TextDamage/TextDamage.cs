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

    private Vector3 hitPos;
    private float scaleStart, scaleEnd, alphaTextStart, alphaTextEnd;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.text = GetComponentInChildren<TextMeshProUGUI>() ?? this.text;
        if (this.text == null) Debug.LogWarning(transform.name + ": LoadText", gameObject);
    }

    public void DoAnimation(double value, object type)
    {
        this.hitPos = transform.position;
        SetPont(value, type);
        SetTextAnimationParameter(typeAnimation);

        Color color = type is SkillType skillType ? GetTextColorByType(skillType) : GetTextColorByType2((Medicine)type);
        AnimateText(color);
    }

    private void SetPont(double damage, object type)
    {
        string damageNumber = LargeNumber.ToString(damage);;

        if (type is SkillType skillType)
        {
            this.text.text = damageNumber;
        }
        if (type is Medicine medicineType)
        {
            this.text.text = "+" + damageNumber.ToString();
        }
    }

    private void SetTextAnimationParameter(TextDamageAnimationType type)
    {
        switch (type)
        {
            case TextDamageAnimationType.FadeInBigFirst:
                scaleStart = 1.5f; scaleEnd = 1;
                alphaTextStart = 0; alphaTextEnd = 1;
                break;
            case TextDamageAnimationType.FadeOutBigFirst:
                scaleStart = 1.5f; scaleEnd = 1;
                alphaTextStart = 1; alphaTextEnd = 0;
                break;
            case TextDamageAnimationType.FadeInSmallFirst:
                scaleStart = 1; scaleEnd = 1.5f;
                alphaTextStart = 0; alphaTextEnd = 1;
                break;
            case TextDamageAnimationType.FadeOutSmallFirst:
                scaleStart = 1; scaleEnd = 1.5f;
                alphaTextStart = 1; alphaTextEnd = 0;
                break;
        }
    }

    private void AnimateText(Color color)
    {
        transform.DOMoveY(transform.position.y + textMoveDistance, animationDuration)
            .OnStart(() =>
            {
                transform.position = hitPos;
                text.color = color;
                Debug.Log("On Start at : " + hitPos);
            })
            .OnComplete(() =>
            {
                Debug.Log("On Complete at:" + (transform.position.y + textMoveDistance));
                transform.position = hitPos;
                gameObject.SetActive(false);
            })
            .SetEase(Ease.Linear);

        transform.DOScale(scaleStart, 0);
        transform.DOScale(scaleEnd, animationDuration);

        text.DOFade(alphaTextStart, 0);
        text.DOFade(alphaTextEnd, animationDuration);
    }

    public Color GetTextColorByType(SkillType type)
    {
        return type switch
        {
            SkillType.Fire => Color.red,
            SkillType.Glace => Color.cyan,
            SkillType.Stone => Color.gray + Color.red + Color.yellow,
            SkillType.Poison => Color.green,
            SkillType.Electric => Color.yellow,
            _ => Color.white,
        };
    }

    public Color GetTextColorByType2(Medicine type)
    {
        return type switch
        {
            Medicine.Heal => Color.green,
            Medicine.Power => new Color(1f, 0.65f, 0f), // #FFA500
            _ => Color.white,
        };
    }
}
