using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeUI : MonoBehaviour
{
    public Vector3 size_default = Vector3.one;
    public Vector3 size_new;
    public void ReSize()
    {
        Debug.Log("ReSize");
        // phóng to button hiện tại
        
        foreach (Transform t in transform.parent)
        {
            t.localScale = size_default;
        }
        transform.localScale = size_new;
    }
}
