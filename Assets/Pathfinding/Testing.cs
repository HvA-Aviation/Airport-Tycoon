using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Testing : MonoBehaviour
{
    public List<Vector3Int> untraversableTiles = new List<Vector3Int>();
    public Node[,,] grid;
    public List<Node> backtrackedPath = new List<Node>();

    [Header("Dependecies")]
    [SerializeField] Tilemap collisionMap;

    [Header("Start | End")]
    public Vector3Int startNode;
    public Vector3Int endNode;

    [Header("Grid settings")]
    public int gridWidth;
    public int gridHeight;

    private void Start()
    {
        CreateGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            backtrackedPath.Clear();
            FindPath();
        }
    }

    void FindPath()
    {
        NativeHashMap<Vector3Int, Node> _gridNodes = new NativeHashMap<Vector3Int, Node>(gridHeight * gridWidth, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _openList = new NativeHashMap<Vector3Int, Node>(gridHeight * gridWidth * 8, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _closedList = new NativeHashMap<Vector3Int, Node>(gridHeight * gridWidth * 8, Allocator.TempJob);
        NativeArray<Vector3Int> _neighbourOffsets = new NativeArray<Vector3Int>(8, Allocator.TempJob);

        foreach (var item in grid)
        {
            _gridNodes.Add(item.position, item);
        }

        AStar aStar = new AStar
        {
            gridNodes = _gridNodes,
            openList = _openList,
            closedList = _closedList,
            neighbourOffsets = _neighbourOffsets,
            startNode = grid[startNode.x, startNode.y, startNode.z],
            endNode = grid[endNode.x, endNode.y, endNode.z]
        };

        JobHandle handle = aStar.Schedule();
        handle.Complete();

        foreach (var item in _closedList)
        {
            if (item.Value.position == endNode)
            {
                print($"position: {item.Value.position} || parent: {item.Value.parent}");
                Node currentNode = item.Value;
                backtrackedPath.Add(currentNode);

                while (true)
                {
                    currentNode = _closedList[currentNode.parent];
                    backtrackedPath.Add(currentNode);
                    if (currentNode.position == startNode) break;
                }

            }
        }

        _gridNodes.Dispose();
        _openList.Dispose();
        _closedList.Dispose();
        _neighbourOffsets.Dispose();
    }

    /// <summary>
    /// Create a grid of nodes
    /// </summary>
    void CreateGrid()
    {
        grid = new Node[gridWidth, gridHeight, 1];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int _pos = new Vector3Int(x, y, 0);

                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    traversable = collisionMap.GetTile(_pos)
                };
                grid[x, y, 0] = node;
            }
        }

        grid[startNode.x, startNode.y, startNode.z].isStartNode = true;
        grid[endNode.x, endNode.y, endNode.z].isEndNode = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(startNode, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(endNode, Vector3.one);

        Gizmos.color = Color.cyan;
        foreach (var item in backtrackedPath)
        {
            Gizmos.DrawCube(item.position, Vector3.one);
        }
    }
}
