using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Brushes
{
    public class MultiBrush : Brush
    {
        public override void Holding(Vector3Int position)
        {
            //gets selection
            //get the min and the max of the position
            var min = Vector3Int.Min(_origin, position);
            var max = Vector3Int.Max(_origin, position);

            List<SubBuildItem> currentSelectedGroup = new List<SubBuildItem>();

            //get all tiles between the min and the max position
            for (var x = min.x; x < max.x + 1; x++)
            {
                for (var y = min.y; y < max.y + 1; y++)
                {
                    currentSelectedGroup.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                        new Vector3Int(x, y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
                }
            }

            _selectedTiles = currentSelectedGroup;
            
            base.Holding(position);
        }

        public override void Release(Vector3Int position)
        {
            foreach (SubBuildItem item in _selectedTiles)
            {
                _paintCallback.Invoke(item.GridPosition, _buildableObject.BuildItems[0].Tile);
            }
            
            base.Release(position);
        }

        public MultiBrush(Action<Vector3Int, Tile> paintCallback) : base(paintCallback)
        {
        }
    }
}