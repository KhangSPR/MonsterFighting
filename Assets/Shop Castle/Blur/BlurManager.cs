using UnityEngine;
using UnityEngine.UI;

public class BlurManager : MonoBehaviour
{
    public Material blurMaterial;
    public Image imageTexure;
    public void GetblurMaterial()
    {
        blurMaterial.mainTexture = imageTexure.sprite.texture;

        Debug.Log("Blur");
    }
    private void OnApplicationQuit()
    {
        blurMaterial.mainTexture=null;
    }
}
