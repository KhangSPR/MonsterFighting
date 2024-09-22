using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class SpawnPoints : SaiMonoBehaviour
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
    public void ResetPointEmpty()
    {
        List<Transform> nonEmptyPoints = new List<Transform>();
        foreach (Transform point in points)
        {
            TileSpawn tileSpawn = point.GetComponent<TileSpawn>();
            if(tileSpawn.IsEmpty)
            {
                tileSpawn.IsEmpty = false;
            }
        }
    }
    public virtual Transform GetRandomIsEmpty()
    {
        List<Transform> nonEmptyPoints = new List<Transform>();
        foreach (Transform point in points)
        {
            TileSpawn tileSpawn = point.GetComponent<TileSpawn>();
            if (tileSpawn != null && !tileSpawn.IsEmpty)
            {
                nonEmptyPoints.Add(point);

            }
        }

        // Chọn một điểm spawn ngẫu nhiên từ danh sách đã lọc
        int randIndex = Random.Range(0, nonEmptyPoints.Count);
        return nonEmptyPoints[randIndex];
    }
    public Transform GetRandom()
    {
        int rand = Random.Range(0, points.Count);
        return points[rand];
    }

}
