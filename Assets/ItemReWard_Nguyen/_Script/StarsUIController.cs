using System;
using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class StarsUIController : MonoBehaviour
{
    [SerializeField] private Sprite StarEasy_Image;
    [SerializeField] private Sprite StarNormal_Image;
    [SerializeField] private Sprite StarHard_Image;
    [SerializeField] private MapSO mapSO;
    private void OnEnable()
    {
        InvokeRepeating(nameof(GetMapSO), 0, 0.1f);
        
        
    }

    private void DrawStars(int totalStars)
    {
        Debug.Log("total stars :" + totalStars);
        for(int i = 1; i<= totalStars; i++)
        {
            // Easy Stars
            if (i == 1) transform.GetChild(0).GetComponent<Image>().sprite = StarEasy_Image;
            if (i == 2) transform.GetChild(1).GetComponent<Image>().sprite = StarEasy_Image;
            if (i == 3) transform.GetChild(2).GetComponent<Image>().sprite = StarEasy_Image;
            // Normal Stars
            if (i == 4) transform.GetChild(0).GetComponent<Image>().sprite = StarNormal_Image;
            if (i == 5) transform.GetChild(1).GetComponent<Image>().sprite = StarNormal_Image;
            if (i == 6) transform.GetChild(2).GetComponent<Image>().sprite = StarNormal_Image;
            // Hard Stars
            if (i == 7) transform.GetChild(0).GetComponent<Image>().sprite = StarHard_Image;
            if (i == 8) transform.GetChild(1).GetComponent<Image>().sprite = StarHard_Image;
            if (i == 9) transform.GetChild(2).GetComponent<Image>().sprite = StarHard_Image;
        }
    }

    public void GetMapSO()
    {
        this.mapSO = transform.parent.parent.GetComponent<LevelButton>().GetMapDataSO();
        if (mapSO != null)
        {
            CancelInvoke(nameof(GetMapSO));
            var easyStars = mapSO.starsEasy;
            var normalStars = mapSO.starsNormal;
            var hardStars = mapSO.starsHard;
            var totalStars = easyStars + normalStars + hardStars;
            DrawStars(totalStars);
        }
    }

}
