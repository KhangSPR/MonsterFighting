using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHolderCondition : SaiMonoBehaviour
{
    [SerializeField] protected List<Transform> startHolderConition;
    public List<Transform> StartHolderConition => startHolderConition;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadStarHolderCondition();

    }
    protected void LoadStarHolderCondition()
    {
        // Nếu danh sách đã được gán rồi thì không cần làm gì thêm
        if (StartHolderConition != null && StartHolderConition.Count > 0) return;

        // Xóa danh sách cũ nếu có dữ liệu
        if (startHolderConition == null)
        {
            startHolderConition = new List<Transform>();
        }
        else
        {
            startHolderConition.Clear();
        }

        // Thêm tất cả các đối tượng con vào danh sách
        foreach (Transform child in transform)
        {
            startHolderConition.Add(child);
        }
    }
}
