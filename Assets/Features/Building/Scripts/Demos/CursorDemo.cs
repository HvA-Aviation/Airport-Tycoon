using System;
using Features.Building.Scripts.Datatypes;
using UnityEngine;
using Cursor = Features.Building.Scripts.Grid.Cursor;
using TileGrid = Features.Building.Scripts.Grid.Grid;

namespace Features.Building.Scripts.Demos
{
    public class CursorDemo : MonoBehaviour
    {
        [SerializeField] private Cursor _cursor;
        [SerializeField] private TileGrid _grid;
        [SerializeField] private BuildableObject _defaultSelectedBuilding;

        private void Start()
        {
            _cursor.ChangeSelectedBuildable(_defaultSelectedBuilding);
        }

        private void Update()
        {
            Vector3Int position = _cursor.WorldToGirdPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (!_cursor.IsEnabled)
                return;

            if (Input.GetKey(KeyCode.B))
            {
                _grid.BuildTile(position, 1f);
                return;
            }

            if (Input.GetMouseButton(1) || Input.GetMouseButtonUp(1))
            {
                //remove tiles
                _cursor.MultiBrushSelect(position, 1);
            }
            else
            {
                switch (_cursor.BrushType)
                {
                    case BrushType.Multi:
                        _cursor.MultiBrushSelect(position, 0);
                        break;
                    case BrushType.Outline:
                        _cursor.OutlineBrushSelect(position);
                        break;
                    case BrushType.Drag:
                        _cursor.DragBrush(position);
                        break;
                    case BrushType.Single:
                        _cursor.SingleBrush(position);
                        break;
                }
            }
        }
    }
}