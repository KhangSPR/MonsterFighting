using System;
using UnityEngine;

// Base class for Timer Handling
public abstract class BaseTimerHandler
{
    public DateTime TimerNow { get; protected set; }
    public DateTime TimerNext { get; protected set; }
    public TimeSpan TimerDelta => TimerNext - TimerNow;

    protected string playerPrefsKey;

    public BaseTimerHandler(string key)
    {
        playerPrefsKey = key;
        TimerNow = DateTime.Now;
        LoadTimer(); //Load Timer When Create
    }

    public void UpdateTimer()
    {
        TimerNow = DateTime.Now;
    }

    public void ResetTimer()
    {
        TimerNext = DateTime.Now.AddSeconds(-1); // Set time to a past value.
    }

    public string GetTimerString()
    {
        return $"{TimerDelta.Hours:D2}:{TimerDelta.Minutes:D2}:{TimerDelta.Seconds:D2}";
    }
    public string GetTimerWatch()
    {
        return $"{TimerDelta.Minutes:D2}:{TimerDelta.Seconds:D2}";
    }
    public void SetNextDay000(int resetHours, int resetMinutes)
    {
        TimerNext = TimerNow.AddDays(1).Date.AddHours(resetHours).AddMinutes(resetMinutes);
    }
    public void SetWatch(float additionalTimeInSeconds)
    {
        TimerNext = TimerNow.AddSeconds(additionalTimeInSeconds);

        Debug.Log("TimerNext: " + TimerNext);
    }

    public void SaveTimer()
    {
        PlayerPrefs.SetString(playerPrefsKey, TimerNext.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public void LoadTimer()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            TimerNext = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(playerPrefsKey)));

            Debug.Log("Key: " + playerPrefsKey + "TimerNext: " + TimerNext);
        }
    }
    public void LastElapsedTimeWatch()
    { 
    }
    public void RemoveTimerKey()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
    }
}
