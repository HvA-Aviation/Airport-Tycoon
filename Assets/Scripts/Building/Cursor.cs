using System;
using UnityEngine;

namespace Building
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Grid _grid;

        [SerializeField] private Color _validColor;
        [SerializeField] private Color _invalidColor;

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
            if (Input.GetMouseButton(0))
            {
                _grid.Set(position, 0);
            }
            if (Input.GetMouseButton(1))
            {
                _grid.Remove(position);
            }

            transform.position = clampedValue;
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