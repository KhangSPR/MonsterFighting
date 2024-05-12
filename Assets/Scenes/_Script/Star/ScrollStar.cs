using System.Collections;
using UnityEngine;
using UIGameDataManager;

public class ScrollStar : MonoBehaviour
{


    [SerializeField] Star[] Stars;

    [SerializeField] float EnlargeScale = 1.5f;
    [SerializeField] float ShrinkScale = 1f;
    [SerializeField] float EnlargeDuration = 0.35f;
    [SerializeField] float ShrinkDuration = 0.35f;


    //public int numBerStar;
    [SerializeField] int numberStar;
    public int NumberStar { get { return numberStar; } set { numberStar = value; } }

    private void OnEnable()
    {

        Debug.Log("OnEnable :" + NumberStar);
        OnShowStars(NumberStar);

        CharacterData.numBerStar += OnShowStars;
    }
    private void OnDisable()
    {
        CharacterData.numBerStar -= OnShowStars;
    }
    void OnShowStars(int numberOfStars)
    {
        StartCoroutine(ShowStarsRoutine(numberOfStars));
    }

    private IEnumerator ShowStarsRoutine(int numberOfStars)
    {
        foreach (Star star in Stars)
        {
            star.YellowStar.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < numberOfStars; i++)
        {
            yield return StartCoroutine(EnlargeAndShrinkStar(Stars[i]));
        }
    }

    private IEnumerator EnlargeAndShrinkStar(Star star)
    {
        yield return StartCoroutine(ChangeStarScale(star, EnlargeScale, EnlargeDuration));
        yield return StartCoroutine(ChangeStarScale(star, ShrinkScale, ShrinkDuration));
    }

    private IEnumerator ChangeStarScale(Star star, float targetScale, float duration)
    {
        Vector3 initialScale = star.YellowStar.transform.localScale;
        Vector3 finalScale = new Vector3(targetScale, targetScale, targetScale);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            star.YellowStar.transform.localScale = Vector3.Lerp(initialScale, finalScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        star.YellowStar.transform.localScale = finalScale;
    }
}
