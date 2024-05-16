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

        private List<SubBuildItem> _selectedGroup = new List<SubBuildItem>();
        private List<SubBuildItem> _shape = new List<SubBuildItem>();
        private Vector3Int _origin;
        private int _rotation = 0;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private BuildableObject _currentSelectedBuilding;

        public bool IsEnabled => _cursorTilemap.gameObject.activeSelf;
        public BrushType BrushType => _currentSelectedBuilding.BrushType;

        private void Start()
        {
            transform.localScale = Vector3.one * _grid.CellSize;
        }

        private void Update()
        {
            //enable cursor when over UI
            _cursorTilemap.gameObject.SetActive(!_eventSystem.IsPointerOverGameObject());
        }

        public void Rotate(int direction)
        {
            if (_currentSelectedBuilding.BrushType != BrushType.Single)
                return;

            if (direction != 0)
            {
                Vector2Int dir = Vector2Int.one;

                if (direction == 1)
                {
                    dir.y = -1;
                    _rotation++;
                }
                else
                {
                    dir.x = -1;
                    _rotation--;
                }

                //rotate the shape clockwise
                for (int i = 0; i < _shape.Count; i++)
                {
                    var gridPosition = _shape[i].GridPosition.Position;
                    _shape[i].GridPosition.Position = new Vector2Int(gridPosition.y, gridPosition.x) * dir;
                }

                if (_rotation > 3)
                {
                    _rotation = 0;
                }
                else if (_rotation < 0)
                {
                    _rotation = 3;
                }
            }
        }

        #region brushes

        public void MultiBrushSelect(Vector3Int position, int mouseButton)
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                //starts selection and sets the origin of the selection
                _origin = position;
            }
            else if (Input.GetMouseButtonUp(mouseButton))
            {
                //place or remove. This ends the selection
                if (mouseButton == 0)
                {
                    foreach (var selected in _selectedGroup)
                    {
                        _grid.Set(selected.GridPosition, _currentSelectedBuilding.BuildItems[0].Tile);
                    }
                }
                else
                {
                    foreach (var selected in _selectedGroup)
                    {
                        _grid.Remove(selected.GridPosition);
                    }
                }

                //resets the selection 
                _selectedGroup.Clear();
                _cursorTilemap.ClearAllTiles();
            }
            else if (Input.GetMouseButton(mouseButton) && _selectedGroup.Count > 0)
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
                        currentSelectedGroup.Add(new SubBuildItem(_currentSelectedBuilding.BuildItems[0].Tile,
                            new Vector3Int(x, y, (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer)));
                    }
                }

                Hover(Vector3Int.zero, currentSelectedGroup, flipColors: mouseButton == 1);
            }
            else
            {
                //hover in the default shape
                Hover(position, _shape, flipColors: mouseButton == 1);
            }
        }

        public void OutlineBrushSelect(Vector3Int position)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _origin = position;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //place or remove. This ends the selection
                foreach (var selected in _selectedGroup)
                {
                    _grid.Set(selected.GridPosition, _currentSelectedBuilding.BuildItems[0].Tile);
                }

                //resets the selection 
                _selectedGroup = new List<SubBuildItem>();
                _cursorTilemap.ClearAllTiles();
            }
            else if (Input.GetMouseButton(0) && _selectedGroup.Count > 0)
            {
                //gets selection
                //get the min and the max of the position
                Vector3Int min = Vector3Int.Min(_origin, position);
                Vector3Int max = Vector3Int.Max(_origin, position);

                List<SubBuildItem> currentSelectedGroup = new List<SubBuildItem>();

                //get all tiles between the min and the max position
                for (int x = min.x; x < max.x + 1; x++)
                {
                    currentSelectedGroup.Add(new SubBuildItem(_currentSelectedBuilding.BuildItems[0].Tile,
                        new Vector3Int(x, min.y, (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer)));
                    currentSelectedGroup.Add(new SubBuildItem(_currentSelectedBuilding.BuildItems[0].Tile,
                        new Vector3Int(x, max.y, (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer)));
                }

                for (int y = min.y; y < max.y + 1; y++)
                {
                    currentSelectedGroup.Add(new SubBuildItem(_currentSelectedBuilding.BuildItems[0].Tile,
                        new Vector3Int(min.x, y, (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer)));
                    currentSelectedGroup.Add(new SubBuildItem(_currentSelectedBuilding.BuildItems[0].Tile,
                        new Vector3Int(max.x, y, (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer)));
                }

                Hover(Vector3Int.zero, currentSelectedGroup);
            }
            else
            {
                //hover in the default shape
                Hover(position, _shape);
            }
        }

        public void SingleBrush(Vector3Int position)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //place the tiles in the shape of the selection
                List<Vector3Int> positions = new List<Vector3Int>();
                List<Tile> indices = new List<Tile>();
                foreach (var selected in _selectedGroup)
                {
                    positions.Add(selected.GridPosition);
                    indices.Add(selected.Tile);
                }

                _grid.SetGroup(positions, indices, _rotation);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //remove the tiles in the shape of the selection
                _grid.Remove(position);
            }
            else
            {
                //hover and require all spaces to be free
                Hover(position, _shape, true);
            }
        }

        public Vector3Int WorldToGirdPosition(Vector3 worldPosition)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //clamp to grid positions
            Vector2 clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
                RoundToMultiple(pos.y, _grid.CellSize));

            return _grid.ClampedWorldToGridPosition(clampedValue,
                (int)_currentSelectedBuilding.BuildItems[0].GridPosition.Layer);
        }

        /// <summary>
        /// Creates a tile on the positions the mouse is hold down
        /// </summary>
        /// <param name="position"></param>
        public void DragBrush(Vector3Int position)
        {
            Hover(position, _shape);

            if (Input.GetMouseButton(0))
            {
                _grid.Set(position, _currentSelectedBuilding.BuildItems[0].Tile);
            }
            else if (Input.GetMouseButton(1))
            {
                //remove the tiles in the shape of the selection
                _grid.Remove(position);
            }
        }

        #endregion

        private void Hover(Vector3Int position, List<SubBuildItem> shapes, bool requireAllAvailable = false,
            bool flipColors = false)
        {
            //sets the offset of the whole grid
            Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_cursorTilemap.transform.position.x),
                Mathf.RoundToInt(_cursorTilemap.transform.position.y),
                Mathf.RoundToInt(_cursorTilemap.transform.position.z));

            //get the current selection of tiles
            List<SubBuildItem> currentSelectedGroup = new List<SubBuildItem>();
            foreach (var shape in shapes)
            {
                currentSelectedGroup.Add(new SubBuildItem(shape.Tile, position + shape.GridPosition));
            }

            //remove the tiles that were in the previous selection, but not in the current
            foreach (var gridPosition in _selectedGroup)
            {
                if (!currentSelectedGroup.Any(x => x.GridPosition == gridPosition.GridPosition))
                {
                    _cursorTilemap.SetTile(gridPosition.GridPosition - offset, null);
                }
            }

            //check if all positions are required when placing
            bool valid = true;
            if (requireAllAvailable)
            {
                foreach (var gridPosition in currentSelectedGroup)
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
            foreach (SubBuildItem gridPosition in currentSelectedGroup)
            {                
                TileChangeData tempTile = new TileChangeData()
                {
                    position = gridPosition.GridPosition - offset,
                    color = valid
                        ? (_grid.IsEmpty(gridPosition.GridPosition) ? validColor : invalidColor)
                        : invalidColor
                };

                tempTile.transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, _rotation * -90));
                tempTile.tile = gridPosition.Tile;
                _cursorTilemap.SetTile(tempTile, true);
            }

            _selectedGroup = currentSelectedGroup;
        }

        public void ChangeSelectedBuildable(BuildableObject buildableObject)
        {
            _rotation = 0;
            _currentSelectedBuilding = buildableObject;

            //Create a fresh shape so the rotation is correct and doesn't change the prefab
            _shape.Clear();
            foreach (var item in buildableObject.BuildItems)
            {
                _shape.Add(new SubBuildItem(item.Tile,
                    new GridPosition(item.GridPosition.Position, item.GridPosition.Layer)));
            }
        }

        public void DisableCursor()
        {
            _cursorTilemap.gameObject.SetActive(false);
        }

        public void EnableCursor()
        {
            _cursorTilemap.gameObject.SetActive(true);
        }

        public float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }
    }
}