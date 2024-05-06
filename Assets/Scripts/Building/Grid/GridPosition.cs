using System;
using UnityEngine;

namespace Building.Datatypes
{
    [Serializable]
    public class GridPosition
    {
        public Vector2Int Position;
        public GridLayer Layer;
        
        public static implicit operator Vector3Int(GridPosition gridPosition)
        {
            Vector3Int converted = (Vector3Int)gridPosition.Position;
            converted.z = (int)gridPosition.Layer;
            return converted;
        }
    }
}