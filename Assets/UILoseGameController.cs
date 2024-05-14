using DG.Tweening;

using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;

public class UILoseGameController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] Transform RewardItem_Prefab;
    [SerializeField] LevelInfo LevelInfo;
    [SerializeField] MapSO mapSO;
    [Header("UI")]
    [SerializeField] Transform RewardHolder;
    [SerializeField] Transform Status;
    [SerializeField] Transform Sword1;
    [SerializeField] Sprite Sword1_BnW;
    [SerializeField] Sprite Sword2_BnW;
    [SerializeField] Sprite Shield_BnW;
    [SerializeField] Sprite RewardItemBnW; // Sau này cần sửa dòng code này
    [SerializeField] Transform Sword2;
    [SerializeField] Transform Shield;
    [SerializeField] Transform MapUI;
    [SerializeField] Transform ChooseCard;
    [SerializeField] Transform TitleGameFinish;
    [SerializeField] Transform Replay;
    //[SerializeField] Transform NextGame;
    private void Awake()
    {
        mapSO = GameDataManager.Instance.currentMapSO;
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
        MapUI.gameObject.SetActive(false);
        ChooseCard.gameObject.SetActive(false);
        TitleGameFinish.gameObject.SetActive(false);
        Replay.gameObject.SetActive(false);
        //NextGame.gameObject.SetActive(false);
    }
    [ContextMenu("DoAnimation")]
    private void DoAnimation()
    {
        Debug.Log("Do Animation UI Win Game ");
        Shield.gameObject.SetActive(true);
        Sword1.gameObject.SetActive(true);
        Sword1.transform.position = Sword1.transform.position - Sword1.up * 2;
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
                        Status.DOMove(Status.transform.position + Status.up * 2.6f, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f).OnStart(() =>
                        {
                            Sword1.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
                            Sword2.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
                            Shield.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                Sword1.GetComponent<Image>().sprite = Sword1_BnW;
                                Sword2.GetComponent<Image>().sprite = Sword2_BnW;
                                Shield.GetComponent<Image>().sprite = Shield_BnW;
                                TitleGameFinish.gameObject.SetActive(true);
                                RewardHolder.gameObject.SetActive(true);
                                foreach (Transform item in RewardHolder)
                                {
                                    item.DOScale(Vector3.zero, 0f);
                                }
                                RewardHolder.localScale = new Vector3(0, 1, 1);
                                RewardHolder.DOScale(Vector3.one, 1.3f).OnComplete(() =>
                                {
                                    Sequence sq = DOTween.Sequence();
                                    foreach (Transform item in RewardHolder)
                                    {
                                        sq.Append(item.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                                        {
                                            item.GetChild(0).GetComponent<Image>().sprite = RewardItemBnW; // Sau này cần sửa dòng code này
                                            item.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
                                        }));
                                    }
                                    sq.OnComplete(() =>
                                    {
                                        MapUI.gameObject.SetActive(true);
                                        ChooseCard.gameObject.SetActive(true);
                                        //NextGame.gameObject.SetActive(true);
                                        Replay.gameObject.SetActive(true);
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
        Debug.Log(mapSO.Reward.Length);
        foreach (var item in mapSO.Reward)
        {
            Debug.Log(item);
            Debug.Log(item.item);
            Debug.Log(item.item.Image);
            Debug.Log(item.Count);
            GameDataManager.Instance.GameData.enemyStone += (uint)item.Count;


            GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
            rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
            rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

        }
    }
}
