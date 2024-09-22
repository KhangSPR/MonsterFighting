using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeed : SaiMonoBehaviour
{
    [SerializeField] GameObject VFX_Speed;
    [SerializeField] Animator VFX_SpeedAnim;
    [SerializeField] Image image;

    private bool isImpleMentSpeeded = false;
    protected override void loadValue()
    {
        base.loadValue();
        this.LoadVFXAndAnimator();
    }
    protected void LoadVFXAndAnimator()
    {
        this.VFX_Speed = transform.GetChild(0).gameObject;
        this.VFX_SpeedAnim = transform.GetChild(1).GetComponent<Animator>();
        this.image = transform.GetChild(1).GetComponent<Image>();
        VFX_SpeedAnim.enabled = false;
        VFX_Speed.SetActive(false);
        Debug.Log("LoadVFXAndAnimator");
    }

    public void ImpleMentSpeed()
    {
        isImpleMentSpeeded = !isImpleMentSpeeded;
        if(isImpleMentSpeeded)
        {
            VFX_SpeedAnim.enabled = isImpleMentSpeeded;
            VFX_Speed.gameObject.SetActive(isImpleMentSpeeded);
        }
        else
        {
            VFX_SpeedAnim.enabled = isImpleMentSpeeded;
            VFX_Speed.gameObject.SetActive(isImpleMentSpeeded);
            image.color = Color.white;

        }
    }
}
