using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public Image YellowStar;

    private void OnEnable()
    {
        YellowStar = GetComponent<Image>();
        YellowStar.transform.localScale = Vector3.zero;
    }
}
