using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Brushes
{
    public class SingleBrush : Brush
    {
        public SingleBrush(Action<Vector3Int, Tile> paintCallback) : base(paintCallback)
        {
        }
    }
}