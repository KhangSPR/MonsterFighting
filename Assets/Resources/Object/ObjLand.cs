using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLand : AbstractCtrl
{
    [SerializeField] protected int landIndex;
    public int LandIndex => landIndex;
    public void SetLand(int index)
    {
        this.landIndex = index;
    }
    public void ResetLand()
    {
        this.landIndex = -1;
    }
    public bool CampareLand(int index)
    {
        return (this.landIndex == index);
    }
}
