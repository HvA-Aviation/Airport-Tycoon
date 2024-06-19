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
    public class LineBrush : Brush
    {
        public LineBrush(Grid grid) : base(grid)
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

            bool isXAxis = Mathf.Abs(_origin.x - position.x) > Mathf.Abs(_origin.y - position.y);

            Vector2Int axis = isXAxis
                ? new Vector2Int(min.x, max.x)
                : new Vector2Int(min.y, max.y);

            //get all tiles between the min and the max position
            for (int a = axis.x; a < axis.y + 1; a++)
            {
                _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile,
                    new Vector3Int(isXAxis ? a : _origin.x, isXAxis ? _origin.y : a,
                        (int)_buildableObject.BuildItems[0].GridPosition.Layer)));
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

            foreach (SubBuildItem item in _selectedTiles)
            {
                if (GameManager.Instance.FinanceManager.Balance.Value - price < 0)
                    return;

                if (_grid.Set(item.GridPosition, _buildableObject.BuildItems[0].Tile))
                    GameManager.Instance.FinanceManager.Balance.Subtract(price);
            }
        }
    }
}