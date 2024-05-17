using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
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
                for (int i = 0; i < 2; i++)
                {
                    _grid.Remove(new Vector3Int(item.GridPosition.Position.x, item.GridPosition.Position.y, i));
                }
            }
        }
    }
}