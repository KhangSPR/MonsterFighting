using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtension
{
    public static List<Vector3Int> GetUsedCells(this Tilemap tilemap)
    {
        List<Vector3Int> usedCells = new List<Vector3Int>();

        // Loop through all tiles on the tilemap
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            var tile = tilemap.GetTile(pos);
            // If the tile is not null, it is used
            if (tile != null)
            {
                usedCells.Add(pos);
            }
        }

        return usedCells;
    }
}