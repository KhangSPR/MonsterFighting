using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandIndexScript : MonoBehaviour
{
    internal object position;
    [SerializeField] private int landIndex;
    public int LandIndex => landIndex;
    public void SetLandIndex(int index)
    {
        this.landIndex = index;
    }
}
