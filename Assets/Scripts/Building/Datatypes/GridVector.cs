using System;
using UnityEngine;

namespace Building.Datatypes
{
    /// <summary>
    /// Used so everything is an interger and we don't have to use any conversions from float to int 
    /// </summary>
    [Serializable]
    public class GridVector
    {
        public int x;
        public int y;
        public int z;

        public GridVector(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}