using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : EnemyAbstract
{
    public bool stopMoving = false;
    public List<Transform> detect = new List<Transform>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Enemy")
        {
            if (detect.Count == 0)
            {
                stopMoving = true;
                detect.Add(other.transform.parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Xóa đối tượng khỏi danh sách detect khi không còn tiếp xúc với trigger
        detect.Remove(other.transform.parent);
        stopMoving = false;
    }
}
