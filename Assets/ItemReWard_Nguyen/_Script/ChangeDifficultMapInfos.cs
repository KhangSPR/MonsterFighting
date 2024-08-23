using UIGameDataMap;
using UnityEngine;

public class ChangeDifficultMapInfos : SaiMonoBehaviour
{
    [SerializeField] ChangeDifficultMapInfo[] changeDifficultMapInfos;
    public ChangeDifficultMapInfo[] ChangeDifficultMapInfo => changeDifficultMapInfos;

    [SerializeField] GameObject[] HolderLock;
    [SerializeField] GameObject[] HolderPVP;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChangeDifficultMapInfo();
        this.LoadHolder();
        this.LoadHolderPVP();
    }

    protected virtual void LoadChangeDifficultMapInfo()
    {
        if (this.changeDifficultMapInfos == null || this.changeDifficultMapInfos.Length == 0)
        {
            this.changeDifficultMapInfos = transform.GetComponentsInChildren<ChangeDifficultMapInfo>();
        }
    }
    protected virtual void LoadHolder()
    {
        if (HolderLock.Length > 0) return;

        HolderLock = new GameObject[changeDifficultMapInfos.Length];

        for (int i = 0; i < changeDifficultMapInfos.Length; i++)
        {
            if (changeDifficultMapInfos[i] != null && changeDifficultMapInfos[i].transform.childCount > 0)
            {
                HolderLock[i] = changeDifficultMapInfos[i].transform.GetChild(0).gameObject;
            }
        }
    }
    protected virtual void LoadHolderPVP()
    {
        if (HolderPVP.Length > 0) return;
        HolderPVP = new GameObject[HolderLock.Length];

        for (int i = 0; i < HolderLock.Length; i++)
        {
            HolderPVP[i] = HolderLock[i].transform.GetChild(0).gameObject;
        }

    }
    public void SetUnlockDifficult(MapSO mapSO)
    {
        // Kiểm tra số sao của các DifficultyMap
        bool hasEasyDifficulty = mapSO.DifficultyMap[0].stars == 3;
        bool hasNormalDifficulty = mapSO.DifficultyMap[1].stars == 3;
        bool hasHardDifficulty = mapSO.DifficultyMap[2].stars == 3;

        Debug.Log("Easy: " + hasEasyDifficulty + " Normal: " + hasNormalDifficulty + "Hard: " + hasHardDifficulty);

        if (hasEasyDifficulty && hasNormalDifficulty && hasHardDifficulty)
        {
            HolderLock[1].SetActive(false);
            changeDifficultMapInfos[1].Button.enabled = false;

            HolderLock[2].SetActive(false);
            changeDifficultMapInfos[2].Button.enabled = true;
        }
        else if (hasEasyDifficulty && hasNormalDifficulty && !hasHardDifficulty)
        {
            HolderLock[2].SetActive(true);
            changeDifficultMapInfos[2].Button.enabled = true;

        }
        else if (hasEasyDifficulty && !hasNormalDifficulty && !hasHardDifficulty)
        {
            HolderLock[1].SetActive(false);
            changeDifficultMapInfos[1].Button.enabled = true;

            HolderLock[2].SetActive(true);
            changeDifficultMapInfos[2].Button.enabled = false;
        }
        else if (!hasNormalDifficulty && !hasHardDifficulty && !hasHardDifficulty)
        {
            HolderLock[1].SetActive(true);
            changeDifficultMapInfos[1].Button.enabled = false;

            HolderLock[2].SetActive(true);
            changeDifficultMapInfos[2].Button.enabled = false;
        }
    }

}
