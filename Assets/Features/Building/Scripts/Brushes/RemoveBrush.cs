using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.Building.Scripts.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Brushes
{
    public class RemoveBrush : MultiBrush
    {
        public RemoveBrush(Grid grid) : base(grid)
        {
        }

        /// <summary>
        /// Removes from all layers
        /// </summary>
        /// <param name="position">Current position</param>
        public override void Apply(Vector3Int position)
        {
            foreach (SubBuildItem item in _selectedTiles)
            {
                _grid.Remove(item.GridPosition);
            }
        }
    }
}