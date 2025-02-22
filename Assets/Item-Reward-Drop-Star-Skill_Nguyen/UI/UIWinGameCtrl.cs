using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UIGameDataMap;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIWinGameCtrl : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform rewardHolder;
    [SerializeField] private Transform status;
    [SerializeField] private Transform sword1;
    [SerializeField] private Transform sword2;
    [SerializeField] private Transform shield;
    [SerializeField] private Transform titleGameFinish;
    [SerializeField] private Transform holderBtn;
    [SerializeField] private Transform starSpawnHolder;
    [SerializeField] private List<Transform> vfxWing;
    [SerializeField] private List<Transform> vfxStar;

    [SerializeField] private Object_ShakeRectTransfrom objectShake;

    private MapDifficulty mapDifficulty;

    public MapDifficulty MapDifficulty { get => mapDifficulty; set => mapDifficulty = value; }

    private void Awake()
    {
        InitializeUI();
        DoAnimation();
        SpawnRewardItems();
    }

    private void InitializeUI()
    {
        rewardHolder.gameObject.SetActive(false);
        sword1.gameObject.SetActive(false);
        sword2.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
        titleGameFinish.gameObject.SetActive(false);
        holderBtn.gameObject.SetActive(false);
        holderBtn.Find("NextDifficulty").gameObject.SetActive(false);

        foreach (Transform star in vfxStar)
        {
            star.gameObject.SetActive(false);
        }
    }

    [ContextMenu("DoAnimation")]
    private void DoAnimation()
    {
        StartCoroutine(PlayAnimationSequence());
    }

    private IEnumerator PlayAnimationSequence()
    {
        Debug.Log("Starting UI Win Game Animation");

        shield.gameObject.SetActive(true);
        sword1.gameObject.SetActive(true);

        yield return sword1.DOMove(sword1.position - sword1.up * 11.5f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        objectShake._Shake(true);
        yield return sword1.DOMove(sword1.position + sword1.up * 1.6f, 0.16f).SetEase(Ease.Linear).WaitForCompletion();

        objectShake._Shake(false);
        sword2.gameObject.SetActive(true);
        yield return sword2.DOMove(sword2.position - sword2.up * 11.5f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        objectShake._Shake(true);
        yield return sword2.DOMove(sword2.position + sword2.up * 1.6f, 0.16f).SetEase(Ease.Linear).WaitForCompletion();

        objectShake._Shake(false);
        foreach (Transform wing in vfxWing)
        {
            wing.gameObject.SetActive(true);
        }

        int starCount = Mathf.Clamp(GameManager.Instance.Star, 0, vfxStar.Count);
        yield return StartCoroutine(ActivateStars(starCount));

        yield return status.DOMove(status.position + status.up * 2.3f, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f).WaitForCompletion();

        AnimateScaling(sword1, 2.3f);
        AnimateScaling(sword2, 2.3f);
        AnimateScaling(starSpawnHolder, 2.3f);
        yield return shield.DOScale(Vector3.one * 2.3f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        titleGameFinish.gameObject.SetActive(true);
        rewardHolder.gameObject.SetActive(true);
        AnimateRewardHolder();
    }

    private IEnumerator ActivateStars(int starCount)
    {
        Debug.Log("Activating stars: " + starCount);
        for (int i = 0; i < starCount; i++)
        {
            vfxStar[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.35f);
        }
    }

    private void AnimateScaling(Transform target, float scale)
    {
        target.DOScale(Vector3.one * scale, 0.3f).SetEase(Ease.Linear);
    }

    private void AnimateRewardHolder()
    {
        foreach (Transform item in rewardHolder)
        {
            item.localScale = Vector3.zero;
        }
        rewardHolder.localScale = new Vector3(0, 1, 1);
        rewardHolder.DOScale(Vector3.one, 1.3f).OnComplete(() =>
        {
            Sequence rewardSequence = DOTween.Sequence();
            foreach (Transform item in rewardHolder)
            {
                rewardSequence.Append(item.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    item.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
                }));
            }
            rewardSequence.OnComplete(() =>
            {
                holderBtn.gameObject.SetActive(true);
                ActivateNextDifficultyButton();
            });
        });
    }

    private void ActivateNextDifficultyButton()
    {
        if (GameManager.Instance.CheckBtnDifficult())
        {
            holderBtn.Find("NextDifficulty").gameObject.SetActive(true);
        }
    }

    private void SpawnRewardItems()
    {
        if (mapDifficulty.isReceivedReWard) return;

        foreach(Transform item in rewardHolder)
        {
            Destroy(item.gameObject);
        }

        Debug.Log("Spawning reward items");
        foreach (var item in mapDifficulty.Reward)
        {
            GameDataManager.Instance.GameData.StoneEnemy += (uint)item.Count;

            GameObject rewardItem = CreateResourceItem(item);
        }
    }
    private GameObject CreateResourceItem(UIGameDataMap.Resources resource)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemReward, rewardHolder).gameObject;
        objNew.SetActive(false);

        var itemTooltip = objNew.GetComponent<ItemTooltipReward>();
        itemTooltip.Avatar.sprite = resource.item.Image;
        itemTooltip.CountTxt.text = $"x{resource.Count}";
        itemTooltip.ItemReward = resource.item;
        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(resource.item.itemRarity);

        GameDataManager.Instance.OnReceiverRewardResources(resource);

        return objNew;
    }

    private GameObject CreateInventoryItem(InventoryItem item)
    {
        var objNew = Instantiate(RewardClaimManager.Instance.ItemObject, rewardHolder).gameObject;
        objNew.SetActive(false);

        var itemTooltip = objNew.GetComponent<ItemTooltipInventory>();
        itemTooltip.Avatar.sprite = item.itemObject.Sprite;
        itemTooltip.CountTxt.text = $"x{item.count}";
        itemTooltip.ItemObject = item.itemObject;
        itemTooltip.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(item.itemObject.itemRarity);

        InventoryManager.Instance.inventory.AddItem(new Item(item.itemObject), item.count);

        return objNew;
    }
}
