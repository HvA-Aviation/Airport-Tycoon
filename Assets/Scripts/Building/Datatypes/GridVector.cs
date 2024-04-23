using System;

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
    }
}