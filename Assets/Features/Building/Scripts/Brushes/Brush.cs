using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Brushes
{
    public abstract class Brush
    {
        protected List<SubBuildItem> _selectedTiles = new List<SubBuildItem>();
        protected Vector3Int _origin;
        private bool _holding;
        protected BuildableObject _buildableObject;
        protected Grid _grid;
        public int Rotation { get; protected set; }

        public bool RequireAll { get; protected set; }

        public List<SubBuildItem> SelectedTiles => _selectedTiles;

        public Brush(Grid grid)
        {
            _grid = grid;
        }

        /// <summary>
        /// Assign buildable and resets values
        /// </summary>
        /// <param name="buildableObject">Building to be build</param>
        public virtual void Assign(BuildableObject buildableObject)
        {
            _selectedTiles.Clear();
            _buildableObject = buildableObject;
            Rotation = 0;
        }

        /// <summary>
        /// Sets a virtual rotate for specific brushes
        /// </summary>
        /// <param name="direction">rotation direction</param>
        public virtual void Rotate(int direction)
        {
        }

        /// <summary>
        /// Button down, but when holding call holding
        /// </summary>
        /// <param name="position">Press location</param>
        public void Down(Vector3Int position)
        {
            if (!_holding)
            {
                _origin = position;
                _holding = true;

                Press(position);
            }
            else
            {
                Holding(position);
            }
        }

        /// <summary>
        /// Sets the method for holding down a button
        /// </summary>
        /// <param name="position">Holding position</param>
        protected virtual void Holding(Vector3Int position)
        {
        }

        /// <summary>
        /// Sets the method for Press down a button
        /// </summary>
        /// <param name="position">Press position</param>
        protected virtual void Press(Vector3Int position)
        {
        }

        /// <summary>
        /// Resets the current hover location to the current
        /// </summary>
        /// <param name="position">Current hover position</param>
        public virtual void Hover(Vector3Int position)
        {
            _selectedTiles.Clear();
            _selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems[0].Tile, position));
        }


        /// <summary>
        /// Resets holding and selected tiles when button is released
        /// </summary>
        /// <param name="position">release position</param>
        public virtual void Release(Vector3Int position)
        {
            _selectedTiles.Clear();
            _holding = false;
        }
    }
}