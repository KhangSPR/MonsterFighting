using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public class StarsCondition : ScriptableObject
{
    public float threshold1;
    public float threshold2;
    public float threshold3;

    public virtual void SetDefaultValue()
    {

    }

    public virtual float CheckThreshold()
    {
        return 0;
    }
    public void CheckFirstTimeFullStars(bool isFullStar)
    {
        var FirstTimeFullStars = GameDataManager.Instance.FirstTimeGetFullStars;
        if (!isFullStar || FirstTimeFullStars) return;

        GameDataManager.Instance.FirstTimeGetFullStars = true;


    }
}
