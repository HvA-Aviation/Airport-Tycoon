using System;
using Features.Building.Scripts.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class SubBuildItem
    {
        [field: SerializeField]public Tile Tile { get; private set; }
        [field: SerializeField]public GridPosition GridPosition { get; private set; }

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