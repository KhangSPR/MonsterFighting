using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyPoints : SaiMonoBehaviour
{
    public List<Transform> points;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadSpawnPoints();
    }
    protected virtual void loadSpawnPoints()
    {
        if (this.points.Count > 0) return;
        foreach (Transform obj in transform)
        {
            this.points.Add(obj);
        }
        Debug.Log(gameObject.name + ": loadSpawnPoints" + gameObject);
    }
    public virtual Transform GetRandomIsEmpty()
    {
        List<Transform> nonEmptyPoints = new List<Transform>();
        foreach (Transform point in points)
        {
            nonEmptyPoints.Add(point);
        }

        // Chọn một điểm spawn ngẫu nhiên từ danh sách đã lọc
        int randIndex = Random.Range(0, nonEmptyPoints.Count);

        Debug.Log("TranSpawn: " + randIndex);

        return nonEmptyPoints[randIndex];
    }
    public Transform GetRandom()
    {
        int rand = Random.Range(0, points.Count);
        return points[rand];
    }
}
