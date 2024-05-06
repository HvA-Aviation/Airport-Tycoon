using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building.Datatypes
{
    [Serializable]
    public class SubBuildItem
    {
        public Tile Tile;
        public GridPosition GridPosition;
    }
}