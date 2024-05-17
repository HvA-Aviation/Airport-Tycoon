using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Brushes
{
    public class DragBrush : Brush
    {
        public DragBrush(Action<Vector3Int, Tile> paintCallback) : base(paintCallback)
        {
        }
    }
}