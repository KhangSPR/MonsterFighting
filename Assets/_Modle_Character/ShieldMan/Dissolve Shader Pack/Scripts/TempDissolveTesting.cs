using UnityEngine;

public class TempDissolveTesting : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] float dissolveTime = 5f;
    [SerializeField] bool isLoop;

    private bool isDecreasing = true;
    private float elapsedTime;

    private void Start()
    {
        for (int i = 0; i < materials.Length; i++) {
            materials[i].SetFloat("_Dissolve_Amount", 1);
        }

        elapsedTime = dissolveTime;
    }

    private void Update()
    {   
        if (isDecreasing) {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(1.0f, 0.0f, elapsedTime / dissolveTime);

            for (int i = 0; i < materials.Length; i++) {
                materials[i].SetFloat("_Dissolve_Amount", currentValue);
            }

            if (elapsedTime >= dissolveTime) {
                for (int i = 0; i < materials.Length; i++) {
                    materials[i].SetFloat("_Dissolve_Amount", 0);
                }
                elapsedTime = 0;
                isDecreasing = false;
            }
        }

        else if (!isDecreasing && isLoop) {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(0f, 1.0f, elapsedTime / dissolveTime);

            for (int i = 0; i < materials.Length; i++) {
                materials[i].SetFloat("_Dissolve_Amount", currentValue);
            }

            if (elapsedTime >= dissolveTime) {
                for (int i = 0; i < materials.Length; i++) {
                    materials[i].SetFloat("_Dissolve_Amount", 1);
                }
                elapsedTime = 0;
                isDecreasing = true;
            }
        }
    }
}
