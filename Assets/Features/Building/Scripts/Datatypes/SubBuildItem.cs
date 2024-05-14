using System;
using Features.Building.Scripts.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class SubBuildItem
    {
        [SerializeField] private Tile _tile;
        [SerializeField] private GridPosition _gridPosition;

        public Tile Tile => _tile;

        public GridPosition GridPosition => _gridPosition;

        public SubBuildItem(Tile tile, GridPosition gridPosition)
        {
            _tile = tile;
            _gridPosition = gridPosition;
        }
        
        public SubBuildItem(Tile tile, Vector3Int gridPosition)
        {
            _tile = tile;
            _gridPosition = new GridPosition(new Vector2Int(gridPosition.x, gridPosition.y), (GridLayer)gridPosition.z);
        }
    }
}