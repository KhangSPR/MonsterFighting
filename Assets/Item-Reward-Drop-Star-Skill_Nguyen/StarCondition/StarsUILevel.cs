using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class StarsUILevel : SaiMonoBehaviour
{
    [SerializeField] private Sprite[] starImages; // 0: Easy, 1: Normal, 2: Hard
    [SerializeField] private MapSO mapSO;
    [SerializeField] private LevelButton levelButton;

    protected override void Start()
    {
        base.Start();
        GetMapSO();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        levelButton = transform.parent.parent.GetComponent<LevelButton>();
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

    private void GetMapSO()
    {
        this.mapSO = levelButton.MapSO;
        if (mapSO != null)
        {
            int totalStars = mapSO.SumStarsMapDifficult(mapSO.DifficultyMap);

            Debug.Log("SumStar: " + totalStars);

            DrawStars(totalStars);
        }
    }
}
