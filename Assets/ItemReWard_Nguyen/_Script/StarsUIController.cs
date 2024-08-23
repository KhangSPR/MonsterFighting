using System;
using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class StarsUIController : MonoBehaviour
{
    [SerializeField] private Sprite[] starImages; // 0: Easy, 1: Normal, 2: Hard
    [SerializeField] private MapSO mapSO;
    [SerializeField] private LevelButton levelButton;

    private void Start()
    {
        GetMapSO();
    }

    private void DrawStars(int totalStars)
    {
        Debug.Log("Total stars: " + totalStars);
        for (int i = 0; i < totalStars; i++)
        {
            Image starImage = transform.GetChild(i % 3).GetComponent<Image>();
            int difficultyIndex = i / 3;
            if (difficultyIndex < starImages.Length)
            {
                starImage.sprite = starImages[difficultyIndex];
            }
            
        }
    }

    public void GetMapSO()
    {
        this.mapSO = levelButton.GetMapDataSO();
        if (mapSO != null)
        {
            int totalStars = mapSO.SumStarsMapDifficult(mapSO.DifficultyMap);

            Debug.Log("SumStar: " + totalStars);

            DrawStars(totalStars);
        }
    }
}
