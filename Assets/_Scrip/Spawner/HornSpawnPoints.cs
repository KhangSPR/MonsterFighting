using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSpawnPoints : BaseSpawnPoints
{
    public Transform GetPoints(int landIndex)
    {
        Transform selectPoints = points[0].transform;
        Vector3 position = selectPoints.position;

        float[,] yOffset = new float[,]
        {
            { 0f, -1f, -2f },     // landIndex == 0
            { 1f, 0f, -1f },      // landIndex == 1
            { 2f, 1f, 0f }        // landIndex == 2
        };

        int rand = Random.Range(0, 2);

        // Lấy component LandIndexScript từ selectedPoint và đặt LandIndex nếu tồn tại
        LandIndexScript landIndexScript = selectPoints.GetComponent<LandIndexScript>();
        if (landIndexScript != null)
        {
            landIndexScript.SetLandIndex(rand);
        }

        position.y += yOffset[landIndex, rand];
        selectPoints.position = position;
        //selectedPoint.position = Vector3.zero;

        Debug.Log("GetPoints: " + selectPoints.position + " | Land Index: " + rand);

        return selectPoints;
    }
    public void SwapPosition()
    {
        Vector3 newPosition = this.points[0].transform.position;
        newPosition.y = this.points[1].transform.position.y;
        this.points[0].transform.position = newPosition;

        Debug.Log("SwapPosition: " + this.points[0].transform.position);
    }

}
