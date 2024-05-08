using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Collections;
using System.Linq;

public struct AStar : IJob
{
    public NativeHashMap<Vector3Int, Node> gridNodes;

    public NativeHashMap<Vector3Int, Node> openList;
    public NativeHashMap<Vector3Int, Node> closedList;
    public NativeArray<Node> backtrackedPath;

    public NativeArray<Vector3Int> neighbourOffsets;

    public Node startNode;
    public Node endNode;

    public void Execute()
    {
        Node currentNode = startNode;
        currentNode.hCost = CalculateHCost(currentNode.position, endNode.position);
        currentNode.CalculateFCost();
        openList.Add(currentNode.position, currentNode);

        neighbourOffsets[0] = Vector3Int.up;
        neighbourOffsets[1] = Vector3Int.left;
        neighbourOffsets[2] = Vector3Int.right;
        neighbourOffsets[3] = Vector3Int.down;
        neighbourOffsets[4] = Vector3Int.up + Vector3Int.right;
        neighbourOffsets[5] = Vector3Int.up + Vector3Int.left;
        neighbourOffsets[6] = Vector3Int.down + Vector3Int.right;
        neighbourOffsets[7] = Vector3Int.down + Vector3Int.left;

        while (openList.Count() != 0)
        {
            // Assign current node to lowest F cost in open list
            currentNode = LowestFCostInList(openList);
            closedList.TryAdd(currentNode.position, currentNode);

            // Exit out of loop if we reached the end node
            if (currentNode.position == endNode.position)
            {
                BacktrackPath();
                break;
            }

            // Check all neighbours of the current node
            for (int i = 0; i < neighbourOffsets.Length; i++)
            {
                if (closedList.ContainsKey(currentNode.position + neighbourOffsets[i]) ||
                    !gridNodes.ContainsKey(currentNode.position)) continue;

                Node neighbour = new Node
                {
                    position = currentNode.position + neighbourOffsets[i],
                    parent = currentNode.position,
                    gCost = currentNode.gCost + i < 4 ? 10 : 14,
                    hCost = CalculateHCost(currentNode.position + neighbourOffsets[i], endNode.position),
                    traversable = gridNodes[currentNode.position + neighbourOffsets[i]].traversable
                };
                neighbour.CalculateFCost();

                if (neighbour.traversable) continue;

                if (openList.ContainsKey(neighbour.position) && neighbour.gCost < openList[neighbour.position].gCost)
                {
                    openList[neighbour.position] = neighbour;
                }
                else if (!openList.ContainsKey(neighbour.position))
                {
                    openList.TryAdd(neighbour.position, neighbour);
                }
            }
            openList.Remove(currentNode.position);
        }
    }

    /// <summary>
    /// Backtrack the path from endnode to startnode
    /// </summary>
    void BacktrackPath()
    {
        if (closedList.ContainsKey(endNode.position))
        {
            Node _currentNode = closedList[endNode.position];
            backtrackedPath[0] = _currentNode;
            int index = 0;
            while (true)
            {
                _currentNode = closedList[_currentNode.parent];
                backtrackedPath[index++] = _currentNode;
                if (_currentNode.position == startNode.position) break;
            }
        }
    }

    /// <summary>
    /// Calculate the H cost for a node from point to endnode
    /// </summary>
    float CalculateHCost(Vector3Int currentNodePosition, Vector3Int endNodePosition)
    {
        float xCost = Mathf.Abs(endNodePosition.x - currentNodePosition.x);
        float yCost = Mathf.Abs(endNodePosition.y - currentNodePosition.y);

        if (xCost > yCost)
            return 14 * yCost + 10 * (xCost - yCost);
        return 14 * xCost + 10 * (yCost - xCost);
    }

    /// <summary>
    /// Get the lowest f cost in a given list of nodes
    /// </summary>
    Node LowestFCostInList(NativeHashMap<Vector3Int, Node> nodeList)
    {
        float lowestFcost = float.MaxValue;
        Node currentNode = new Node();
        foreach (var item in nodeList)
        {
            if (item.Value.position == Vector3Int.zero) break;
            if (item.Value.fCost < lowestFcost)
            {
                lowestFcost = item.Value.fCost;
                currentNode = item.Value;
            }
        }
        return currentNode;
    }
}

public class Pathfinding : MonoBehaviour
{
    [Header("Lists")]
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

    /// <summary>
    /// Declare the variables and send them to the AStar job
    /// </summary>
    void FindPath()
    {
        int arraySize = gridHeight * gridWidth;

        // Declare the variables
        NativeHashMap<Vector3Int, Node> _gridNodes = new NativeHashMap<Vector3Int, Node>(arraySize, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _openList = new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeHashMap<Vector3Int, Node> _closedList = new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
        NativeArray<Vector3Int> _neighbourOffsets = new NativeArray<Vector3Int>(8, Allocator.TempJob);
        NativeArray<Node> _backtrackedPath = new NativeArray<Node>(200, Allocator.TempJob);

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
            startNode = grid[startNode.x, startNode.y, startNode.z],
            endNode = grid[endNode.x, endNode.y, endNode.z],
            backtrackedPath = _backtrackedPath
        };

        // Schedule the job and complete it
        JobHandle handle = aStar.Schedule();
        handle.Complete();

        // retrieve the backtracked path from the job
        backtrackedPath = _backtrackedPath.ToList();

        // Dispose all the NativeContainers to avoid memory leaks
        _gridNodes.Dispose();
        _openList.Dispose();
        _closedList.Dispose();
        _neighbourOffsets.Dispose();
        _backtrackedPath.Dispose();
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
        Gizmos.DrawWireCube(new Vector3(gridWidth / 2, gridHeight / 2, 0), new Vector3(gridWidth, gridHeight));

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
