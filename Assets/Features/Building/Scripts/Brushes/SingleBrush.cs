using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.Building.Scripts.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Brushes
{
    public class SingleBrush : Brush
    {
        private List<SubBuildItem> _shape;

        public SingleBrush(Grid grid) : base(grid)
        {
            RequireAll = true;
        }

        /// <summary>
        /// Assign the shape of the building
        /// </summary>
        /// <param name="buildableObject">Building to be build</param>
        public override void Assign(BuildableObject buildableObject)
        {
            base.Assign(buildableObject);
            _shape = new List<SubBuildItem>();
            foreach (SubBuildItem subBuildItem in _buildableObject.BuildItems)
            {
                GridPosition position =
                    new GridPosition(subBuildItem.GridPosition.Position, subBuildItem.GridPosition.Layer);

                _shape.Add(new SubBuildItem(subBuildItem.Tile, position));
            }
        }

        /// <summary>
        /// Shows the selected tiles in the correct shape
        /// </summary>
        /// <param name="position">Current position</param>
        public override void Hover(Vector3Int position)
        {
            _selectedTiles.Clear();
            foreach (SubBuildItem buildItem in _shape)
            {
                _selectedTiles.Add(new SubBuildItem(buildItem.Tile, position + buildItem.GridPosition));
            }
        }

        /// <summary>
        /// Rotate in direction and change the shape of the building
        /// </summary>
        /// <param name="direction">rotation direction</param>
        public override void Rotate(int direction)
        {
            if (direction != 0)
            {
                Vector2Int dir = Vector2Int.one;

                if (direction == 1)
                {
                    dir.y = -1;
                    Rotation++;
                }
                else
                {
                    dir.x = -1;
                    Rotation--;
                }

                //rotate the shape
                for (int i = 0; i < _shape.Count; i++)
                {
                    var gridPosition = _shape[i].GridPosition.Position;
                    _shape[i].GridPosition.Position = new Vector2Int(gridPosition.y, gridPosition.x) * dir;
                }

                Rotation %= 4;
            }
        }

        /// <summary>
        /// Place all the selected positions and link them
        /// </summary>
        /// <param name="position">current position</param>
        public override void Apply(Vector3Int position)
        {
            //place the tiles in the shape of the selection
            List<Vector3Int> positions = new List<Vector3Int>();
            List<Tile> indices = new List<Tile>();
            foreach (var selected in _selectedTiles)
            {
                positions.Add(selected.GridPosition);
                indices.Add(selected.Tile);
            }

            _grid.SetGroup(positions, indices, Rotation);
        }
    }
}