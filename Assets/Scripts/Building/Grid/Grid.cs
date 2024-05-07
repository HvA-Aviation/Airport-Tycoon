using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Building.Datatypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Building.Datatypes.TileData;

public class Grid : MonoBehaviour
{
    /// <summary>
    /// contains the whole grid. the 3d array is used as the following [x,y,z]
    /// x = x-axis
    /// y = y-axis
    /// z = z-axis for multiple layers for example floor tile layer and decoration layer
    /// </summary>
    private int[,,] _cells;

    private int[,,] _rotations;

    [SerializeField] private BuildableAtlas _atlas;
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private Vector3Int _gridSize;
    [SerializeField] private float _cellSize;

    [SerializeField] private List<List<Vector3Int>> _cellGroup;

    /// <summary>
    /// If true the map will be updated at the end of the frame and set to false
    /// </summary>
    private bool _mapUpdated;

    public float CellSize => _cellSize;
    
    void Start()
    {
        if (_atlas == null)
            Debug.LogError("No atlas is assigned!");

        _cells = new int[_gridSize.x, _gridSize.y, _gridSize.z];
        _rotations = new int[_gridSize.x, _gridSize.y, _gridSize.z];
        _cellGroup = new List<List<Vector3Int>>();
        PopulateCells();
    }

