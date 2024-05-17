using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Brushes
{
    public class MultiBrush : Brush
    {
       
        public MultiBrush(Grid grid) : base(grid)
        {
        }
        
        /// <summary>
        /// Sets the selected tiles between the origin and current position
        /// </summary>
        /// <param name="position">Current position</param>
        protected override void Holding(Vector3Int position)
        {
            //gets selection
            //get the min and the max of the position
            var min = Vector3Int.Min(_origin, position);
            var max = Vector3Int.Max(_origin, position);

            _selectedTiles.Clear();

            //get all tiles between the min and the max position
            for (var x = min.x; x < max.x + 1; x++)
            {
                for (var y = min.y; y < max.y + 1; y++)
                {
                    _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                        new Vector3Int(x, y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
                }
            }
            
            base.Holding(position);
        }

        public override void Apply(Vector3Int position)
        {
            foreach (SubBuildItem item in _selectedTiles)
            {
                _grid.Set(item.GridPosition, _buildableObject.BuildItems[0].Tile);
            }
        }
    }
}