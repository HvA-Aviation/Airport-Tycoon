using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes.TileData
{
    [Serializable]
    public class BaseTile
    {
        public Tile Tile;
        public Color Color = Color.white;
        public float BuildLoad;
        public bool UnTraversable;
    }
}