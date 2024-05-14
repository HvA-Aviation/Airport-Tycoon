using System;
using UnityEngine;

namespace Features.Building.Scripts.Grid
{
    [Serializable]
    public struct CellData
    {
        public int Tile;
        public int Rotation;
        public float BuildPercentage;
        
        public static CellData empty => new CellData(-1, 0);
        
        public CellData(int tile, int rotation, float buildPercentage = 0.4f)
        {
            Tile = tile;
            Rotation = rotation;
            BuildPercentage = buildPercentage;
        }
    }
}