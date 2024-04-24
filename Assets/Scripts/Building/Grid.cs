using System;
using System.Collections;
using System.Collections.Generic;
using Building.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    /// <summary>
    /// contains the whole grid. the 3d array is used as the following [x,y,z]
    /// x = x-axis
    /// y = y-axis
    /// z = z-axis for multiple layers for example floor tile layer and decoration layer
    /// </summary>
    private int[,,] _cells;

    [SerializeField] private BuildableAtlas _atlas;
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private Vector3Int _gridSize;
    [SerializeField] private float _cellSize;
    
    private bool _mapUpdated;

    public float CellSize => _cellSize;


    void Start()
    {
        if (_atlas == null)
            Debug.LogError("No atlas is assigned!");

        _cells = new int[_gridSize.x, _gridSize.y, _gridSize.z];
        PopulateCells();
    }

    private void PopulateCells()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int z = 0; z < _gridSize.z; z++)
                {
                    _cells[x, y, z] = -1;
                }
            }
        }
    }
    
    private void LateUpdate()
    {
        if (_mapUpdated)
        {
            UpdateMap();
            _mapUpdated = false;
        }
    }

    private void UpdateMap()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int z = 0; z < _gridSize.z; z++)
                {
                    var cell = _cells[x, y, z];

                    Tile tile = null;
                    Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x + .5f),
                        Mathf.RoundToInt(_tilemap.transform.position.y + .5f),
                        Mathf.RoundToInt(_tilemap.transform.position.z + .5f));
                    
                    if (_cells[x, y, z] != -1)
                    {
                        tile = _atlas.Items[cell].Sprite;
                        
                    }
                    
                    _tilemap.SetTile(new Vector3Int(x, y, z) - offset, tile);

                }
            }
        }
    }

    public int Get(Vector3Int gridVector)
    {
        if (OutOfBounds(gridVector))
            return 1;

        return _cells[gridVector.x, gridVector.y, gridVector.z];
    }

    private bool OutOfBounds(Vector3Int gridVector)
    {
        if (gridVector.x < 0 || gridVector.x > _gridSize.x - 1 || gridVector.y < 0 || gridVector.y > _gridSize.y - 1 ||
            gridVector.z < 0 || gridVector.z > _gridSize.z - 1)
            return true;

        return false;
    }

    public bool Set(Vector3Int gridVector, int buildIndex)
    {
        if (Get(gridVector) == -1)
        {
            _cells[gridVector.x, gridVector.y, gridVector.z] = buildIndex;
            _mapUpdated = true;
            return true;
        }

        return false;
    }

    public bool Remove(Vector3Int gridVector)
    {
        if (!OutOfBounds(gridVector) && Get(gridVector) != -1)
        {
            _cells[gridVector.x, gridVector.y, gridVector.z] = -1;
            _mapUpdated = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Converts the clamped world position of the cursor to a grid vector that
    /// has the values of the position the cursor is on
    /// </summary>
    /// <param name="worldPosition">Clamped world position (is rounded to the closest multiple of the cell size)</param>
    /// <param name="layer">The build layer</param>
    /// <returns >Grid vector that has the values of the position the cursor is on</returns>
    public Vector3Int ClampedWorldToGridPosition(Vector2 worldPosition, int layer)
    {
        Vector2 gridPosition = (worldPosition + (Vector2)transform.position) * (1 / _cellSize);
        return new Vector3Int((int)gridPosition.x, (int)gridPosition.y, layer);
    }

    public bool IsGridPositionEmpty(Vector3Int gridVector)
    {
        return Get(gridVector) == -1;
    }

    private void OnDrawGizmos()
    {
        if (_cells == null)
        {
            _cells = new int[_gridSize.x, _gridSize.y, _gridSize.z];
        }

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                //var origin = new Vector2(x, y) * _cellSize - (new Vector2(_gridSize.x, _gridSize.y) * _cellSize / 2 - Vector2.one * _cellSize / 2);
                Vector2 origin = new Vector2(x, y) * _cellSize - (Vector2)transform.position;
                var offset = _cellSize / 2;

                Gizmos.color = Color.gray;
                Gizmos.DrawLine(origin + new Vector2(-offset, offset), origin + new Vector2(offset, offset));
                Gizmos.DrawLine(origin + new Vector2(-offset, -offset), origin + new Vector2(offset, -offset));

                Gizmos.DrawLine(origin + new Vector2(offset, offset), origin + new Vector2(offset, -offset));
                Gizmos.DrawLine(origin + new Vector2(-offset, offset), origin + new Vector2(-offset, -offset));
            }
        }
    }
}