    /// <summary>
    /// Creates a flattend 2d array to see if it is traversable
    /// </summary>
    /// <returns>A flattend 2d bool array with false as traverable</returns>
    public bool[,] UnTraversable()
    {
        bool[,] unTraversable = new bool[_gridSize.x, _gridSize.y];

        for (int z = 0; z < _gridSize.z; z++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    if (unTraversable[x, y] || _cells[x, y, z] == -1)
                        continue;

                    unTraversable[x, y] = !_atlas.Items[_cells[x, y, z]].Traversable;
                }
            }
        }

        return unTraversable;
    }

    /// <summary>
    /// Sets the default value of the whole array
    /// </summary>
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

    /// <summary>
    /// If the map update was called this frame update it
    /// </summary>
    private void LateUpdate()
    {
        if (_mapUpdated)
        {
            UpdateMap();
            _mapUpdated = false;
        }
    }

    /// <summary>
    /// Loops through the whole grid and sets all the cells to what they are supposed to be
    /// TODO add a buffer, so correct tiles aren't changed
    /// </summary>
    private void UpdateMap()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int z = 0; z < _gridSize.z; z++)
                {
                    var cell = _cells[x, y, z];

                    Vector3Int offset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                        Mathf.RoundToInt(_tilemap.transform.position.y),
                        Mathf.RoundToInt(_tilemap.transform.position.z));


                    TileChangeData tile = new TileChangeData()
                    {
                        position = new Vector3Int(x, y, z) - offset,
                    };

                    //if tile exists add rotation and a tile
                    if (cell != -1)
                    {
                        tile.transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, _rotations[x, y, z] * -90));
                        tile.tile = _atlas.Items[cell].Tile;
                    }

                    _tilemap.SetTile(tile, false);
                }
            }
        }
    }

    /// <summary>
    /// Get the build index of the given position
    /// </summary>
    /// <param name="gridVector">Position in grid</param>
    /// <returns>1 or the cell build index</returns>
    public int Get(Vector3Int gridVector)
    {
        //when out of bounds returns 1
        if (OutOfBounds(gridVector))
            return 1;

        return _cells[gridVector.x, gridVector.y, gridVector.z];
    }

    /// <summary>
    /// Sets the cell if it is empty
    /// </summary>
    /// <param name="gridVector">Position on grid</param>
    /// <param name="buildIndex">Build index of the tile found in BuildableAtlas</param>
    /// <returns>True if setting was a success</returns>
    public bool Set(Vector3Int gridVector, int buildIndex)
    {
        if (Get(gridVector) == -1)
        {
            _cells[gridVector.x, gridVector.y, gridVector.z] = buildIndex;
            _rotations[gridVector.x, gridVector.y, gridVector.z] = 0;

            _mapUpdated = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Sets the cell if it is empty
    /// </summary>
    /// <param name="gridVector">Position on grid</param>
    /// <param name="tile">Tile that has to be placed. It has to be set in the BuildableAtlas</param>
    /// <returns>True if setting was a success</returns>
    public bool Set(Vector3Int gridVector, Tile tile)
    {
        //get tile from atlas
        TileData tileData = _atlas.Items.FirstOrDefault(x => x.Tile == tile);

        if (tileData == default)
        {
            Debug.LogError("Tile \"" + tile.name + "\" not found in Atlas");
            return false;
        }

        return Set(gridVector, Array.FindIndex(_atlas.Items, x => x.Tile == tileData.Tile));
    }

    /// <summary>
    /// Sets a group of cells if the target cells are empty
    /// </summary>
    /// <param name="gridVectors">Positions on grid</param>
    /// <param name="tiles">Tiles that are to be placed. They have to be set in the BuildableAtlas</param>
    /// <param name="rotation">Rotation of the individual tiles (0 to 3 clockwise)</param>
    /// <returns>True if setting was a success</returns>
    public bool SetGroup(List<Vector3Int> gridVectors, List<Tile> tiles, int rotation)
    {
        //check if all the positions are available
        foreach (var position in gridVectors)
        {
            if (Get(position) != -1)
                return false;
        }

        for (int i = 0; i < gridVectors.Count; i++)
        {
            //get tile from atlas
            TileData tileData = _atlas.Items.FirstOrDefault(x => x.Tile == tiles[i]);

            if (tileData == default)
            {
                Debug.LogError("Tile \"" + tiles[i].name + "\" not found in Atlas");
                return false;
            }

            _cells[gridVectors[i].x, gridVectors[i].y, gridVectors[i].z] =
                Array.FindIndex(_atlas.Items, x => x.Tile == tileData.Tile);
            _rotations[gridVectors[i].x, gridVectors[i].y, gridVectors[i].z] = rotation;
            _mapUpdated = true;
        }

        _cellGroup.Add(gridVectors);

        return true;
    }

    /// <summary>
    /// Removes a tile from the grid
    /// </summary>
    /// <param name="gridVector">Position to be removed</param>
    /// <returns>True if remove was successful</returns>
    public bool Remove(Vector3Int gridVector)
    {
        if (!OutOfBounds(gridVector) && Get(gridVector) != -1)
        {
            //checks if given tile is part of a linked group. If not only add the given position
            var group = _cellGroup.FirstOrDefault(x => x.Any(j => j == gridVector));
            if (group == default)
                group = new List<Vector3Int>() { gridVector };

            //Remove from array
            foreach (var item in group)
            {
                _cells[item.x, item.y, item.z] = -1;
            }

            //remove from group
            _cellGroup.Remove(group);

            _mapUpdated = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if given position is within the bounds of the grid
    /// </summary>
    /// <param name="gridVector"></param>
    /// <returns>True when within the grid</returns>
    private bool OutOfBounds(Vector3Int gridVector)
    {
        if (gridVector.x < 0 || gridVector.x > _gridSize.x - 1 || gridVector.y < 0 || gridVector.y > _gridSize.y - 1 ||
            gridVector.z < 0 || gridVector.z > _gridSize.z - 1)
            return true;

        return false;
    }
    
    /// <summary>
    /// Easy way of checking if a tile is empty. This is a seperate method, because we could easily change this in the future
    /// </summary>
    /// <param name="gridVector">Position in grid</param>
    /// <returns>True if empty</returns>
    public bool IsEmpty(Vector3Int gridVector)
    {
        return Get(gridVector) == -1;
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

    /// <summary>
    /// Shows the grid and the traversable nodes
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_cells == null)
        {
            _cells = new int[_gridSize.x, _gridSize.y, _gridSize.z];
        }

        var untraversable = UnTraversable();

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

                Gizmos.color = untraversable[x, y] ? Color.red : Color.green;
                Gizmos.DrawSphere(origin, _cellSize / 10);
            }
        }
    }
}