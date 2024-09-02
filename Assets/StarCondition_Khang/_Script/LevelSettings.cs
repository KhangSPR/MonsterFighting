using System;
using System.Collections.Generic;

public class LevelSettings
{
    public string levelName; // Tên chế độ chơi
    public List<ILevelCondition> starConditions; // Danh sách các điều kiện để đạt sao

    // Constructor để khởi tạo danh sách điều kiện


    //Event
    public static event Action HpPercentage;

    public LevelSettings()
    {
        starConditions = new List<ILevelCondition>();
    }
    public string GetLevelSettings()
    {
        List<string> levelSettingsList = new List<string>();

        foreach (ILevelCondition condition in starConditions)
        {
            switch (condition)
            {
                case HpPercentageCondition hpPercentage:
                    levelSettingsList.Add(hpPercentage.RequiredHpPercentage.ToString()+"% hp");
                    break;

                case HpAndTimeCondition hpAndTime:
                    levelSettingsList.Add(hpAndTime.RequiredHpPercentage.ToString()+"% hp");
                    levelSettingsList.Add(hpAndTime.RequiredCompletionTime.ToString()+"s");
                    break;

                case TimeCondition timeCondition:
                    levelSettingsList.Add(timeCondition.RequiredTime.ToString()+"s");
                    break;

            }
        }
        string result = string.Join(", ", levelSettingsList);

        return result;
    }
    public void CheckStarCondition()
    {
        foreach (ILevelCondition condition in starConditions)
        {
            switch (condition)
            {
                case HpPercentageCondition hpPercentage:
                    HpPercentage?.Invoke();
                    break;

                case HpAndTimeCondition hpAndTime:
                    break;

                case TimeCondition timeCondition:
                    break;

            }
        }
    }
}
