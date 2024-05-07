using System;
using System.Collections.Generic;
using System.Linq;
using Building.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building
{
    public class Cursor : MonoBehaviour
    {
        private Grid _grid;
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private List<SubBuildItem> _selectedGroup = new List<SubBuildItem>();
        [SerializeField] private List<SubBuildItem> _shape = new List<SubBuildItem>();
        [SerializeField] private Vector3Int _origin;
        [SerializeField] private int _rotation = 0;

        [SerializeField] private BuildableObject _selectedBuilding;
        [SerializeField] private List<BuildableObject> _buildings;

        private int _currentMouse;

        private void Start()
        {
            _grid = FindObjectOfType<Grid>();

            transform.localScale = Vector3.one * _grid.CellSize;
        }

        private void Update()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
                RoundToMultiple(pos.y, _grid.CellSize));

            var position = _grid.ClampedWorldToGridPosition(clampedValue, (int)_selectedBuilding.BuildItems[0].GridPosition.Layer);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeToBuilding(_buildings[0]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeToBuilding(_buildings[1]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeToBuilding(_buildings[2]);
            }

            if (_selectedBuilding.BrushType == BrushType.Multi)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    _currentMouse = Input.GetMouseButtonDown(0) ? 0 : 1;
                    _origin = position;
                }
                else if (Input.GetMouseButtonUp(_currentMouse))
                {
                    if (_currentMouse == 0)
                    {
                        foreach (var selected in _selectedGroup)
                        {
                            _grid.Set(selected.GridPosition, _selectedBuilding.BuildItems[0].Tile);
                        }
                    }
                    else
                    {
                        foreach (var selected in _selectedGroup)
                        {
                            _grid.Remove(selected.GridPosition);
                        }
                    }

                    _selectedGroup = new List<SubBuildItem>();
                    _tilemap.ClearAllTiles();
                }
                else if (Input.GetMouseButton(_currentMouse) && _selectedGroup.Count > 0)
                {
                    var min = Vector3Int.Min(_origin, position);
                    var max = Vector3Int.Max(_origin, position);

                    List<SubBuildItem> currentSelectedGroup = new List<SubBuildItem>();

                    for (var x = min.x; x < max.x + 1; x++)
                    {
                        for (var y = min.y; y < max.y + 1; y++)
                        {
                            //new SubBuildItem(_selectedBuilding.Tile, position + shape.GridPosition)
                            currentSelectedGroup.Add(new SubBuildItem(_selectedBuilding.BuildItems[0].Tile,
                                new Vector3Int(x, y, (int)_selectedBuilding.BuildItems[0].GridPosition.Layer)));
                        }
                    }

                    Hover(Vector3Int.zero, currentSelectedGroup);
                }
                else
                {
                    Hover(position, _shape);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    for (int i = 0; i < _shape.Count; i++)
                    {
                        var gridPosition = _shape[i].GridPosition.Position;
                        _shape[i].GridPosition.Position = new Vector2Int(gridPosition.y, -gridPosition.x);
                    }

                    _rotation++;

                    if (_rotation > 3)
                    {
                        _rotation = 0;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
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
                    _grid.Remove(position);
                }
                else
                {
                    Hover(position, _shape, true);
                }
            }
        }

        private void Hover(Vector3Int position, List<SubBuildItem> shapes, bool requireAllAvailable = false)
        {
            //Sets the offset of the whole grid
            Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                Mathf.RoundToInt(_tilemap.transform.position.y),
                Mathf.RoundToInt(_tilemap.transform.position.z));

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
                    _tilemap.SetTile(gridPosition.GridPosition - offset, null);
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

            //set the tile on the tilemap
            foreach (var gridPosition in currentSelectedGroup)
            {
                TileChangeData tempTile = new TileChangeData()
                {
                    position = gridPosition.GridPosition - offset,
                    color = valid
                        ? (_grid.IsEmpty(gridPosition.GridPosition) ? _validColor : _invalidColor)
                        : _invalidColor
                };

                tempTile.transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, _rotation * -90));
                tempTile.tile = gridPosition.Tile;
                _tilemap.SetTile(tempTile, true);
            }

            _selectedGroup = currentSelectedGroup;
        }

        public void ChangeToBuilding(BuildableObject buildableObject)
        {
            _rotation = 0;
            _selectedBuilding = buildableObject;

            //Create a fresh shape so the rotation is correct and doesn't change the prefab
            _shape = new List<SubBuildItem>();
            foreach (var item in buildableObject.BuildItems)
            {
                _shape.Add(new SubBuildItem(item.Tile, new GridPosition(item.GridPosition.Position, item.GridPosition.Layer)));
            }
        }

        public float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }
    }
}