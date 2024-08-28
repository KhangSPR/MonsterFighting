using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardClaimManager : MonoBehaviour
{
    private static RewardClaimManager instance;                             //instance variable
    public static RewardClaimManager Instance { get => instance; }          //instance getter
    public void Awake()
    {
        if (RewardClaimManager.instance != null)
        {
            Debug.LogError("Only 1 RewardClaimManager Warning");
        }
        RewardClaimManager.instance = this;
    }
    public void ClaimItemReward()
    {

    }

}
