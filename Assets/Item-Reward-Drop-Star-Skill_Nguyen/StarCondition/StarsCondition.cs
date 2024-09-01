using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public class StarsCondition
{
    public uint threshold1;
    public uint threshold2;
    public uint threshold3;
    public uint currentThreshold;

    [Space]
    [Space]
    public string conditionDescription;

    public virtual void SetDefaultValue()
    {

    }

    public virtual uint CheckThreshold()
    {
        return 0;
    }
    public void CheckFirstTimeFullStars(bool isFullStar)
    {
        //var FirstTimeFullStars = GameDataManager.Instance.FirstTimeGetFullStars;
        //if (!isFullStar || FirstTimeFullStars) return;

        //GameDataManager.Instance.FirstTimeGetFullStars = true;
    }
}
