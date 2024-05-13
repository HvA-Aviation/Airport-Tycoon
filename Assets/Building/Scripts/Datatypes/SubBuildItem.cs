using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building.Datatypes
{
    [Serializable]
    public class SubBuildItem
    {
        public SubBuildItem(Tile tile, GridPosition gridPosition)
        {
            Tile = tile;
            GridPosition = gridPosition;
        }
        
        public SubBuildItem(Tile tile, Vector3Int gridPosition)
        {
            Tile = tile;
            GridPosition = new GridPosition(new Vector2Int(gridPosition.x, gridPosition.y), (GridLayer)gridPosition.z);
        }

        public Tile Tile;
        public GridPosition GridPosition;
    }
}