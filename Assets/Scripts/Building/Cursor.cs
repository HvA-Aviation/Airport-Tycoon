using System;
using System.Collections.Generic;
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

            /*if (Input.GetMouseButton(0))
            {
                _grid.Set(position, 0);
            }
            if (Input.GetMouseButton(1))
            {
                _grid.Remove(position);
            }*/

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                _currentMouse = Input.GetMouseButtonDown(0) ? 0 : 1;
            }


            if (Input.GetMouseButtonDown(_currentMouse))
            {
                _selectedGroup = new List<Vector3Int>() { position };
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
                Selection(position);
            }


            //transform.position = clampedValue;
        }

        private List<Vector3Int> _selectedGroup;

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