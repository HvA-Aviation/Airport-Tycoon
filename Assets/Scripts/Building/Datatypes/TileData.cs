using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building.Datatypes
{
    [Serializable]
    public class TileData
    {
        public Tile Tile;
        public bool Traversable = true;
        //TODO different tile for rotation list
    }
}