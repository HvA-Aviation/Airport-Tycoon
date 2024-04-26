using System;
using System.Collections.Generic;
using Building.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Grid _grid;
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

        [SerializeField] private List<Vector3Int> _selectedGroup = new List<Vector3Int>();
        [SerializeField] private List<Vector3Int> _size = new List<Vector3Int>();
        [SerializeField] private int _rotation = 0;
        [SerializeField] private BrushType _brush;

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

            var position = _grid.ClampedWorldToGridPosition(clampedValue, 0);

            UpdateBuildColor(_grid.IsGridPositionEmpty(position));

            if (_brush == BrushType.Multi)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    _currentMouse = Input.GetMouseButtonDown(0) ? 0 : 1;
                }
                else if (Input.GetMouseButtonUp(_currentMouse))
                {
                    if (_currentMouse == 0)
                    {
                        foreach (var selected in _selectedGroup)
                        {
                            _grid.Set(selected, 0);
                        }
                    }
                    else
                    {
                        foreach (var selected in _selectedGroup)
                        {
                            _grid.Remove(selected);
                        }
                    }

                    _selectedGroup = new List<Vector3Int>();
                    _tilemap.ClearAllTiles();
                }
                else if (Input.GetMouseButton(_currentMouse) && _selectedGroup.Count > 0)
                {
                    var min = Vector3Int.Min(_selectedGroup[0], position);
                    var max = Vector3Int.Max(_selectedGroup[0], position);

                    List<Vector3Int> currentSelectedGroup = new List<Vector3Int>();

                    for (var x = min.x; x < max.x + 1; x++)
                    {
                        for (var y = min.y; y < max.y + 1; y++)
                        {
                            currentSelectedGroup.Add(new Vector3Int(x, y, 0));
                        }
                    }

                    Hover(Vector3Int.zero, currentSelectedGroup);
                } else
                {
                    Hover(position, _size);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    for (int i = 0; i < _size.Count; i++)
                    {
                        _size[i] = new Vector3Int(-_size[i].y, _size[i].x, _size[i].z);
                    }

                    _rotation++;

                    if (_rotation > 3)
                    {
                        _rotation = 0;
                    }
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    _grid.SetGroup(_selectedGroup, new List<int>() { 0, 0 }, _rotation);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    _grid.Remove(position);
                } else
                {
                    Hover(position, _size);
                }
            }

            /*if (Input.GetMouseButtonDown(0))
            {
                _grid.SetGroup(_selectedGroup, new List<int>() {0, 0});

            }
            else if (Input.GetMouseButtonDown(1))
            {
                _grid.Remove(position);

            }
            else
            {
                Hover(position, _size);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                for (int i = 0; i < _size.Count; i++)
                {
                    _size[i] = new Vector3Int(_size[i].y, -_size[i].x, _size[i].z);
                }
            }*/
        }

        private void Hover(Vector3Int position, List<Vector3Int> shapes)
        {
            Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                Mathf.RoundToInt(_tilemap.transform.position.y),
                Mathf.RoundToInt(_tilemap.transform.position.z));


            List<Vector3Int> currentSelectedGroup = new List<Vector3Int>();
            foreach (var shape in shapes)
            {
                currentSelectedGroup.Add(position + shape);
            }

            foreach (var gridPosition in _selectedGroup)
            {
                if (!currentSelectedGroup.Contains(gridPosition))
                {
                    _tilemap.SetTile(gridPosition - offset, null);
                }
            }

            foreach (var gridPosition in currentSelectedGroup)
            {
                if (!_selectedGroup.Contains(gridPosition))
                {
                    Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
                    tempTile.sprite = _spriteRenderer.sprite;
                    tempTile.color = _grid.IsGridPositionEmpty(gridPosition) ? _validColor : _invalidColor;
                    _tilemap.SetTile(gridPosition - offset, tempTile);
                }
            }

            if (_selectedGroup.Count > 0 && currentSelectedGroup.Contains(_selectedGroup[0]))
            {
                currentSelectedGroup.Remove(_selectedGroup[0]);
                currentSelectedGroup.Insert(0, _selectedGroup[0]);
            }

            _selectedGroup = currentSelectedGroup;
        }

        private void Hover(List<Vector3Int> positions)
        {
            Hover(Vector3Int.zero, positions);
        }

        private void Selection(Vector3Int position)
        {
            var min = Vector3Int.Min(_selectedGroup[0], position);
            var max = Vector3Int.Max(_selectedGroup[0], position);

            Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                Mathf.RoundToInt(_tilemap.transform.position.y),
                Mathf.RoundToInt(_tilemap.transform.position.z));

            List<Vector3Int> currentSelectedGroup = new List<Vector3Int>();

            for (var x = min.x; x < max.x + 1; x++)
            {
                for (var y = min.y; y < max.y + 1; y++)
                {
                    currentSelectedGroup.Add(new Vector3Int(x, y, 0));
                }
            }

            //remove the tiles that aren't selected
            foreach (var gridPosition in _selectedGroup)
            {
                if (!currentSelectedGroup.Contains(gridPosition))
                {
                    _tilemap.SetTile(gridPosition - offset, null);
                }
            }

            foreach (var gridPosition in currentSelectedGroup)
            {
                var tile = _tilemap.GetSprite(gridPosition - offset);
                if (tile == null)
                {
                    Tile tempTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
                    tempTile.sprite = _spriteRenderer.sprite;
                    tempTile.color = _grid.IsGridPositionEmpty(gridPosition) ? _validColor : _invalidColor;
                    _tilemap.SetTile(gridPosition - offset, tempTile);
                }
            }


            currentSelectedGroup.Remove(_selectedGroup[0]);
            currentSelectedGroup.Insert(0, _selectedGroup[0]);

            _selectedGroup = currentSelectedGroup;
        }

        private void UpdateBuildColor(bool isValid)
        {
            if (isValid && _spriteRenderer.color != _validColor)
            {
                _spriteRenderer.color = _validColor;
            }
            else if (!isValid)
            {
                _spriteRenderer.color = _invalidColor;
            }
        }

        public float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }
    }
}