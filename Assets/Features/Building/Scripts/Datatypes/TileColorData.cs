using System;
using UnityEngine;

namespace Features.Building.Scripts.Datatypes
{
    public class TileColorData : EventArgs
    {
        public Vector3Int Position;
        public float Progress;
    }
}