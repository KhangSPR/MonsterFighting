using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageRefresh : SaiMonoBehaviour
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
            CoroutineManager.Instance.StartManagedCoroutine(Cooldown(cooldownDuration));

            Debug.Log("StartCooldown");
        }
    }

    public IEnumerator Cooldown(float cooldownDuration)
    {
        isCoolingDown = true;
        refreshImage.enabled = true;

        float elapsedTime = 0f;

        for (float i = height.x; i <= height.y; i++)
        {
            refreshImage.rectTransform.anchoredPosition = new Vector3(0, i, 0);
            yield return new WaitForSeconds(cooldownDuration / height.y);

            elapsedTime += cooldownDuration / height.y;
        }

        isCoolingDown = false;
        refreshImage.enabled = false;

    }
}
