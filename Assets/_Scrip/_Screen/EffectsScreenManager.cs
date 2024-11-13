using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsScreenManager : MonoBehaviour
{
    public Material screenDamageMat;
    private Coroutine screenDamageTask;
    public float speedMultiplier = 2f;

    [Space]
    [Space]
    public Object_ShakeTransfrom object_ShakeTransfrom;

    // Instance Manager
    private static EffectsScreenManager instance;
    public static EffectsScreenManager Instance => instance;

    private void Awake()
    {
        //if (instance != null)
        //    Debug.LogError("Only one EffectsScreenManager instance allowed!");

        instance = this;
    }

    private void Start()
    {
        ResetEffectScreenDefaultDMG();
    }

    public void ScreenDamageEffect(float thresholdIntensity)
    {
        float intensity = Random.Range(0.1f, 1);

        if (screenDamageTask != null)
            StopCoroutine(screenDamageTask);

        screenDamageTask = StartCoroutine(screenDamage(intensity, thresholdIntensity));
    }

    // Effect Screen Reset Default
    private void ResetEffectScreenDefaultDMG()
    {
        screenDamageMat.SetFloat("_FullScrennIntensity", 0);
    }

    private IEnumerator screenDamage(float intensity, float thresholdIntensity)
    {
        float targetRadius;
        float curRadius;

        if (0.3 < thresholdIntensity)
        {
            // Nếu intensity lớn hơn threshold, set độ hư hại từ 0.05 -> 0
            targetRadius = Remap(intensity, 0.1f, 1f, 0.05f, 0f);
            curRadius = 0.05f;
        }
        else
        {
            // Nếu intensity nhỏ hơn threshold, set độ hư hại từ 0.25 -> 0
            targetRadius = Remap(intensity, 0.1f, 1f, 0.25f, 0f);
            curRadius = 0.25f;

        }
        // Giảm dần mức độ hư hại từ curRadius về targetRadius
        for (float t = 0; curRadius > targetRadius; t += Time.deltaTime * speedMultiplier)
        {
            curRadius = Mathf.Lerp(curRadius, targetRadius, t);
            screenDamageMat.SetFloat("_FullScrennIntensity", curRadius);
            yield return null;
        }

        // Giảm dần từ targetRadius về 0
        for (float t = 0; curRadius > 0f; t += Time.deltaTime * speedMultiplier)
        {
            curRadius = Mathf.Lerp(targetRadius, 0f, t);
            screenDamageMat.SetFloat("_FullScrennIntensity", curRadius);
            yield return null;
        }
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }
}
