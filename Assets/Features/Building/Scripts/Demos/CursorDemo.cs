using System;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
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
            GameManager.Instance.BuildingManager.ChangeSelectedBuildable(_defaultSelectedBuilding);
        }

        private void Update()
        {
            Vector3Int position = _cursor.WorldToGirdPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (!_cursor.IsEnabled)
                return;

            if (Input.GetMouseButton(0))
            {
                _cursor.Press(position);
            } else if (Input.GetMouseButtonUp(0))
            {
                _cursor.Release(position);
            }
            else
            {
                _cursor.Hover(position);
            }
            
            _cursor.UpdateVisuals();
        }
    }
}