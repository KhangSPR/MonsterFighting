using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIGameDataManager;

public class CardRefresh : SaiMonoBehaviour
{
    public Image refreshImage;
    public Vector2 height;
    public bool isCoolingDown = false;
    public float cooldownDuration;

    protected override void Start()
    {
        base.Start();
        refreshImage.enabled = false;
    }

    public void StartCooldown()
    {
        if (!isCoolingDown)
        {
            CoroutineManager.Instance.StartManagedCoroutine(CardCooldown(cooldownDuration));

            Debug.Log("StartCooldown");
        }
    }
    public IEnumerator CardCooldown(float cooldownDuration)
    {
        isCoolingDown = true;
        refreshImage.enabled = true;

        for (float i = height.x; i <= height.y; i++)
        {
            //refreshImage.rectTransform.sizeDelta = new Vector2(refreshImage.rectTransform.sizeDelta.x, i);
            refreshImage.rectTransform.anchoredPosition = new Vector3(0, i, 0);
            yield return new WaitForSeconds(cooldownDuration / height.y);
        }

        isCoolingDown = false;
        refreshImage.enabled = false;
    }
}