using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DetectPlayer : PlayerAbstract

{
    public bool stopMoving = false;
    public List<Transform> detect = new List<Transform>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Tower")
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
