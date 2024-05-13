using System;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class TileData
    {
        public Tile Tile;
        public bool UnTraversable = false;
        //TODO different tile for rotation list
    }
}