using System;
using UnityEngine;

namespace Building
{
    [Serializable]
    public class CellData
    {
        public CellData(int tile, int rotation)
        {
            Tile = tile;
            Rotation = rotation;
        }

        public int Tile;
        public int Rotation;

        public static CellData empty => new CellData(-1, 0);
    }
}