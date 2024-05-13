using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using System.Collections;
using System.Linq;

public class NPCController : MonoBehaviour
{
    [Header("Dependecies")]
    [SerializeField] private Grid _grid;
    [SerializeField] private bool _drawGizmos;

    [Header("Movement variables")]
    [SerializeField] float _moveSpeed = 1f;

    Node[,,] _nodeGrid;
    List<Node> _backtrackedPath = new List<Node>();
    [SerializeField] Vector3Int _endNode;
    int _gridWidth;
    int _gridHeight;
    [SerializeField] bool _pathCompleted = true;
    [SerializeField] Node[] open;
    [SerializeField] Node[] closed;

    public bool PathCompleted => _pathCompleted;

    private void Start()
    {
        _gridWidth = _grid.GridSize.x;
        _gridHeight = _grid.GridSize.y;
    }

    public Vector3Int GetGridPosition()
    {
        var pos = transform.position;

        //clamp to grid positions
        var clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
            RoundToMultiple(pos.y, _grid.CellSize));

        return new Vector3Int((int)clampedValue.x, (int)clampedValue.y, 0);
    }

    void Update()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //clamp to grid positions
        var clampedValue = new Vector2(RoundToMultiple(pos.x, _grid.CellSize),
            RoundToMultiple(pos.y, _grid.CellSize));

        if (Input.GetKeyDown(KeyCode.P) & _pathCompleted)
        {
            CreateGrid();
            StopAllCoroutines();

            _backtrackedPath.Clear();

            _endNode = new Vector3Int((int)clampedValue.x, (int)clampedValue.y, 0);

            FindPath(_endNode);
            StartCoroutine(MoveToTarget(_backtrackedPath));
        }
    }

    public void SetTarget(Vector3Int position)
    {
        //clamp to grid positions
        var clampedValue = new Vector2(RoundToMultiple(position.x, _grid.CellSize),
            RoundToMultiple(position.y, _grid.CellSize));

        CreateGrid();
        StopAllCoroutines();

        _backtrackedPath.Clear();

        _endNode = new Vector3Int((int)clampedValue.x, (int)clampedValue.y, 0);

        FindPath(_endNode);
        StartCoroutine(MoveToTarget(_backtrackedPath));
    }

    /// <summary>
    /// Move the NPC to the target (This is just for visualisation purposes, needs to be replanced with actual movement system)
    /// </summary>
    private IEnumerator MoveToTarget(List<Node> path)
    {
        _pathCompleted = false;
        for (int i = path.Count - 1; i >= 0; i--)
        {
            while (Vector3.Distance(path[i].position, transform.position) > 0.1f)
            {
                Vector3 direction = path[i].position - transform.position;
                transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.position = path[i].position;
        }
        _pathCompleted = true;
    }

    /// <summary>
    /// Find the path from current position to the end node
    /// </summary>
    void FindPath(Vector3Int destination)
    {
        int arraySize = _gridHeight * _gridWidth;

        // Declare the variables
        NativeHashMap<Vector3Int, Node> _gridNodes = new NativeHashMap<Vector3Int, Node>(arraySize, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _openList =
            new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _closedList =
            new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeArray<Vector3Int> _neighbourOffsets = new NativeArray<Vector3Int>(8, Allocator.TempJob);
        NativeArray<Node> _backtrackedPath = new NativeArray<Node>(arraySize, Allocator.TempJob);
        NativeArray<int> _backtrackedPathLength = new NativeArray<int>(1, Allocator.TempJob);

        // Add all nodes in the generated grid to the NativeHashMap (Replace this with cody's grid system)
        foreach (var item in _nodeGrid)
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
            startNode = _nodeGrid[(int)transform.position.x, (int)transform.position.y, (int)transform.position.z],
            endNode = _nodeGrid[destination.x, destination.y, destination.z],
            backtrackedPath = _backtrackedPath,
            backTrackedPathLength = _backtrackedPathLength
        };

        // Schedule the job and complete it
        JobHandle handle = aStar.Schedule();
        handle.Complete();

        // retrieve the backtracked path from the job
        for (int i = 0; i < _backtrackedPathLength[0]; i++)
        {
            this._backtrackedPath.Add(_backtrackedPath[i]);
        }

        open = _openList.GetValueArray(Allocator.Temp).ToArray();
        closed = _closedList.GetValueArray(Allocator.Temp).ToArray();

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
        _nodeGrid = new Node[_gridWidth, _gridHeight, 1];

        var untraversable = _grid.UnTraversable();
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    traversable = !untraversable[x, y]
                };
                _nodeGrid[x, y, 0] = node;
            }
        }
    }

    public float RoundToMultiple(float value, float roundTo)
    {
        return Mathf.RoundToInt(value / roundTo) * roundTo;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_drawGizmos) return;

        foreach (var item in open)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(item.position, Vector3.one);
        }

        foreach (var item in closed)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(item.position, Vector3.one);
        }

        foreach (var item in _backtrackedPath)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(item.position, Vector3.one);
        }
    }
}
