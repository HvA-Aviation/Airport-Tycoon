using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    public class TileUpdateData : EventArgs
    {
        public Vector3Int Position;
        public Color Color;
        public Tile Tile;
        public Matrix4x4 Transform;

        public static implicit operator TileChangeData(TileUpdateData t)
        {
            return new TileChangeData(t.Position, t.Tile, t.Color, t.Transform);
        }
    }
}