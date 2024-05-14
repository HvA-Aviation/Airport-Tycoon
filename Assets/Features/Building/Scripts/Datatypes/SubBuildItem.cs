using System;
using Features.Building.Scripts.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class SubBuildItem
    {
        public Tile Tile;
        public GridPosition GridPosition;
        
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
    }
}