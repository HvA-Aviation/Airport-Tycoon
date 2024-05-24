using System;
using System.Collections.Generic;
using System.Linq;
using Features.Building.Scripts.Datatypes;
using Features.Managers;
using Features.Workers.TaskCommands;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Features.Building.Scripts.Datatypes.TileData;

namespace Features.Building.Scripts.Grid
{
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// contains the whole grid. the 3d array is used as the following [x,y,z]
        /// x = x-axis
        /// y = y-axis
        /// z = z-axis for multiple layers for example floor tile layer and decoration layer
        /// </summary>
        private CellData[,,] _cells;

        [SerializeField] private BuildableAtlas _atlas;
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private Vector3Int _gridSize;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _buildingStaringOpacity;

        [SerializeField] private List<List<Vector3Int>> _cellGroup;
        [SerializeField] public bool[,] TraversableTiles { get; private set; }
        private Vector3Int _gridOffset;
        private List<TileChangeData> _gridChangeBuffer = new List<TileChangeData>();
        private List<TileColorData> _gridColorBuffer = new List<TileColorData>();

        public Vector3Int GridSize => _gridSize;
        public float CellSize => _cellSize;

        void Start()
        {
            if (_atlas == null)
                Debug.LogError("No atlas is assigned!");

            _gridOffset = new Vector3Int(Mathf.RoundToInt(_tilemap.transform.position.x),
                Mathf.RoundToInt(_tilemap.transform.position.y),
                Mathf.RoundToInt(_tilemap.transform.position.z));

            _cells = new CellData[_gridSize.x, _gridSize.y, _gridSize.z];
            _cellGroup = new List<List<Vector3Int>>();
            PopulateCells();

            //create and populate traversabletiles
            TraversableTiles = new bool[_gridSize.x, _gridSize.y];
            UpdateTraversable();
        }

