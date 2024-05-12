//using UnityEngine;

//public class ObjLookAtBullet : SaiMonoBehaviour
//{

//    //private void Start()
//    //{
//    //    // Đăng ký lắng nghe sự kiện
//    //    ObjLookAtDistanceGuardEvents.TransformChanged += OnTransformChanged;
//    //}

//    //private void OnDestroy()
//    //{
//    //    // Huỷ đăng ký lắng nghe sự kiện khi không còn cần thiết
//    //    ObjLookAtDistanceGuardEvents.TransformChanged -= OnTransformChanged;
//    //}

//    //private void OnTransformChanged(object sender, TransformChangedEventArgs e)
//    //{
//    //    // Xử lý sự thay đổi của transform ở đây
//    //    Transform newTransform = e.NewTransform;

//    //    // Do something with the new transform...
//    //}
//    protected override void Update()
//    {
//        base.Update();
//        getTagetPosition();

//    }

//    protected virtual void getTagetPosition()
//    {
//        if (ObjLookAtDistanceGuard.Instance.enemy != null)
//        {
//            Vector3 direction = ObjLookAtDistanceGuard.Instance.enemy.position - transform.parent.position;
//            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
//            transform.parent.rotation = Quaternion.Euler(0, 0, rot + 180);
//        }
//        return;
//    }
//}
