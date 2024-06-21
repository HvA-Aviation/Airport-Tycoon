using System.Collections.Generic;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Unity.Collections;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;
using Utilities = Features.Building.Scripts.Datatypes.UtilityType;

public class GridManager : MonoBehaviour
{
    /// <summary>
    /// Temporary solution to fix having every npc create a grid
    /// </summary>

    [SerializeField] private Grid _grid;

    public Grid Grid => _grid;

    public NativeHashMap<Vector3Int, Node> NodeGrid { get; private set; }
    public NativeHashMap<Vector3Int, Node> NoClipNodeGrid { get; private set; }

    private void Awake()
    {
        NodeGrid = new NativeHashMap<Vector3Int, Node>(_grid.GridSize.x * _grid.GridSize.y, Allocator.Persistent);
        NoClipNodeGrid = new NativeHashMap<Vector3Int, Node>(_grid.GridSize.x * _grid.GridSize.y, Allocator.Persistent);
        GameManager.Instance.EventManager.Subscribe(Features.EventManager.EventId.OnGridUpdateEvent, (args) => CreateGrid());
    }

    void Start()
    {
        CreateGrid();
        for (int x = 0; x < _grid.GridSize.x; x++)
        {
            for (int y = 0; y < _grid.GridSize.y; y++)
            {
                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    untraversable = false
                };
                NoClipNodeGrid.Add(new Vector3Int(x, y, 0), node);
            }
        }
    }

    /// <summary>
    /// Create a grid of nodes
    /// </summary>
    void CreateGrid()
    {
        NodeGrid.Clear();
        for (int x = 0; x < _grid.GridSize.x; x++)
        {
            for (int y = 0; y < _grid.GridSize.y; y++)
            {
                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    untraversable = !_grid.TraversableTiles[x, y]
                };
                NodeGrid.Add(new Vector3Int(x, y, 0), node);
            }
        }
    }

    public Dictionary<Vector3Int, List<Vector3Int>> GetUtilities(Utilities utilityType)
    {
        _grid.GetUtilities(utilityType, out Dictionary<Vector3Int, List<Vector3Int>> keyValuePairs);
        return keyValuePairs;
    }

    public int GetRotation(Vector3Int position) => _grid.GetRotation(position);
    public Vector3Int GetPaxSpawnPoint() => _grid.PaxSpawnPosition;

    private void OnApplicationQuit()
    {
        NodeGrid.Dispose();
        NoClipNodeGrid.Dispose();

    }
}
