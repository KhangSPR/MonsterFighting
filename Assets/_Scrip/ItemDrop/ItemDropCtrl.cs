using UnityEngine;
using DG.Tweening;


public abstract class ItemDropCtrl : MonoBehaviour
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
            Invoke("ItemPickupAnimation", Random.Range(2f, 3f));
        }
    }
    private void OnDisable()
    {

    }

    private void OnDestroy()
    {
        transform.DOKill(); // Hủy tất cả Tween trên Transform này
    }

    //protected void OnMouseDown()
    //{
    //    ItemPickupAnimation();
    //}

    public void ItemDropAnimation()
    {
        float scale = Random.Range(0, 1f);
        float angle = Random.Range(-90f, 90f);
        Vector3 target = transform.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * scale;
        transform.DOJump(target, Random.Range(0.3f, 0.7f), 1, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOKill();
        });
    }

    public void ItemPickupAnimation()
    {
        if (hasBeenPickedUp) return; // Kiểm tra nếu đã nhặt thì không thực hiện lại

        Debug.Log("PickupItem");
        hasBeenPickedUp = true;

        float scale = Random.Range(0.4f, 0.7f);
        float angle = Random.Range(-90f, 90f);
        Vector3 firstTarget = transform.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * scale;


        UITopLeft uiTopLeft = Map_Ui_Manager.instance.UI_Top_left.GetComponent<UITopLeft>();


        Transform target = uiTopLeft.TransformsResources[0];
        Debug.Log(target);

        isAnimationProcess = true;

        // Repair
        // Tăng chi phí
        this.OnReceiverItem();

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
    protected abstract void OnReceiverItem();
}
