using UnityEngine;
using TMPro;
using System.Collections;

public class TextSizeAnimation : MonoBehaviour
{
    [Header("Wave")]
    public TMP_Text textUI;
    public float startSize;
    public float endSize;
    public float animationDuration;
    public AnimationCurve scaleCurve;

    [Header("Final")]
    public float startSize1;
    public float endSize1;
    public float animationDuration1;
    public AnimationCurve scaleCurve1;

    private float timer = 0f;
    public void PlayWaveAnimation(string Text, bool endWave)
    {
        if (textUI == null)
        {
            Debug.LogError("TextMeshProUGUI chưa được gán!");
            return;
        }
        textUI.text = Text;
        textUI.fontSize = startSize;
        textUI.alpha = 0f;

        if(endWave)
        {
            StartCoroutine(AnimateFinalWaveTextSize());
        }
        else
        {
            StartCoroutine(AnimateWaveTextSize());
        }
    }
    private IEnumerator AnimateWaveTextSize()
    {
        timer = 0f;
        while (timer < animationDuration)
        {
            float progress = timer / animationDuration;
            float scaleValue = Mathf.Lerp(startSize, endSize, scaleCurve.Evaluate(progress));
            textUI.fontSize = scaleValue;
            textUI.alpha = Mathf.Lerp(0f, 1f, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        textUI.fontSize = endSize;
        textUI.alpha = 1f;
    }

    // Coroutine Wave "FINAL WAVE"
    private IEnumerator AnimateFinalWaveTextSize()
    {
        timer = 0f;
        while (timer < animationDuration1)
        {
            float progress = timer / animationDuration1;
            float scaleValue = Mathf.Lerp(startSize1, endSize1, scaleCurve1.Evaluate(progress));
            textUI.fontSize = scaleValue;
            textUI.alpha = Mathf.Lerp(0f, 1f, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        textUI.fontSize = endSize1;
        textUI.alpha = 1f;
    }
}
