using DG.Tweening;
using System;
using System.Linq;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class UIWinGameController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] Transform RewardItem_Prefab;
    [SerializeField] LevelInfo LevelInfo;
    [SerializeField] MapSO mapSO;
    [Header("UI")]
    [SerializeField] Transform RewardHolder;
    [SerializeField] Transform RewardHolderFirstTime;
    [SerializeField] Transform Status;
    [SerializeField] Transform Sword1;
    [SerializeField] Transform Sword2;
    [SerializeField] Transform Shield;
    [SerializeField] Transform MapUI;
    [SerializeField] Transform ChooseCard;
    [SerializeField] Transform TitleGameFinish;
    [SerializeField] Transform Replay;
    [SerializeField] Transform NextGame;
    [SerializeField] Transform FirstTimeReward_Text;
    private void Awake()
    {
        mapSO = GameDataManager.Instance.currentMapSO;
        UIGameStart();
        DoAnimation();
        SpawnRewardItem();
        CheckStarsCondition();
        UnlockedNewLevel();
    }

    private void UnlockedNewLevel()
    {
        var aiso = LevelSystemManager.Instance.aiso;
        var currentMapSO = GameDataManager.Instance.currentMapSO;
        var currentId = -1;
        var currentAreaName = "";
        var currentAreaIndex = -1;
        foreach (var area in aiso.areasData)
        {
            foreach (var level in area.levelsData)
            {
                if (currentMapSO.id == level.levelIndex && LevelButton.GetTyMap(level.levelName) == currentMapSO.mapType)
                {
                    currentId = currentMapSO.id; currentAreaName = area.areaName;
                    for (int i = 0; i < aiso.areasData.Count; i++)
                    {
                        if (aiso.areasData[i].areaName == currentAreaName) currentAreaIndex = i;
                    }
                }
            }
        }
        if(currentId == aiso.areasData[currentAreaIndex].levelsData.Count - 1 && currentAreaIndex <= aiso.areasData.Count-1)
        {

            aiso.areasData[currentAreaIndex+1].levelsData[0].isUnlocked = true;
        } else if(currentId < aiso.areasData[currentAreaIndex].levelsData.Count - 1)
        {
            aiso.areasData[currentAreaIndex].levelsData[currentId+1].isUnlocked = true;
        }

        Debug.Log($"UnlockedNewLevel:currentMapSO:{currentMapSO}");
        Debug.Log($"UnlockedNewLevel:currentId:{currentId}, currentAreaName: {currentAreaName}, currentAreaIndex : {currentAreaIndex}");
    }

    void CheckStarsCondition()
    {
        var oldStarsCount = mapSO.GetStarsCount(mapSO.difficult);
        var starResult = mapSO.GetStarsCondition(mapSO.difficult).CheckThreshold();

        if (oldStarsCount < starResult) mapSO.SetStarsCount(mapSO.difficult, (int)starResult) ;
        mapSO.GetStarsCondition(mapSO.difficult).CheckFirstTimeFullStars((int)starResult >= 3);
        Debug.Log("Star :" + starResult);
    }
    void UIGameStart()
    {
        RewardHolder.gameObject.SetActive(false);
        Sword1.gameObject.SetActive(false);
        Sword2.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
        MapUI.gameObject.SetActive(false);
        ChooseCard.gameObject.SetActive(false);
        TitleGameFinish.gameObject.SetActive(false);
        Replay.gameObject.SetActive(false);
        NextGame.gameObject.SetActive(false);
        RewardHolderFirstTime.gameObject.SetActive(false);
        FirstTimeReward_Text.gameObject.SetActive(false);
    }
    [ContextMenu("DoAnimation")]
    private void DoAnimation()
    {
        Debug.Log("Do Animation UI Win Game ");
        Shield.gameObject.SetActive(true);
        Sword1.gameObject.SetActive(true);
        Sword1.transform.position = Sword1.transform.position - Sword1.up* 2;
        Sword1.DOMove(Sword1.transform.position - Sword1.up * 0.3f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Sword1.DOMove(Sword1.transform.position + Sword1.up * 2.3f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Sword2.gameObject.SetActive(true);
                Sword2.transform.position -= Sword2.up * 2f;
                Sword2.DOMove(Sword2.transform.position - Sword2.up * 0.3f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Sword2.DOMove(Sword2.transform.position + Sword2.up * 2.3f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        Status.DOMove(Status.transform.position + Status.up * 3.3f, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f).OnStart(() => 
                        { 
                            Sword1.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
                            Sword2.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
                            Shield.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                TitleGameFinish.gameObject.SetActive(true);
                                RewardHolder.gameObject.SetActive(true);
                                foreach(Transform item in RewardHolder)
                                {
                                    item.DOScale(Vector3.zero, 0f);
                                }
                                foreach (Transform item in RewardHolderFirstTime)
                                {
                                    item.DOScale(Vector3.zero, 0f);
                                }
                                RewardHolder.localScale = new Vector3(0, 1,1);
                                RewardHolderFirstTime.localScale = new Vector3(0, 1, 1);
                                RewardHolder.DOScale(Vector3.one, 1.3f).SetDelay(0.2f).OnComplete(() =>
                                {
                                    Sequence sq = DOTween.Sequence();
                                    foreach(Transform item in RewardHolder)
                                    {
                                        sq.Append(item.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                                        {
                                            item.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
                                        }));
                                    }
                                    sq.OnComplete(() =>
                                    {
                                        FirstTimeReward_Text.gameObject.SetActive(true);
                                        RewardHolderFirstTime.gameObject.SetActive(true);
                                        RewardHolderFirstTime.DOScale(Vector3.one, 1.3f).SetDelay(0.2f).OnComplete(() =>
                                        {
                                            Sequence sq = DOTween.Sequence();
                                            foreach (Transform firstTimeItem in RewardHolderFirstTime)
                                            {
                                                sq.Append(firstTimeItem.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                                                {
                                                    firstTimeItem.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
                                                })).OnComplete(() =>
                                                {
                                                    SetRewardOneTimme();
                                                    MapUI.gameObject.SetActive(true);
                                                    ChooseCard.gameObject.SetActive(true);
                                                    NextGame.gameObject.SetActive(true);
                                                    Replay.gameObject.SetActive(true);
                                                });
                                            }
                                        });
                                        
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }
    void SetRewardOneTimme()
    {
        switch(mapSO.difficult){
            case Difficult.Easy:
                GameDataManager.Instance.currentMapSO.isOneTimeRewardGotEasy = true; break;
                case Difficult.Normal: GameDataManager.Instance.currentMapSO.isOneTimeRewardGotHard = true; break;
                case Difficult.Hard : GameDataManager.Instance.currentMapSO.isOneTimeRewardGotEasy = true; break;
        }
        Debug.Log("Dòng lệnh này sẽ dùng để Add vào Inventory");
    }
    void SpawnRewardItem()
    {
        switch (mapSO.difficult)
        {
            case Difficult.Easy:
                {
                    Debug.Log(mapSO.RewardEasy.Length);
                    foreach (var item in mapSO.RewardEasy)
                    {
                        Debug.Log(item);
                        Debug.Log(item.item);
                        Debug.Log(item.item.Image);
                        Debug.Log(item.Count);
                        GameDataManager.Instance.GameData.enemyStone += (uint)item.Count;// sau này cần sửa đoạn code này


                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

                    }

                    foreach (var itemFirstTime in mapSO.OneTimeRewardEasy)
                    {
                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolderFirstTime).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = !mapSO.isOneTimeRewardGotEasy ? itemFirstTime.item.Image : itemFirstTime.item.ImageBnW;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{itemFirstTime.Count}";
                    }
                    break;
                }
            case Difficult.Normal: {
                    Debug.Log(mapSO.RewardNormal.Length);
                    foreach (var item in mapSO.RewardNormal)
                    {
                        Debug.Log(item);
                        Debug.Log(item.item);
                        Debug.Log(item.item.Image);
                        Debug.Log(item.Count);
                        GameDataManager.Instance.GameData.enemyStone += (uint)item.Count;// sau này cần sửa đoạn code này


                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

                    }

                    foreach (var itemFirstTime in mapSO.OneTimeRewardNormal)
                    {
                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolderFirstTime).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = !mapSO.isOneTimeRewardGotNormal ? itemFirstTime.item.Image : itemFirstTime.item.ImageBnW;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{itemFirstTime.Count}";
                    }
                } break;
            case Difficult.Hard: {
                    Debug.Log(mapSO.RewardHard.Length);
                    foreach (var item in mapSO.RewardHard)
                    {
                        Debug.Log(item);
                        Debug.Log(item.item);
                        Debug.Log(item.item.Image);
                        Debug.Log(item.Count);
                        GameDataManager.Instance.GameData.enemyStone += (uint)item.Count;// sau này cần sửa đoạn code này


                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

                    }

                    foreach (var itemFirstTime in mapSO.OneTimeRewardHard)
                    {
                        GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolderFirstTime).gameObject;
                        rewardItem.transform.Find("Img").GetComponent<Image>().sprite = !mapSO.isOneTimeRewardGotHard ? itemFirstTime.item.Image : itemFirstTime.item.ImageBnW;
                        rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{itemFirstTime.Count}";
                    }
                } break;
        }
        
        
    }
}
