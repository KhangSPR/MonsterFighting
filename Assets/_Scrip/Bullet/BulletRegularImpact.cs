using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRegularImpact : BulletImpact
{
    // ... Các thành phần và phương thức của class

    public override BulletCtrl GetBulletCtrl()
    {
        // Trả về BulletCtrl cụ thể cho lớp con BulletRegularImpact
        return bulletRegularCtrl;
    }
}
