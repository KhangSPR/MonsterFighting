using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPortals : PortalSpawnManagerAbstract
{
    [Header("Slider")]
    [SerializeField] Slider progress; // Thanh trượt

    [Header("Enemy Info")]
    [SerializeField] int enemyMax;  // Tổng số lượng kẻ thù
    private int enemySpawned = 0;   // Số lượng kẻ thù đã được sinh ra

    private MapSO mapSOProgress;

    [Header("Animation & UI")]
    public ObjFlagAnimation ObjFlagPrefab;
    public float[] displayPercentage;
    public List<ObjFlagAnimation> animationList = new List<ObjFlagAnimation>();
    public RectTransform targetUI;

    protected override void Start()
    {
        base.Start();
        InitializeProgress();
    }

    private void InitializeProgress()
    {
        if (portalSpawnManagerCtrl.MapSO == null) return;

        mapSOProgress = portalSpawnManagerCtrl.MapSO;
        enemyMax = mapSOProgress.SumEnemyAll(portalSpawnManagerCtrl.Difficult);
        Debug.Log("EnemyMax = " + enemyMax);

        progress.maxValue = enemyMax;
        progress.value = 0;

        CalculateUIPercentage();
        InitializeUIPercentageMarkers();
    }

    public void OnEnemySpawned()
    {
        enemySpawned++;
        Debug.Log($"EnemySpawned: {enemySpawned}/{enemyMax}");
        UpdateProgress();
        UpdateAnimationFlag();
    }

    private void UpdateAnimationFlag()
    {
        float currentPercentage = (float)enemySpawned / enemyMax;

        for (int i = 0; i < displayPercentage.Length; i++)
        {
            if (currentPercentage >= displayPercentage[i])
            {
                animationList[i].RunAnimationFlag();
            }
        }
    }

    private void CalculateUIPercentage()
    {
        displayPercentage = new float[portalSpawnManagerCtrl.Wave.Length];

        for (int i = 0; i < portalSpawnManagerCtrl.Wave.Length; i++)
        {
            if (i > 0)
            {
                displayPercentage[i] = displayPercentage[i - 1] + (float)portalSpawnManagerCtrl.Wave[i].SumEnemyWave(portalSpawnManagerCtrl.Wave[i]) / enemyMax;
            }
            else
            {
                displayPercentage[i] = (float)portalSpawnManagerCtrl.Wave[i].SumEnemyWave(portalSpawnManagerCtrl.Wave[i]) / enemyMax;
            }

            Debug.Log($"Percentage[{i}]: {displayPercentage[i]}");
        }
    }


    private void InitializeUIPercentageMarkers()
    {
        foreach (float percentage in displayPercentage)
        {
            Vector3 newPos = CalculateUIPosition(percentage);
            CreateFlagAnimationAtPosition(newPos);
        }

        targetUI.gameObject.SetActive(true);
    }

    private Vector3 CalculateUIPosition(float percentage)
    {
        float minX = -333 / 2f;
        float maxX = 333 / 2f;
        float positionXTarget = Mathf.Lerp(maxX, minX, percentage);

        return new Vector3(positionXTarget, targetUI.localPosition.y, targetUI.localPosition.z);
    }

    private void CreateFlagAnimationAtPosition(Vector3 position)
    {
        GameObject newObj = Instantiate(ObjFlagPrefab.gameObject, targetUI.position, Quaternion.identity, targetUI);
        newObj.transform.localPosition = position;
        animationList.Add(newObj.GetComponent<ObjFlagAnimation>());
    }

    // Cập nhật thanh trượt
    private void UpdateProgress()
    {
        StartCoroutine(UpdateProgressCoroutine(progress.value, enemySpawned));
    }

    private IEnumerator UpdateProgressCoroutine(float startValue, int targetEnemySpawned)
    {
        float elapsedTime = 0f;
        float duration = 1f;
        float targetValue = targetEnemySpawned;

        while (elapsedTime < duration)
        {
            float lerpedValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            progress.value = lerpedValue;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        progress.value = targetValue; // Đảm bảo giá trị cuối cùng
    }
}
