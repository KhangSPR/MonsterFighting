using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Transform StarSpawnHolder;
    [SerializeField] List<Transform> _VFXWing;
    [SerializeField] List<Transform> _VFXStar;


    [SerializeField] private _Object_Shake _object_Shake;
    //[SerializeField] Transform TargetSword;

    MapDifficulty mapDifficulty;

    //[Space]
    //[Space]
    //[Space]
    //[Space]
    //[Header("MissionStar Panel")]
    //[SerializeField] Transform MissionPanel;
    //[SerializeField] Transform StarUI;
    //[SerializeField] Transform MissionInfomationUI;
    #region Mission Star
    //public void CloseMissionPanel()
    //{
    //    MissionPanel.gameObject.SetActive(false);   
    //}
    #endregion
    public MapDifficulty MapDifficulty { get { return mapDifficulty; } set { mapDifficulty = value; } }

    private void Awake()
    {
        UIGameStart();
        DoAnimation();
        SpawnRewardItem();
        /////////////////
        //CloseMissionPanel();
    }
    void DeActiveListStar()
    {
        foreach (Transform obj in _VFXStar)
        {
            obj.gameObject.SetActive(false);
        }
    }
    void UIGameStart()
    {
        RewardHolder.gameObject.SetActive(false);
        Sword1.gameObject.SetActive(false);
        Sword2.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
        TitleGameFinish.gameObject.SetActive(false);
        HolderBtn.gameObject.SetActive(false);
        HolderBtn.transform.Find("NextDifficulty").gameObject.SetActive(false);
        DeActiveListStar();
    }
    [ContextMenu("DoAnimation")]
    private void DoAnimation()
    {
        StartCoroutine(DoAnimationSequence());
    }

    private IEnumerator DoAnimationSequence()
    {
        Debug.Log("Do Animation UI Win Game ");
        Shield.gameObject.SetActive(true);
        Sword1.gameObject.SetActive(true);

        yield return Sword1.DOMove(Sword1.transform.position - Sword1.up * 10.5f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        _object_Shake._Shake(true);
        yield return Sword1.DOMove(Sword1.transform.position + Sword1.up * 2f, 0.15f).SetEase(Ease.Linear).WaitForCompletion();

        _object_Shake._Shake(false);
        Sword2.gameObject.SetActive(true);
        yield return Sword2.DOMove(Sword2.transform.position - Sword2.up * 10.5f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        _object_Shake._Shake(true);
        yield return Sword2.DOMove(Sword2.transform.position + Sword2.up * 2f, 0.15f).SetEase(Ease.Linear).WaitForCompletion();

        _object_Shake._Shake(false);
        foreach (Transform vfxWing in _VFXWing)
        {
            vfxWing.gameObject.SetActive(true);
        }

        int countStar = GameManager.Instance.Star;

        if (countStar < 0 || countStar > 3) yield break;  // Sửa điều kiện

        // Đợi cho đến khi hoàn thành việc kích hoạt VFX Stars
        yield return StartCoroutine(ActivateVFXStarsBasedOnCount(countStar));

        yield return Status.DOMove(Status.transform.position + Status.up * 2.3f, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f).WaitForCompletion();

        Sword1.DOScale(Vector3.one * 2.3f, 0.3f).SetEase(Ease.Linear);
        Sword2.DOScale(Vector3.one * 2.3f, 0.3f).SetEase(Ease.Linear);
        StarSpawnHolder.DOScale(Vector3.one *2.3f,0.3f).SetEase(Ease.Linear);
        yield return Shield.DOScale(Vector3.one * 2.3f, 0.3f).SetEase(Ease.Linear).WaitForCompletion();

        TitleGameFinish.gameObject.SetActive(true);
        RewardHolder.gameObject.SetActive(true);

        foreach (Transform item in RewardHolder)
        {
            item.DOScale(Vector3.zero, 0f);
        }
        RewardHolder.localScale = new Vector3(0, 1, 1);
        yield return RewardHolder.DOScale(Vector3.one, 1.3f).WaitForCompletion();

        DG.Tweening.Sequence sq = DOTween.Sequence();
        foreach (Transform item in RewardHolder)
        {
            sq.Append(item.DOScale(Vector3.one * 1.2f, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
            {
                item.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
            }));
        }

        sq.OnComplete(() =>
        {
            HolderBtn.gameObject.SetActive(true);
            ActiveBtnDifficult();
        });
    }

    IEnumerator ActivateVFXStarsBasedOnCount(int countStar)
    {
        Debug.Log("Start activating VFX Stars");
        countStar = Mathf.Min(countStar, _VFXStar.Count);

            Debug.Log("CountStar: " + countStar);
        for (int i = 0; i < countStar; i++)
        {
            _VFXStar[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(0.35f);
        }

        Debug.Log("Finished activating VFX Stars");
    }

    private void ActiveBtnDifficult()
    {
        if (GameManager.Instance.CheckBtnDifficult())
            HolderBtn.transform.Find("NextDifficulty").gameObject.SetActive(true);
    }
    void SpawnRewardItem()
    {
        if (mapDifficulty.isReceivedReWard) return;

        Debug.Log("Spawn Item");
        foreach (var item in mapDifficulty.Reward)
        {

            GameDataManager.Instance.GameData.StoneEnemy += (uint)item.Count;

            GameObject rewardItem = Instantiate(RewardItem_Prefab, RewardHolder).gameObject;
            rewardItem.transform.Find("Img").GetComponent<Image>().sprite = item.item.Image;
            rewardItem.transform.Find("Count").GetComponent<Text>().text = $"x{item.Count}";

        }
    }
}
