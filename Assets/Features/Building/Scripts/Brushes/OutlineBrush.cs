using System;
using System.Collections.Generic;
using System.Linq;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Brushes
{
    public class OutlineBrush : Brush
    {
        public OutlineBrush(Grid grid) : base(grid)
        {
        }
        
        /// <summary>
        /// Sets the selected tiles as outline between the origin and current position
        /// </summary>
        /// <param name="position">Current position</param>
        protected override void Holding(Vector3Int position)
        {
            //gets selection
            //get the min and the max of the position
            Vector3Int min = Vector3Int.Min(_origin, position);
            Vector3Int max = Vector3Int.Max(_origin, position);

            _selectedTiles.Clear();

            //get all tiles between the min and the max position
            for (int x = min.x; x < max.x + 1; x++)
            {
                _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                    new Vector3Int(x, min.y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
                _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                    new Vector3Int(x, max.y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
            }

            for (int y = min.y; y < max.y + 1; y++)
            {
                _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                    new Vector3Int(min.x, y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
                _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                    new Vector3Int(max.x, y, (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
            }
            
            base.Holding(position);
        }

        /// <summary>
        /// Place all the selected positions
        /// </summary>
        /// <param name="position">current position</param>
        public override void Apply(Vector3Int position)
        {
            int price = GameManager.Instance.BuildingManager.CurrentBuildableObject.Price;
            
            foreach (SubBuildItem item in _selectedTiles.Distinct())
            {
                if (GameManager.Instance.FinanceManager.Balance.Value - price < 0)
                    return;
                
                if (_grid.Set(item.GridPosition, _buildableObject.BuildItems[0].Tile))
                    GameManager.Instance.FinanceManager.Balance.Subtract(price);
            }
        }
    }
}