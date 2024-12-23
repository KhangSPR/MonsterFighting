using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    private static EnergyManager instance;
    public static EnergyManager Instance { get => instance; set => instance = value; }

    public int currentEnergyAmount { get;private set; }
    [SerializeField] int maxEnergyAmount;

    [Tooltip("Restore 1 energy every ... seconds")]
    [SerializeField] float energyRestoreInterval;
    [SerializeField]
    float timeCount;

    DateTime lastRestoreTime;

    private void Awake(){
        if (instance == null){
            instance = this;
        }
    }

    private void Start(){
        currentEnergyAmount = CalculateEnergy();
    }

    private void Update(){
        RestoreEnergy();
    }

    private void OnApplicationQuit(){
        PlayerPrefs.SetString(nameof(lastRestoreTime), DateTime.Now.ToString());
        PlayerPrefs.SetInt(nameof(currentEnergyAmount), currentEnergyAmount);
    }

    public void ConsumeEnergy(){
        currentEnergyAmount -= (currentEnergyAmount > 0) ? 1 : 0;
    }

    public void AddEnergy(){
        currentEnergyAmount += (currentEnergyAmount < maxEnergyAmount) ? 1 : 0;
        lastRestoreTime = DateTime.Now;
        timeCount = energyRestoreInterval;
    }

    public string GetEnergyTimer(){
        int minutes = (int)(timeCount / 60); 
        int seconds = (int)(timeCount % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void RestoreEnergy(){
        timeCount = (currentEnergyAmount < maxEnergyAmount) ? timeCount - Time.deltaTime : 0;
        if (timeCount <= 0){

            Debug.Log("EnergyManager RestoreEnergy: " + currentEnergyAmount);
            AddEnergy();
        }
    }

    // Calculate energy from last recovery time
    private int CalculateEnergy(){
        int energy = PlayerPrefs.GetInt(nameof(currentEnergyAmount));
        try {
            lastRestoreTime = DateTime.Parse(PlayerPrefs.GetString(nameof(lastRestoreTime)));
        }
        catch (Exception){
            lastRestoreTime = DateTime.Now;
            return maxEnergyAmount;
        }
        float lastRestoreInterval = (float)(DateTime.Now - lastRestoreTime).TotalSeconds;
            energy += (int)(lastRestoreInterval / energyRestoreInterval);
            timeCount = energyRestoreInterval - lastRestoreInterval % energyRestoreInterval;
            lastRestoreTime.AddSeconds(energyRestoreInterval);
        return (energy > maxEnergyAmount) ? maxEnergyAmount : energy;
    }
}
