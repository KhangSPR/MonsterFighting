using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnPoints : SaiMonoBehaviour
{
    public List<LandIndexScript> points;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawnPoints();
    }

    // Tải tất cả các điểm spawn từ các thành phần con chứa LandIndexScript
    protected virtual void LoadSpawnPoints()
    {
        if (this.points.Count > 0) return;

        // Tìm tất cả các thành phần LandIndexScript trong các đối tượng con
        LandIndexScript[] foundPoints = GetComponentsInChildren<LandIndexScript>();
        this.points.AddRange(foundPoints);

        Debug.Log($"{gameObject.name}: LoadSpawnPoints {gameObject} with {points.Count} points.");
    }

    // Đặt lại điểm spawn thành trạng thái trống
    public virtual void ResetPointEmpty()
    {
        foreach (LandIndexScript point in points)
        {
            TileSpawn tileSpawn = point.GetComponent<TileSpawn>();
            if (tileSpawn != null && tileSpawn.IsEmpty)
            {
                tileSpawn.IsEmpty = false;
            }
        }
    }

    // Lấy một điểm spawn ngẫu nhiên từ các điểm không trống
    public virtual Transform GetRandomIsEmpty()
    {
        List<Transform> nonEmptyPoints = new List<Transform>();

        foreach (LandIndexScript point in points)
        {
            TileSpawn tileSpawn = point.GetComponent<TileSpawn>();
            if (tileSpawn != null && !tileSpawn.IsEmpty)
            {
                nonEmptyPoints.Add(point.transform);
            }
        }

        if (nonEmptyPoints.Count == 0)
        {
            Debug.LogWarning("No empty spawn points available.");
            return null;
        }

        int randIndex = Random.Range(0, nonEmptyPoints.Count);
        return nonEmptyPoints[randIndex];
    }

    // Lấy một điểm spawn ngẫu nhiên bất kỳ
    public virtual Transform GetRandom()
    {
        int rand = Random.Range(0, points.Count);
        return points[rand].transform;
    }
}
