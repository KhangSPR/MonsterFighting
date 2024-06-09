using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public Sprite Stars;
    public Sprite NoStars ;
    public int starsCount;
    private void Awake()
    {
        var currentMapSO = GameDataManager.Instance.currentMapSO;
        starsCount = (int)currentMapSO.GetStarsCondition(currentMapSO.difficult).CheckThreshold();
        foreach(Transform star in transform)
        {
            star.GetComponent<Image>().sprite = NoStars;
        }
        for(int i = 0; i < starsCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = Stars;
        }
    }
}
