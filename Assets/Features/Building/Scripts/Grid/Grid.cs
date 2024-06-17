using Features.Building.Scripts.Datatypes;
using Features.EventManager;
using Features.Managers;
using Features.Workers.TaskCommands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [SerializeField] private Vector3Int _gridSize;
        [SerializeField] private float _cellSize;

        [SerializeField] private List<List<Vector3Int>> _cellGroup;
        [SerializeField] public bool[,] TraversableTiles { get; private set; }

        private Dictionary<UtilityType, List<Vector3Int>> _utilityLocations =
            new Dictionary<UtilityType, List<Vector3Int>>()
            {
                { UtilityType.Security, new List<Vector3Int>() },
                { UtilityType.CheckIn, new List<Vector3Int>() },
                { UtilityType.Gate, new List<Vector3Int>() }
            };

        public Vector3Int GridSize => _gridSize;
        public float CellSize => _cellSize;

        void Start()
        {
            if (_atlas == null)
                Debug.LogError("No atlas is assigned!");

            _cells = new CellData[_gridSize.x, _gridSize.y, _gridSize.z];
            _cellGroup = new List<List<Vector3Int>>();
            PopulateCells();

            //create and populate traversabletiles
            TraversableTiles = new bool[_gridSize.x, _gridSize.y];
            UpdateTraversable();
            
            GameManager.Instance.EventManager.TriggerEvent(EventId.GridUpdateEvent);
        }

        /// <summary>
        /// Gets the utilities of a given type. This will only return the ones that have been built
        /// </summary>
        /// <param name="utilityType">The type of the utility</param>
        /// <returns>A list with all the utilities</returns>
        public List<Vector3Int> GetUtilities(UtilityType utilityType)
        {
            List<Vector3Int> utilities = new List<Vector3Int>();
            utilities.AddRange(_utilityLocations[utilityType]);
            return utilities;
        }

        /// <summary>
        /// Gets the utility workload by position
        /// </summary>
        /// <param name="target">Position of the utility</param>
        /// <returns>Workload as a float</returns>
        public float GetUtilityWorkLoad(Vector3Int target)
        {
            int index = Get(target);
            if (index == BuildableAtlas.Empty)
                return 0;

            return _atlas.GetTileData(index).BuildLoad;
        }

        /// <summary>
        /// Checks if work needs to be done 
        /// </summary>
        /// <param name="target">Position of the utility</param>
        /// <returns>Workload as a float</returns>
        public bool IsWorkDone(Vector3Int target)
        {
            int index = Get(target);
            if (index == -1)
                return true;

            return _atlas.GetTileData(index).BuildLoad == 0;
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
                            unTraversable[x, y] = _atlas.GetTileData(_cells[x, y, z].Tile).UnTraversable;
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

            GameManager.Instance.EventManager.TriggerEvent(EventId.GridUpdateEvent);
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
        /// Used by a worker when they are at a build position. This updates the data and enables the utilities when done
        /// </summary>
        /// <param name="gridVector">Build position</param>
        /// <param name="speed">Build speed / work load</param>
        /// <returns>Return if finised</returns>
        public bool BuildTile(Vector3Int gridVector, float speed)
        {
            if (IsEmpty(gridVector))
                return true;

            //get all the build tiles of a group
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
                    _cells[tile.x, tile.y, tile.z].CurrentWorkLoad + speed * GameManager.Instance.GameTimeManager.DeltaTime, 0,
                    _cells[tile.x, tile.y, tile.z].WorkLoad);

                CellData cellData = _cells[tile.x, tile.y, tile.z];
                
                GameManager.Instance.EventManager.TriggerEvent(EventId.OnChangeColorTile, new TileColorData()
                {
                    Position = tile,
                    Progress = cellData.CurrentWorkLoad / cellData.WorkLoad,
                });

                isFinished = _cells[tile.x, tile.y, tile.z].CurrentWorkLoad == _cells[tile.x, tile.y, tile.z].WorkLoad;

                //if finished and a utility type add a task
                if (isFinished && _atlas.TileIsType<UtilityTile>(cellData.Tile))
                {
                    UtilityType utilityType = _atlas.GetTileData<UtilityTile>(cellData.Tile).UtilityType;

                    if (utilityType != UtilityType.None)
                    {
                        _utilityLocations[utilityType].Add(gridVector);

                        if (utilityType == UtilityType.Security)
                        {
                            GameManager.Instance.TaskManager.SecurityTaskSystem.AddTask(new OperateTask(gridVector));
                        }
                        else
                        {
                            GameManager.Instance.TaskManager.GeneralTaskSystem.AddTask(new OperateTask(gridVector));
                        }
                    }
                }
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
        /// Get the rotation of the given position
        /// </summary>
        /// <param name="gridVector">Position in grid</param>
        /// <returns>0 or the cell rotation</returns>
        public int GetRotation(Vector3Int gridVector)
        {
            //when out of bounds returns 0 rotation
            if (OutOfBounds(gridVector))
                return 0;

            return _cells[gridVector.x, gridVector.y, gridVector.z].Rotation;
        }

        /// <summary>
        /// Sets the cell if it is empty
        /// </summary>
        /// <param name="gridVector">Position on grid</param>
        /// <param name="buildIndex">Build index of the tile found in BuildableAtlas</param>
        /// <returns>True if setting was a success</returns>
        public bool Set(Vector3Int gridVector, int buildIndex, int rotation = 0, bool addTask = true)
        {
            if (IsEmpty(gridVector))
            {
                CellData cellData = _cells[gridVector.x, gridVector.y, gridVector.z];

                cellData.Tile = buildIndex;
                cellData.Rotation = rotation;
                cellData.Rotation = 0;
                cellData.WorkLoad = _atlas.GetTileData(buildIndex).BuildLoad;

                _cells[gridVector.x, gridVector.y, gridVector.z] = cellData;
                
                GameManager.Instance.EventManager.TriggerEvent(EventId.OnChangeTile, new TileUpdateData()
                {
                    Position = gridVector,
                    Color = _atlas.GetTileData(buildIndex).Color,
                    Tile = _atlas.GetTileData(buildIndex).Tile,
                    Transform = Matrix4x4.Rotate(Quaternion.Euler(0, 0, cellData.Rotation * -90))
                });

                if (addTask)
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
            return Set(gridVector, _atlas.GetTileDataIndex(tile));
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
                if (!IsEmpty(position))
                    return false;
            }

            for (int i = 0; i < gridVectors.Count; i++)
            {
                BaseTile tileData = _atlas.GetTileData(tiles[i]);
                int index = _atlas.GetTileDataIndex(tiles[i]);

                Set(gridVectors[i], index, rotation, false);
            }

            GameManager.Instance.TaskManager.BuilderTaskSystem.AddTask(new BuildTask(gridVectors[0]));
            if (gridVectors.Count > 1)
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
            if (!OutOfBounds(gridVector) && !IsEmpty(gridVector))
            {
                //checks if given tile is part of a linked group. If not only add the given position
                List<Vector3Int> group = _cellGroup.FirstOrDefault(x => x.Any(j => j == gridVector));
                if (group == default)
                    group = new List<Vector3Int>() { gridVector };

                //Remove from array
                foreach (Vector3Int position in group)
                {
                    CellData cell = _cells[position.x, position.y, position.z];
                    UtilityType type = _atlas.GetTileData<UtilityTile>(cell.Tile).UtilityType;

                    if (type != UtilityType.None)
                    {
                        _utilityLocations[type].Remove(position);
                        GameManager.Instance.QueueManager.RemoveQueue(position);
                    }

                    _cells[position.x, position.y, position.z].Clear();
                    
                    GameManager.Instance.EventManager.TriggerEvent(EventId.OnChangeTile, new TileUpdateData()
                    {
                        Position = position,
                        Tile = null,
                    });
                }

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
            return Get(gridVector) == BuildableAtlas.Empty;
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