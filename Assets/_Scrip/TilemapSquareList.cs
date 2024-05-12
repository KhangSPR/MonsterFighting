using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSquareList : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Vector2> tileSquareCenters = new List<Vector2>();
    public List<GameObject> objectList = new List<GameObject>();
    public GameObject objectPrefab;
    public int objectCount = 0; // start object count at 0

    void Start()
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                Vector2 center = GetTilemapSquareCenter(tilemap, pos);
                if (!tileSquareCenters.Contains(center)) // check if center is not already in the list
                {
                    tileSquareCenters.Add(center); // add the center to the list
                    objectCount++;
                    string objectName = "Door_" + objectCount.ToString(""); // creates string "Object0001", "Object0002", etc.

                    // create new object with formatted name
                    GameObject newObject = Instantiate(objectPrefab, center, Quaternion.identity);
                    newObject.name = objectName; // set object name
                    objectList.Add(newObject); // add the new object to the list

                }
            }
        }
    }

    Vector2 GetTilemapSquareCenter(Tilemap tilemap, Vector3Int pos)
    {
        Vector2 min = tilemap.CellToWorld(pos);
        Vector2 max = min + (Vector2)tilemap.cellSize;
        float size = Mathf.Max(max.x - min.x, max.y - min.y);
        Vector2 center = (max + min) / 2f;

        return center;
    }
}