using DG.Tweening;

using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;


public class UIWinGameController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] Transform RewardItem_Prefab;
   
    [Header("UI")]
    [SerializeField] Transform RewardHolder;
    [SerializeField] Transform Status;
    [SerializeField] Transform Sword1;
    [SerializeField] Transform Sword2;
    [SerializeField] Transform Shield;
    [SerializeField] Transform TitleGameFinish;
    [SerializeField] Transform HolderBtn;

    MapDifficulty mapDifficulty;
    public MapDifficulty MapDifficulty { get { return mapDifficulty; } set {  mapDifficulty = value; } }

    private void Awake()
    {
        UIGameStart();
        DoAnimation();
        SpawnRewardItem();
    }
    void UIGameStart()
    {
        RewardHolder.gameObject.SetActive(false);
        Sword1.gameObject.SetActive(false);
        Sword2.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
        TitleGameFinish.gameObject.SetActive(false);
        HolderBtn.gameObject.SetActive(false);
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
                        Status.DOMove(Status.transform.position + Status.up * 2.3f, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f).OnStart(() => 
                        { 
                            Sword1.DOScale(Vector3.one*2, 0.3f).SetEase(Ease.Linear);
                            Sword2.DOScale(Vector3.one*2, 0.3f).SetEase(Ease.Linear);
                            Shield.DOScale(Vector3.one*2, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                TitleGameFinish.gameObject.SetActive(true);
                                RewardHolder.gameObject.SetActive(true);
                                foreach(Transform item in RewardHolder)
                                {
                                    item.DOScale(Vector3.zero, 0f);
                                }
                                RewardHolder.localScale = new Vector3(0, 1,1);
                                RewardHolder.DOScale(Vector3.one, 1.3f).OnComplete(() =>
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
                                        HolderBtn.gameObject.SetActive(true);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    void SpawnRewardItem()
    {
        if (mapDifficulty.isReceivedReWard) return;

        Debug.Log("Spawn Item");
        foreach (var item in mapDifficulty.Reward)
        {

            GameDataManager.Instance.GameData.enemyStone += (uint)item.Count;

            GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
            rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
            rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

        }
    }
}
