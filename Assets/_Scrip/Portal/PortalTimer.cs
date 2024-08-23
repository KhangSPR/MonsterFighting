using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class PortalTimer : PortalSpawnManagerAbstract
{
    [Header("Active Portal")]
    [SerializeField] protected float timer;
    [SerializeField] protected float[] delayArray;

    [Header("TimeSpawns")]
    [SerializeField] float[] TimeSpawns;
    MapSO mapSOInstance;

    private Stack<float> delayStack = new Stack<float>();
    protected bool isReady = false;
    protected override void Start()
    {
        base.Start();
        //Set MapSO
        this.mapSOInstance = portalSpawnManagerCtrl.MapSO;

        //Set Time 
        this.TimeSpawns = this.mapSOInstance.TimeSpawnPortal(PortalSpawnManager.Instance.Difficult);


        //Set Timer Spawn
        this.TimerSpawn(TimeSpawns);

        //Push Stack
        for (int i = delayArray.Length - 1; i >= 0; i--)
        {
            this.delayStack.Push(delayArray[i]);
        }

    }
    protected override void Update()
    {
        // Increase timer
        this.timer += Time.deltaTime;

        this.CheckSpawnReady();
    }
    void TimerSpawn(float[] spawnTimes)
    {
        this.delayArray = new float[spawnTimes.Length];

        for (int i = 0; i < spawnTimes.Length; i++)
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

    private void CheckSpawnReady()
    {
        if (this.delayStack.Count > 0 && this.timer >= this.delayStack.Peek())
        {
            isReady = true;

            //Action spawn
            this.portalSpawnManagerCtrl.PortalSpawnAction.SpawnAction();


            this.Active();

            // Remove Delay Stack
            this.delayStack.Pop();

            if (this.delayStack.Count == 0)
            {
                // Reset timer
                this.timer = 0f;

                //Enable
                enabled = false;
            }
        }
    }
    public virtual void Active()
    {
        this.isReady = false;
        this.timer = 0;
    }
}
