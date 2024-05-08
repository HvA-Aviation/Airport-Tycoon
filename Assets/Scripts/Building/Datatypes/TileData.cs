using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building.Datatypes
{
    [Serializable]
    public class TileData
    {
        public Tile Tile;
        public bool UnTraversable = false;
        //TODO different tile for rotation list
    }
}