using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.EventManager;
using Features.Managers;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileUpdateData = Features.Building.Scripts.Datatypes.TileUpdateData;

namespace Features.Building.Scripts.Grid
{
    public class GridTilemap : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Grid _grid;
        private Vector3Int _gridOffset;
        [SerializeField] private float _buildingStaringOpacity;

        private List<TileUpdateData> _gridChangeBuffer = new List<TileUpdateData>();
        private List<TileColorData> _gridColorBuffer = new List<TileColorData>();

        public List<TileUpdateData> GridChangeBuffer => _gridChangeBuffer;
        public List<TileColorData> GridColorBuffer => _gridColorBuffer;

        private void Start()
        {
            _gridOffset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                Mathf.RoundToInt(_tilemap.transform.position.y),
                Mathf.RoundToInt(_tilemap.transform.position.z));

            GameManager.Instance.EventManager.Subscribe(EventId.OnChangeTile, AddTileUpdateData);
            GameManager.Instance.EventManager.Subscribe(EventId.OnChangeColorTile, AddTileColorData);
        }

        private void AddTileUpdateData(EventArgs eventArgs)
        {
            GridChangeBuffer.Add(eventArgs as TileUpdateData);
        }

        private void AddTileColorData(EventArgs eventArgs)
        {
            GridColorBuffer.Add(eventArgs as TileColorData);
        }

        /// <summary>
        /// If the map update was called this frame update it
        /// </summary>
        private void LateUpdate()
        {
            if (_gridChangeBuffer.Count > 0 || _gridColorBuffer.Count > 0)
            {
                UpdateMap();
            }
        }

        /// <summary>
        /// Loops through the whole grid and sets all the cells to what they are supposed to be
        /// </summary>
        private void UpdateMap()
        {
            bool traversableChanged = false;
            foreach (TileUpdateData tileChangeData in _gridChangeBuffer)
            {
                Color color = _tilemap.GetColor(tileChangeData.Position);
                color.a = _buildingStaringOpacity;
                tileChangeData.Color = color;
                
                _tilemap.SetTile(tileChangeData, true);
            }

            if (_gridChangeBuffer.Count > 0)
            {
                //Update Traversable
                _grid.UpdateTraversable();
            }

            foreach (TileColorData tileChangeData in _gridColorBuffer)
            {
                float buildAmount = _buildingStaringOpacity + (tileChangeData.Progress * (1 - _buildingStaringOpacity));

                Color color = _tilemap.GetColor(tileChangeData.Position);
                color.a = buildAmount;

                _tilemap.SetTileFlags(tileChangeData.Position, TileFlags.None);
                _tilemap.SetColor(tileChangeData.Position, color);
            }

            _gridChangeBuffer.Clear();
            _gridColorBuffer.Clear();
        }
    }
}