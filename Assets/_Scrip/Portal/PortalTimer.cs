using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class PortalTimer : PortalSpawnManagerAbstract
{
    [Header("Active Portal")]
    [SerializeField] protected float timer;
    [SerializeField] protected float[] delayArray;

    [Header("TimeSpawns")]
    [SerializeField] List<float> TimeSpawns;

    private Stack<float> delayStack = new Stack<float>();
    protected bool isReady = false;

    protected override void Start()
    {
        base.Start();
        // Set MapSO
        // Set Time 
        //this.TimeSpawns = this.mapSOInstance.TimeSpawnPortal(portalSpawnManagerCtrl.CurrentWave);

        // Set Timer Spawn
        //this.TimerSpawn(TimeSpawns);

        // Push Stack
        for (int i = delayArray.Length - 1; i >= 0; i--)
        {
            this.delayStack.Push(delayArray[i]);
        }
    }

    protected override void Update()
    {
        if (!GameManager.Instance.ReadyTimer) return;
        // Increase timer
        this.timer += Time.deltaTime;

        this.CheckSpawnReady();
    }

    private void CheckSpawnReady()
    {
        if (this.delayStack.Count > 0 && this.timer >= this.delayStack.Peek())
        {
            isReady = true;

            // Action spawn
            this.portalSpawnManagerCtrl.PortalSpawnAction.SpawnAction();

            this.Active();

            // Remove Delay Stack
            this.delayStack.Pop();

            if (this.delayStack.Count == 0)
            {
                // Reset timer
                this.timer = 0f;

                // Enable
                enabled = false;
            }
        }
    }

    public virtual void Active()
    {
        this.isReady = false;
        this.timer = 0;
    }

    void TimerSpawn(List<float> spawnTimes)
    {
        this.delayArray = new float[spawnTimes.Count];

        for (int i = 0; i < spawnTimes.Count; i++)
        {
            if (i == 0)
            {
                // Ensure the first value is non-negative by taking its absolute
                this.delayArray[i] = Mathf.Abs(spawnTimes[i]);
            }
            else
            {
                // Calculate the delay using absolute difference
                this.delayArray[i] = Mathf.Abs(spawnTimes[i] - spawnTimes[i - 1]);
            }
        }
    }

    public void UpdateTimeSpawns(bool portalType)
    {
        if (portalSpawnManagerCtrl == null )
        {
            Debug.LogError("portalSpawnManagerCtrl or mapSOInstance is not set.");
            return;
        }

        // Ensure CurrentWave is not null
        if (portalSpawnManagerCtrl.CurrentWave == null)
        {
            Debug.LogError("CurrentWave is not set.");
            return;
        }
        if(portalType)
        {
            // Lấy TimeSpawns từ MapSO cho wave hiện tại
            this.TimeSpawns = portalSpawnManagerCtrl.MapSO.TimeSpawnWavePotal(portalSpawnManagerCtrl.CurrentWave);
        }
        else
        {
            this.TimeSpawns = portalSpawnManagerCtrl.MapSO.TimeSpawningPortal(portalSpawnManagerCtrl.CurrentWave);
        }

        // Cập nhật lại delayArray
        this.TimerSpawn(this.TimeSpawns);

        // Làm mới lại delayStack
        delayStack.Clear();
        for (int i = delayArray.Length - 1; i >= 0; i--)
        {
            this.delayStack.Push(delayArray[i]);
        }

        // Reset timer
        this.timer = 0f;

        // Kích hoạt PortalTimer lại nếu nó đang bị disable
        if (!enabled)
        {
            enabled = true;
        }
    }
}
