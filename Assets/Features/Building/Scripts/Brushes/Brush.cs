using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Brushes
{
    public abstract class Brush
    {
        protected List<SubBuildItem> _selectedTiles = new List<SubBuildItem>();
        protected Vector3Int _origin;
        private bool _holding;
        protected BuildableObject _buildableObject;
        protected Action<Vector3Int, Tile> _paintCallback;

        public List<SubBuildItem> SelectedTiles => _selectedTiles;

        public Brush(Action<Vector3Int, Tile> paintCallback)
        {
            _paintCallback = paintCallback;
        }

        public void Assign(BuildableObject buildableObject)
        {
            _selectedTiles = new List<SubBuildItem>();
            _buildableObject = buildableObject;
        }

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

        public void Hover(Vector3Int position)
        {
            _selectedTiles.Clear();
            //_selectedTiles.Add(new SubBuildItem(_buildableObject.BuildItems));
        }

        public virtual void Press(Vector3Int position)
        {
        }

        public virtual void Holding(Vector3Int position)
        {
        }

        public virtual void Release(Vector3Int position)
        {
            _selectedTiles.Clear();
            _holding = false;
        }
    }
}