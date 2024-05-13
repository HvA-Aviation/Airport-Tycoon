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
        //TODO change to workload
        public float BuildPercentage = .4f;

        public bool Build(float speed)
        {
            BuildPercentage = Mathf.Clamp(BuildPercentage + speed, 0, 1);

            return BuildPercentage == 1;
        }

        public static CellData empty => new CellData(-1, 0);
    }
}