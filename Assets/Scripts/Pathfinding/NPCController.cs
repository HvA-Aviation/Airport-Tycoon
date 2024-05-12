using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using System.Collections;

public class NPCController : MonoBehaviour
{
    [Header("Lists")]
    List<Vector3Int> untraversableTiles = new List<Vector3Int>();
    Node[,,] grid;
    List<Node> backtrackedPath = new List<Node>();
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();

    [Header("Dependecies")]
    [SerializeField] private Grid _grid;

    [Header("Start | End")]
    public Vector3Int startNode;
    public Vector3Int endNode;

    [Header("Grid settings")]
    public int gridWidth;
    public int gridHeight;

    [Header("Display gizmos")]
    public bool displayGizmos;

    private void Start()
    {
        gridWidth = _grid.GridSize.x;
        gridHeight = _grid.GridSize.y;
        CreateGrid();
    }

    public float RoundToMultiple(float value, float roundTo)
    {
        return Mathf.RoundToInt(value / roundTo) * roundTo;
    }

    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //clamp to grid positions
        var clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
            RoundToMultiple(pos.y, _grid.CellSize));

        if (Input.GetKeyDown(KeyCode.P))
        {
            StopAllCoroutines();

            backtrackedPath.Clear();
            openList.Clear();
            closedList.Clear();

            endNode = new Vector3Int((int)clampedValue.x, (int)clampedValue.y, 0);

            FindPath(endNode);
            StartCoroutine(MoveToTarget(backtrackedPath));
        }
    }

    IEnumerator MoveToTarget(List<Node> path)
    {
        for (int i = path.Count - 1; i >= 0; i--)
        {
            transform.position = path[i].position;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FindPath(Vector3Int destination)
    {
        int arraySize = gridHeight * gridWidth;

        // Declare the variables
        NativeHashMap<Vector3Int, Node> _gridNodes = new NativeHashMap<Vector3Int, Node>(arraySize, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _openList = new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _closedList = new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeArray<Vector3Int> _neighbourOffsets = new NativeArray<Vector3Int>(8, Allocator.TempJob);
        NativeArray<Node> _backtrackedPath = new NativeArray<Node>(arraySize / 2, Allocator.TempJob);
        NativeArray<int> _backtrackedPathLength = new NativeArray<int>(1, Allocator.TempJob);

        // Add all nodes in the generated grid to the NativeHashMap (Replace this with cody's grid system)
        foreach (var item in grid)
        {
            _gridNodes.Add(item.position, item);
        }

        // Create a new instance of the AStar job and assign its variables
        AStar aStar = new AStar
        {
            gridNodes = _gridNodes,
            openList = _openList,
            closedList = _closedList,
            neighbourOffsets = _neighbourOffsets,
            startNode = grid[(int)transform.position.x, (int)transform.position.y, (int)transform.position.z],
            endNode = grid[destination.x, destination.y, destination.z],
            backtrackedPath = _backtrackedPath,
            backTrackedPathLength = _backtrackedPathLength
        };

        // Schedule the job and complete it
        JobHandle handle = aStar.Schedule();
        handle.Complete();

        // retrieve the backtracked path from the job
        for (int i = 0; i < _backtrackedPathLength[0]; i++)
        {
            backtrackedPath.Add(_backtrackedPath[i]);
        }

        // retrieve closed and openlist data for debugging
        foreach (var item in _closedList)
        {
            closedList.Add(item.Value);
        }

        foreach (var item in _openList)
        {
            openList.Add(item.Value);
        }

        // Dispose all the NativeContainers to avoid memory leaks
        _gridNodes.Dispose();
        _openList.Dispose();
        _closedList.Dispose();
        _neighbourOffsets.Dispose();
        _backtrackedPath.Dispose();
        _backtrackedPathLength.Dispose();
    }

    /// <summary>
    /// Create a grid of nodes
    /// </summary>
    void CreateGrid()
    {
        grid = new Node[gridWidth, gridHeight, 1];

        var untraversable = _grid.UnTraversable();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    traversable = untraversable[x, y]
                };
                grid[x, y, 0] = node;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!displayGizmos) return;

        Gizmos.DrawWireCube(new Vector3(gridWidth / 2, gridHeight / 2, 0), new Vector3(gridWidth, gridHeight));

        Gizmos.color = Color.cyan;
        foreach (var item in backtrackedPath)
        {
            Gizmos.DrawCube(item.position, Vector3.one);
        }

        foreach (var item in openList)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(item.position, Vector3.one);
        }

        foreach (var item in closedList)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(item.position, Vector3.one);
        }
    }
}
