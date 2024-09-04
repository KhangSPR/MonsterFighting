using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPortals : PortalSpawnManagerAbstract
{
    [Header("Slider")]
    [SerializeField] Slider progress;
    [SerializeField] int enemyNax;
    [SerializeField] int enemySpawn;

    MapSO mapSOProgress;


    public int EnemySpawn
    {
        get { return enemySpawn; }
        set
        {
            enemySpawn = value;
            UpdateProgress();
        }
    }
    protected override void Start()
    {
        base.Start();
        if (portalSpawnManagerCtrl.MapSO == null) return;

        //Set MapSO
        mapSOProgress = portalSpawnManagerCtrl.MapSO;
        //Enemy Max
        enemyNax = mapSOProgress.SumEnemySpawnPortal(PortalSpawnManager.Instance.Difficult);
        //Set Max Progress
        progress.maxValue = enemyNax;
    }
    private void UpdateProgress()
    {
        StartCoroutine(UpdateProgressCoroutine(progress.value, enemySpawn));
    }

    private IEnumerator UpdateProgressCoroutine(float startValue, float targetValue)
    {
        float elapsedTime = 0f;
        float duration = 1f;
        float initialValue = startValue;
        while (elapsedTime < duration)
        {
            // Tính toán giá trị tăng dần từ initialValue đến targetValue trong thời gian duration
            float lerpedValue = Mathf.Lerp(initialValue, targetValue, elapsedTime / duration);
            progress.value = lerpedValue; // Cập nhật giá trị của progress
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        progress.value = targetValue; // Đảm bảo rằng giá trị của progress đạt được giá trị cuối cùng là targetValue
    }
}
