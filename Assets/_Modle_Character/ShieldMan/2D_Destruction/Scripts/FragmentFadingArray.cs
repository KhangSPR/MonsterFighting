using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class FragmentFadingArray : MonoBehaviour
{   
    [SerializeField] SpriteRenderer frontShieldSprite;
    [SerializeField] Material frontShieldMaterial;

    [SerializeField] SpriteRenderer backShieldSprite;
    [SerializeField] Material backShieldMaterial;

    [SerializeField] Transform[] effectObject;

    [SerializeField] float dissolveShieldTime = .8f;
    [SerializeField] float fragmentMinTime = .5f;
    [SerializeField] float fragmentMaxTime = .7f;

    private Explodable explodable;
    private Vector3[] initialEffectObjectScale;
    
    private float startFrontValue;
    private float startBackValue;

    private void OnEnable()
    {
        explodable = GetComponent<Explodable>();

        initialEffectObjectScale = new Vector3[effectObject.Length];
        for (int i = 0; i < effectObject.Length; i++)
            initialEffectObjectScale[i] = effectObject[i].localScale;
    }

    public void TriggerFading()
    {   
        float[] fragmentFadeTimeArray = new float[explodable.fragments.Count];
        for (int i = 0; i < explodable.fragments.Count; i++) 
            fragmentFadeTimeArray[i] = Random.Range(fragmentMinTime, fragmentMaxTime);

        Material[] fragmentMaterialArray = new Material[explodable.fragments.Count];
        for (int i = 0; i < explodable.fragments.Count; i++) 
            fragmentMaterialArray[i] = explodable.fragments[i].GetComponent<MeshRenderer>().material;

        frontShieldSprite.material = Instantiate(frontShieldMaterial);
        backShieldSprite.material = Instantiate(backShieldMaterial);

        StartCoroutine(
            TriggerDissolve(
                frontShield: frontShieldSprite.material, 
                backShield: backShieldSprite.material, 
                fragmentFadeTimeArray: fragmentFadeTimeArray,
                fragmentMaterialArray: fragmentMaterialArray
            )
        );
    }

    private IEnumerator TriggerDissolve(Material frontShield, Material backShield, float[] fragmentFadeTimeArray, Material[] fragmentMaterialArray)
    {   
        explodable.explode();
		ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		ef.doExplosion(transform.position);

        for (int i = 0; i < fragmentMaterialArray.Length; i++) {
            fragmentMaterialArray[i].SetFloat("_Dissolve_Amount", 1);
        }

        float elapsedTime = 0;
        startFrontValue = frontShield.GetFloat("_Dissolve_Amount");
        startBackValue = backShield.GetFloat("_Dissolve_Amount");

        bool stopFlag = false;
        while (!stopFlag) {
            stopFlag = true;

            float currentValue;

            if (elapsedTime < dissolveShieldTime) {
                currentValue = Mathf.Lerp(startFrontValue, 0.0f, elapsedTime / dissolveShieldTime);
                frontShield.SetFloat("_Dissolve_Amount", currentValue);

                currentValue = Mathf.Lerp(startBackValue, 0.0f, elapsedTime / dissolveShieldTime);
                backShield.SetFloat("_Dissolve_Amount", currentValue);

                currentValue = elapsedTime / dissolveShieldTime;
                for (int i = 0; i < effectObject.Length; i++)
                    effectObject[i].localScale = Vector3.Lerp(initialEffectObjectScale[i], Vector3.zero, currentValue);
                
                stopFlag = false;
            }

            for (int i = 0; i < fragmentFadeTimeArray.Length; i++) {
                if (fragmentMaterialArray[i].GetFloat("_Dissolve_Amount") <= 0) continue;

                if (elapsedTime >= fragmentFadeTimeArray[i]) fragmentMaterialArray[i].SetFloat("_Dissolve_Amount", 0);

                currentValue = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fragmentFadeTimeArray[i]);
                fragmentMaterialArray[i].SetFloat("_Dissolve_Amount", currentValue);

                stopFlag = false;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private void OnDisable()
    {
        explodable.deleteFragments();
        frontShieldSprite.material.SetFloat("_Dissolve_Amount", startFrontValue);
        frontShieldSprite.material.SetFloat("_Dissolve_Amount", startBackValue);

        for (int i = 0; i < effectObject.Length; i++) effectObject[i].localScale = initialEffectObjectScale[i];
    }
}
