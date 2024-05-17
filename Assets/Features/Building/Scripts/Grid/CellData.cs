using System;
using UnityEngine;

namespace Features.Building.Scripts.Grid
{
    [Serializable]
    public struct CellData
    {
        public int Tile;
        public int Rotation;
        public float WorkLoad;
        public float CurrentWorkLoad;
        
        public static CellData empty => new CellData(-1, 0, 0);
        
        public CellData(int tile, int rotation, float workLoad)
        {
            Tile = tile;
            Rotation = rotation;
            WorkLoad = workLoad;
            CurrentWorkLoad = 0;
        }

        /// <summary>
        /// Set the tile to an empty tile
        /// </summary>
        public void Clear()
        {
            Tile = -1;
            Rotation = 0;
            WorkLoad = 0;
            CurrentWorkLoad = 0;
        }
    }
}