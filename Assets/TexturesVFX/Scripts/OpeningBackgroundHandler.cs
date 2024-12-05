using System.Collections;
using UnityEngine;

[System.Serializable]
class DissolveMaterialBox
{
    public Material material;
    public bool isFadeOut = true;
    public int triggerId;
}

// triggerID: 1 = Title

public class OpeningBackgroundHandler : MonoBehaviour
{   
    [SerializeField] DissolveMaterialBox[] dissolveMaterialBox;
    [SerializeField] float dissolveTime = 1f;

    private void OnEnable()
    {   
        for (int i = 0; i < dissolveMaterialBox.Length; i++) {
            if (dissolveMaterialBox[i].isFadeOut) dissolveMaterialBox[i].material.SetFloat("_Dissolve_Amount", 1);
            else dissolveMaterialBox[i].material.SetFloat("_Dissolve_Amount", 0);
        }
    }

    public void TriggerTitle()
    {
        StartCoroutine(TitleDissolve(1));
    }

    private IEnumerator TitleDissolve(int id)
    {
        float elapsedTime = 0f;
        float currentValue;
        while (elapsedTime <= dissolveTime) {
            for (int i = 0; i < dissolveMaterialBox.Length; i++) {
                if (id != dissolveMaterialBox[i].triggerId) continue;

                if (dissolveMaterialBox[i].isFadeOut) currentValue = Mathf.Lerp(1.0f, 0.0f, elapsedTime / dissolveTime);
                else currentValue = Mathf.Lerp(0f, 1.0f, elapsedTime / dissolveTime);

                dissolveMaterialBox[i].material.SetFloat("_Dissolve_Amount", currentValue);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