        /// <summary>
        /// Creates a flattend 2d array to see if the cell position is traversable
        /// </summary>
        /// <returns>A flattend 2d bool array with false as traversable</returns>
        public void UpdateTraversable()
        {
            bool[,] unTraversable = new bool[_gridSize.x, _gridSize.y];

            for (int z = 0; z < _gridSize.z; z++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    for (int y = 0; y < _gridSize.y; y++)
                    {
                        if (unTraversable[x, y] || _cells[x, y, z].Tile == CellData.empty.Tile)
                            continue;

                        if (_cells[x, y, z].CurrentWorkLoad >= _cells[x, y, z].WorkLoad)
                            unTraversable[x, y] = _atlas.Items[_cells[x, y, z].Tile].UnTraversable;
                    }
                }
            }

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    TraversableTiles[x, y] = !unTraversable[x, y];
                }
            }
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
                        _cells[x, y, z] = CellData.empty;
                    }
                }
            }
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
            foreach (TileChangeData tileChangeData in _gridChangeBuffer)
            {
                _tilemap.SetTile(tileChangeData, true);
            }

            if (_gridChangeBuffer.Count > 0)
            {
                //Update Traversable
                UpdateTraversable();
            }

            foreach (TileColorData tileChangeData in _gridColorBuffer)
            {
                _tilemap.SetTileFlags(tileChangeData.Position, TileFlags.None);
                _tilemap.SetColor(tileChangeData.Position, tileChangeData.Color);
            }

            _gridChangeBuffer.Clear();
            _gridColorBuffer.Clear();
        }

        public bool BuildTile(Vector3Int gridVector, float speed)
        {
            if (Get(gridVector) == -1)
                return true;

            List<Vector3Int> buildTiles = new List<Vector3Int>() { gridVector };
            for (int i = 0; i < _cellGroup.Count; i++)
            {
                if (_cellGroup[i].Contains(gridVector))
                {
                    foreach (Vector3Int child in _cellGroup[i])
                    {
                        if (child != buildTiles[0])
                            buildTiles.Add(child);
                    }
                }
            }

            bool isFinished = false;
            foreach (Vector3Int tile in buildTiles)
            {
                _cells[tile.x, tile.y, tile.z].CurrentWorkLoad = Mathf.Clamp(
                    _cells[tile.x, tile.y, tile.z].CurrentWorkLoad + speed * Time.deltaTime, 0,
                    _cells[tile.x, tile.y, tile.z].WorkLoad);

                CellData cellData = _cells[tile.x, tile.y, tile.z];

                float buildAmount = _buildingStaringOpacity + (cellData.CurrentWorkLoad / cellData.WorkLoad * (1 - _buildingStaringOpacity));

                Color color = _tilemap.GetColor(tile);
                color.a = buildAmount;
                _gridColorBuffer.Add(new TileColorData()
                {
                    Position = tile,
                    Color = color,
                });

                isFinished = _cells[tile.x, tile.y, tile.z].CurrentWorkLoad == _cells[tile.x, tile.y, tile.z].WorkLoad;
            }

            return isFinished;
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

            return _cells[gridVector.x, gridVector.y, gridVector.z].Tile;
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
                CellData cellData = _cells[gridVector.x, gridVector.y, gridVector.z];

                cellData.Tile = buildIndex;
                cellData.Rotation = 0;
                cellData.WorkLoad = _atlas.Items[buildIndex].WorkLoad;

                _cells[gridVector.x, gridVector.y, gridVector.z] = cellData;

                Color color = _atlas.Items[buildIndex].Color;
                color.a = _buildingStaringOpacity;
                
                _gridChangeBuffer.Add(new TileChangeData()
                {
                    position = gridVector,
                    color = color,
                    tile = _atlas.Items[buildIndex].Tile,
                    transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, cellData.Rotation * -90))
                });

                GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(new BuildTask(gridVector));

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
            foreach (Vector3Int position in gridVectors)
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

                CellData cellData = _cells[gridVectors[i].x, gridVectors[i].y, gridVectors[i].z];

                cellData.Tile = Array.FindIndex(_atlas.Items, x => x.Tile == tileData.Tile);
                cellData.Rotation = rotation;
                cellData.WorkLoad = tileData.WorkLoad;

                _cells[gridVectors[i].x, gridVectors[i].y, gridVectors[i].z] = cellData;

                Color color = _atlas.Items[cellData.Tile].Color;
                color.a = _buildingStaringOpacity;
                
                _gridChangeBuffer.Add(new TileChangeData()
                {
                    position = gridVectors[i],
                    color = color,
                    tile = tileData.Tile,
                    transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, cellData.Rotation * -90))
                });
            }

            GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(new BuildTask(gridVectors[0]));
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
                List<Vector3Int> group = _cellGroup.FirstOrDefault(x => x.Any(j => j == gridVector));
                if (group == default)
                    group = new List<Vector3Int>() { gridVector };

                //Remove from array
                foreach (Vector3Int item in group)
                {
                    _cells[item.x, item.y, item.z].Clear();
                }

                _gridChangeBuffer.Add(new TileChangeData()
                {
                    position = gridVector,
                    tile = null,
                });

                //remove from group
                _cellGroup.Remove(group);
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
            if (gridVector.x < 0 || gridVector.x > _gridSize.x - 1 || gridVector.y < 0 ||
                gridVector.y > _gridSize.y - 1 ||
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
                _cells = new CellData[_gridSize.x, _gridSize.y, _gridSize.z];
            }

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector2 origin = new Vector2(x, y) * _cellSize - (Vector2)transform.position;
                    float offset = _cellSize / 2;

                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(origin + new Vector2(-offset, offset), origin + new Vector2(offset, offset));
                    Gizmos.DrawLine(origin + new Vector2(-offset, -offset), origin + new Vector2(offset, -offset));

                    Gizmos.DrawLine(origin + new Vector2(offset, offset), origin + new Vector2(offset, -offset));
                    Gizmos.DrawLine(origin + new Vector2(-offset, offset), origin + new Vector2(-offset, -offset));
                }
            }

            if (Application.isPlaying)
            {
                bool[,] traversable = TraversableTiles;

                for (int x = 0; x < _gridSize.x; x++)
                {
                    for (int y = 0; y < _gridSize.y; y++)
                    {
                        Vector2 origin = new Vector2(x, y) * _cellSize - (Vector2)transform.position;

                        Gizmos.color = traversable[x, y] ? Color.green : Color.red;
                        Gizmos.DrawSphere(origin, _cellSize / 10);
                    }
                }
            }
        }
    }
}