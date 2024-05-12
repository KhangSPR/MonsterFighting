using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/AreasData")]
public class AreaInfomationSO : ScriptableObject
{
    public List<AreasData> areasData;

    public void Reset()
    {
        for (int i = 0; i < areasData.Count; i++)
        {
            areasData[i].areaIndex = i;

            for(int j = 0;j< areasData[i].levelsData.Count; j++)
            {
                var levelData = areasData[i].levelsData[j];
                levelData.isUnlocked = false;
                levelData.levelIndex = j;
                levelData.levelName = j + "";

            }
        }
        areasData[0].levelsData[0].isUnlocked = true;
    }
}
