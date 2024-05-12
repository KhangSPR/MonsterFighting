using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplodeImpact : BulletImpact
{
    // ... Các thành phần và phương thức của class

    public override BulletCtrl GetBulletCtrl()
    {
        // Trả về BulletCtrl cụ thể cho lớp con BulletExplodeImpact
        return bulletExplodeCtrl; 
    }

}
