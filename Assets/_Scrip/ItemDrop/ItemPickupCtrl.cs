using UnityEngine;
using DG.Tweening;
public class ItemPickupCtrl : MonoBehaviour
{
    bool isAnimationProcess = false;
    bool hasBeenPickedUp = false;
    public bool IsAnimationProcess => isAnimationProcess;

    
    private void OnEnable()
    {
        hasBeenPickedUp = false;

        ItemDropAnimation();
        if (!hasBeenPickedUp) // Chỉ gọi Invoke nếu vật phẩm chưa được nhặt
        {
            Invoke("ItemPickupAnimation", Random.Range(3, 6));
        }
    }
    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        transform.DOKill(); // Hủy tất cả Tween trên Transform này
    }
    protected void OnMouseDown()
    {
        ItemPickupAnimation();
    }
    public void ItemDropAnimation()
    {
        //var angel = Random.Range()
        var scale = Random.Range(0, 1f);
        var angle = Random.Range(-90f, 90f);
        var target = transform.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * scale;
        transform.DOJump(target, Random.Range(0.3f, 0.7f), 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOKill();
        });
    }

    public void ItemPickupAnimation()
    {
        Debug.Log("PickupItem");
        if (hasBeenPickedUp) return; // Kiểm tra nếu đã nhặt thì không thực hiện lại
        hasBeenPickedUp = true;

        var scale = Random.Range(0.4f, 0.7f);
        var angle = Random.Range(-90f, 90f);
        var firstTarget = transform.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * scale;
        var target = Map_Ui_Manager.instance.UI_Top_left.GetComponent<UITopLeft>().TransformsResources[1];
        Debug.Log(target);
        isAnimationProcess = true;

        //++ COST
        CostManager.Instance.StoneEnemyCurrency += 1;


        transform.DOMove(firstTarget, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(target.position, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isAnimationProcess = false;
                Debug.Log("PickupItem");
                EnemyDropSpawner.Instance.Despawn(transform);
                transform.DOKill();
            });
        });

    }
    //private void OnDestroy()
    //{
    //    GameManager.Instance.EnemyCurrency += 1;
    //}
}
