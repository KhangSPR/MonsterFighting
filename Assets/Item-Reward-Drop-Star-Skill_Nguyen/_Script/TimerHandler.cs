using System;
using UnityEngine;

public class TimerHandler
{
    public DateTime TimerNow { get; private set; }
    public DateTime TimerNextDay { get; private set; }
    public TimeSpan TimerDelta => TimerNextDay - TimerNow;

    private string playerPrefsKey = "RewardTimerSaved";
    public TimerHandler(int rewardResetHours, int rewardResetMinutes)
    {
        TimerNow = DateTime.Now;
        TimerNextDay = TimerNow.AddDays(-1);
        TimerNextDay = new DateTime(TimerNow.Year, TimerNow.Month, TimerNow.Day, rewardResetHours, rewardResetMinutes, 0, 0);
    }

    public void UpdateTimer()
    {
        TimerNow = DateTime.Now;
    }
    public void ResetTimer()
    {
        TimerNextDay = DateTime.Now.AddSeconds(-1); // Đặt thời gian đã qua.
    }
    public string GetTimerString()
    {
        return $"{TimerDelta.Hours:D2}:{TimerDelta.Minutes:D2}:{TimerDelta.Seconds:D2}s";
    }

    public void SetNextDay(int rewardResetHours, int rewardResetMinutes)
    {
        TimerNextDay = TimerNow.AddDays(1).Date.AddHours(rewardResetHours).AddMinutes(rewardResetMinutes);
    }
    public void SaveTimer()
    {
        PlayerPrefs.SetString(playerPrefsKey, TimerNextDay.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public void LoadTimer()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            TimerNextDay = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString(playerPrefsKey)));
        }
    }

    public void RemoveTimerKey()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
    }
}
