using System;
using System.Collections.Generic;
using System.Linq;
using Brushes;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Features.Building.Scripts.Grid
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Tilemap _cursorTilemap;

        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private BuildableObject _currentSelectedBuilding;

        private Dictionary<BrushType, Brush> _brushes = new Dictionary<BrushType, Brush>();

        public bool IsEnabled => _cursorTilemap.gameObject.activeSelf;
        private Brush _currentBrush => _brushes[_currentSelectedBuilding.BrushType];

        private void Start()
        {
            _brushes = new Dictionary<BrushType, Brush>()
            {
                { BrushType.Single, new SingleBrush(_grid) },
                { BrushType.Outline, new OutlineBrush(_grid) },
                { BrushType.Multi, new MultiBrush(_grid) },
                { BrushType.Remove, new RemoveBrush(_grid) }
            };
            
            transform.localScale = Vector3.one * _grid.CellSize;
        }

        private Vector3Int _origin;

        /// <summary>
        /// Call on button down or holding
        /// </summary>
        /// <param name="position">Position of the pressed location</param>
        public void Press(Vector3Int position)
        {
            _currentBrush.Down(position);
        }
        
        /// <summary>
        /// Call on button release
        /// </summary>
        /// <param name="position">Position of the released location</param>
        public void Release(Vector3Int position)
        {
            _currentBrush.Release(position);
        }
        
        /// <summary>
        /// Called when not releasing or holding to show the current location
        /// </summary>
        /// <param name="position">Position of the hover</param>
        public void Hover(Vector3Int position)
        {
            _currentBrush.Hover(position);
        }
        
        /// <summary>
        /// Rotates the selected building
        /// </summary>
        /// <param name="direction"></param>
        public void Rotate(int direction)
        {
            _currentBrush.Rotate(direction);
        }
        
        /// <summary>
        /// Called when the cursor should be visualized
        /// </summary>
        public void UpdateVisuals()
        {
            Visualize(_currentBrush.SelectedTiles, _currentBrush.RequireAll);
        }

        private void Update()
        {
            //enable cursor when over UI
            _cursorTilemap.gameObject.SetActive(!_eventSystem.IsPointerOverGameObject());
        }
        
        /// <summary>
        /// Changes a world position to a clamped grid position
        /// </summary>
        /// <param name="worldPosition">Position to be clamped</param>
        /// <returns></returns>
        public Vector3Int WorldToGirdPosition(Vector3 worldPosition)
        {
            Vector3 pos = worldPosition;

            //clamp to grid positions
            Vector2 clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
                RoundToMultiple(pos.y, _grid.CellSize));

            return _grid.ClampedWorldToGridPosition(clampedValue,
                (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer);
        }

        /// <summary>
        /// Updates the cursor tilemap, so it visualizes the current selected tiles
        /// </summary>
        /// <param name="selectedGroup">The current selected tiles</param>
        /// <param name="requireAllAvailable">Sets that all need to be required if placed</param>
        /// <param name="flipColors">Flip colors to the correct and incorrect</param>
        private void Visualize(List<SubBuildItem> selectedGroup, bool requireAllAvailable = false,
            bool flipColors = false)
        {
            //sets the offset of the whole grid
            Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_cursorTilemap.transform.position.x),
                Mathf.RoundToInt(_cursorTilemap.transform.position.y),
                Mathf.RoundToInt(_cursorTilemap.transform.position.z));

            _cursorTilemap.ClearAllTiles();

            //check if all positions are required when placing
            bool valid = true;
            if (requireAllAvailable)
            {
                foreach (var gridPosition in selectedGroup)
                {
                    if (!_grid.IsEmpty(gridPosition.GridPosition))
                    {
                        valid = false;
                        break;
                    }
                }
            }

            Color validColor = !flipColors ? _validColor : _invalidColor;
            Color invalidColor = !flipColors ? _invalidColor : _validColor;

            //set the tile on the tilemap
            foreach (SubBuildItem gridPosition in selectedGroup)
            {
                TileChangeData tempTile = new TileChangeData()
                {
                    position = gridPosition.GridPosition - offset,
                    color = valid
                        ? (_grid.IsEmpty(gridPosition.GridPosition) ? validColor : invalidColor)
                        : invalidColor
                };

                tempTile.transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, _currentBrush.Rotation * -90));
                tempTile.tile = gridPosition.Tile;
                _cursorTilemap.SetTile(tempTile, true);
            }
        }

        /// <summary>
        /// Set other buildable
        /// </summary>
        /// <param name="buildableObject">Building that is going to be placed</param>
        public void ChangeSelectedBuildable(BuildableObject buildableObject)
        {
            _currentSelectedBuilding = buildableObject;
            _brushes[buildableObject.BrushType].Assign(buildableObject);
        }

        /// <summary>
        /// Disable the cursor tilemap
        /// </summary>
        public void DisableCursor()
        {
            _cursorTilemap.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Enable the cursor tilemap
        /// </summary>
        public void EnableCursor()
        {
            _cursorTilemap.gameObject.SetActive(true);
        }

        /// <summary>
        /// Used for clamping the world position to grid position
        /// </summary>
        /// <param name="value">Value to be changed</param>
        /// <param name="roundTo">closest number</param>
        /// <returns></returns>
        private float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }
    }
}