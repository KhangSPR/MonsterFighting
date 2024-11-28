using UnityEngine;
using DG.Tweening;

public class ParabolicMovement : MonoBehaviour
{
    public GameObject obj1; // Đối tượng 1 (trái)
    public GameObject obj2; // Đối tượng 2 (phải)
    public float duration;
    public float horizontalDistance;
    public float verticalDistance;

    public ObjectCtrl objCtrl;
    protected void Update()
    {
        if (this.objCtrl!=null && !this.objCtrl.transform.gameObject.activeSelf)
        {
            BarSpawner.Instance.Despawn(transform);
        }
    }
    void OnEnable()
    {
        ResetAnimation();
        // Khởi chạy animation lần đầu
        StartAnimation();
    }
    void StartAnimation()
    {
        Debug.Log("StartAnimation");

        // Điểm kết thúc của obj1 (trái)
        Vector3 endPositionObj1 = new Vector3(transform.position.x - horizontalDistance, transform.position.y + verticalDistance, transform.position.z);

        // Điểm kết thúc của obj2 (phải)
        Vector3 endPositionObj2 = new Vector3(transform.position.x + horizontalDistance, transform.position.y + verticalDistance, transform.position.z);

        // Di chuyển obj1 (trái) theo trục X và Y
        obj1.transform.DOMove(endPositionObj1, duration).SetEase(Ease.OutQuad);

        // Di chuyển obj2 (phải) theo trục X và Y
        obj2.transform.DOMove(endPositionObj2, duration).SetEase(Ease.OutQuad);
    }
    void ResetAnimation()
    {
        // Đặt vị trí ban đầu của obj1 (trái)
        obj1.transform.position = transform.position;

        // Đặt vị trí ban đầu của obj2 (phải)
        obj2.transform.position = transform.position;

        // Dừng mọi animation đang chạy trên obj1 và obj2 (nếu có)
        obj1.transform.DOKill();
        obj2.transform.DOKill();
    }
    public void OnClickComplete()
    {
        if(this.objCtrl is PlayerCtrl ctrl)
        {
            PlayerSpawner.Instance.Despawn(this.objCtrl.transform);
        }
        this.CloseObj();

        GameManager.Instance.CurrentRemove -= 1;

        Debug.Log("OnClickComplete");
    }
    public void OnClickFailure()
    {
        this.CloseObj();
        Debug.Log("OnClickFailure");
    }
    private void CloseObj()
    {
        this.objCtrl = null;
        gameObject.SetActive(false);
    }
}
