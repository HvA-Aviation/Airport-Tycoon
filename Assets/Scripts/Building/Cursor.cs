using System;
using UnityEngine;

namespace Building
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Grid _grid;

        [SerializeField]private Color _validColor;
        [SerializeField]private Color _invalidColor;

        private void Start()
        {
            _grid = FindObjectOfType<Grid>();

            transform.localScale = Vector3.one * _grid.CellSize;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (_spriteRenderer.color != _validColor)
                {
                    _spriteRenderer.color = _validColor;
                }
                else
                {
                    _spriteRenderer.color = _invalidColor;
                }
            }

            if (transform.localPosition == Vector3.zero)
            {
                _spriteRenderer.color = _invalidColor;
            }
            else
            {
                _spriteRenderer.color = _validColor;
            }

            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;

            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            transform.position = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
                RoundToMultiple(pos.y, _grid.CellSize));
        }

        public float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }
    }
}