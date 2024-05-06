using System;
using UnityEngine;

namespace Building.Datatypes
{
    [Serializable]
    public class GridPosition
    {
        public GridPosition(Vector2Int position, GridLayer layer)
        {
            Position = position;
            Layer = layer;
        }

        public Vector2Int Position;
        public GridLayer Layer;

        public static implicit operator Vector3Int(GridPosition gridPosition)
        {
            Vector3Int converted = (Vector3Int)gridPosition.Position;
            converted.z = (int)gridPosition.Layer;
            return converted;
        }

        public static GridPosition operator +(Vector3Int vector3Int, GridPosition gridPosition)
        {
            return new GridPosition(
                new Vector2Int(vector3Int.x + gridPosition.Position.x, vector3Int.y + gridPosition.Position.y),
                gridPosition.Layer);
        }

        public static GridPosition operator +(GridPosition gridPosition, Vector3Int vector3Int)
        {
            return new GridPosition(
                new Vector2Int(gridPosition.Position.x + vector3Int.x, gridPosition.Position.y + vector3Int.y),
                gridPosition.Layer);
        }
    }
}