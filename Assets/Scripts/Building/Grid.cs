using System;
using System.Collections;
using System.Collections.Generic;
using Building.Datatypes;
using UnityEngine;

public class Grid : MonoBehaviour
{
    /// <summary>
    /// contains the whole grid. the 3d array is used as the following [x,y,z]
    /// x = x-axis
    /// y = y-axis
    /// z = z-axis for multiple layers for example floor tile layer and decoration layer
    /// </summary>
    private int[,,] _cells;

    [SerializeField] private GridVector _gridSize;
    [SerializeField] private float _cellSize;

    public float CellSize => _cellSize;

    // Start is called before the first frame update
    void Start()
    {
        _cells = new int[_gridSize.x, _gridSize.y, _gridSize.z];
    }

    // Update is called once per frame
    void Update()
    {
        
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
                var origin = new Vector2(x, y) * _cellSize - (new Vector2(_gridSize.x, _gridSize.y) * _cellSize / 2 - Vector2.one * _cellSize / 2);
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
