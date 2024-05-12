using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostManager : SaiMonoBehaviour
{
    const float k_LerpTime = 0.6f;

    private static CostManager instance;
    public static CostManager Instance => instance;

    [SerializeField] private TMP_Text currencyTxt;
    [SerializeField] private TMP_Text stoneEnemyCurrencyTxt;
    [SerializeField] private TMP_Text stoneBossCurrencyTxt;

    [SerializeField] private int currency;
    [SerializeField] private int stoneEnemyCurrency;
    [SerializeField] private int stoneBossCurrency;
    public int Currency
    {
        get { return currency; }
        set
        {

            //StartCoroutine(LerpRoutine(currencyTxt, (uint)currency, (uint)value, k_LerpTime));
            currencyTxt.text = value.ToString();
            currency = value;

        }
    }

    public int StoneEnemyCurrency
    {
        get { return stoneEnemyCurrency; }
        set
        {

            //StartCoroutine(LerpRoutine(stoneEnemyCurrencyTxt, (uint)stoneEnemyCurrency, (uint)value, k_LerpTime));
            stoneEnemyCurrencyTxt.text = value.ToString();
            stoneEnemyCurrency = value;

        }
    }

    public int StoneBossCurrency
    {
        get { return stoneBossCurrency; }
        set
        {

            //StartCoroutine(LerpRoutine(stoneBossCurrencyTxt, (uint)stoneBossCurrency, (uint)value, k_LerpTime));
            stoneBossCurrencyTxt.text = value.ToString();
            stoneBossCurrency = value;

        }
    }

    protected override void Awake()
    {
        base.Awake();
        CostManager.instance = this;
    }
    // animated Label counter
    IEnumerator LerpRoutine(TMP_Text label, uint startValue, uint endValue, float duration)
    {
        float lerpValue = (float)startValue;
        float t = 0f;
        label.text = string.Empty;

        while (Mathf.Abs((float)endValue - lerpValue) > 0.05f)
        {
            t += Time.deltaTime / k_LerpTime;

            lerpValue = Mathf.Lerp(startValue, endValue, t);
            label.text = lerpValue.ToString("0");
            yield return null;
        }
        label.text = endValue.ToString();
    }
}
