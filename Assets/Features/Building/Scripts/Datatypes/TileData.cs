using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Datatypes
{
    [Serializable]
    public class TileData
    {
        [field: SerializeField] public Tile Tile { get; private set;  }
        public float WorkLoad;
        public bool UnTraversable = false;
    }
}