using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Collections;

public struct AStar : IJob
{
    public NativeHashMap<Vector3Int, Node> gridNodes;

    public NativeHashMap<Vector3Int, Node> openList;
    public NativeHashMap<Vector3Int, Node> closedList;

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
            currentNode = LowestFCostInList(openList);
            closedList.TryAdd(currentNode.position, currentNode);

            if (currentNode.position == endNode.position)
                break;

            for (int i = 0; i < neighbourOffsets.Length; i++)
            {
                if (closedList.ContainsKey(currentNode.position + neighbourOffsets[i]) ||
                    !gridNodes.ContainsKey(currentNode.position)) continue;

                Node neighbour = new Node
                {
                    position = currentNode.position + neighbourOffsets[i],
                    parent = currentNode.position,
                    gCost = currentNode.gCost + i < 4 ? 10 : 14,
                    hCost = CalculateHCost(currentNode.position + neighbourOffsets[i], endNode.position)
                };
                neighbour.CalculateFCost();

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
    public HashSet<Node> closedList = new HashSet<Node>();
    public HashSet<Node> openList = new HashSet<Node>();
    public List<Node> calculatedPath = new List<Node>();
    public Node[,,] Grid;

    [Header("Dependecies")]
    [SerializeField] Tilemap _collisionMap;

    [Header("Start | End")]
    public Vector3Int startNode;
    public Vector3Int endNode;
    public Node currentNode;

    [Header("Grid settings")]
    public int gridWidth;
    public int gridHeight;

    Vector3Int[] _neighbours = new Vector3Int[8]{
        Vector3Int.up,
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.up + Vector3Int.right,
        Vector3Int.up + Vector3Int.left,
        Vector3Int.down + Vector3Int.right,
        Vector3Int.down + Vector3Int.left,
    };

    // Start is called before the first frame update
    void Start()
    {
        GetCollisionData();
        CreateGrid();
        //AStar(Grid[startNode.x, startNode.y, startNode.z], Grid[endNode.x, endNode.y, endNode.z]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            openList.Clear();
            closedList.Clear();
            calculatedPath.Clear();
            AStar(Grid[startNode.x, startNode.y, startNode.z], Grid[endNode.x, endNode.y, endNode.z]);
        }
    }

    /// <summary>
    /// Create a grid of nodes
    /// </summary>
    void CreateGrid()
    {
        Grid = new Node[gridWidth, gridHeight, 1];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Node node = new Node()
                {
                    position = new Vector3Int(x, y, 0),
                    traversable = untraversableTiles.Contains(new Vector3Int(x, y, 0))
                };
                Grid[x, y, 0] = node;
            }
        }

        Grid[startNode.x, startNode.y, startNode.z].isStartNode = true;
        Grid[endNode.x, endNode.y, endNode.z].isEndNode = true;
    }

    /// <summary>
    /// A* algorithm to decide the best path from the start node to the end node
    /// </summary>
    void AStar(Node startNode, Node endNode)
    {
        closedList.Add(startNode);
        openList = GetNeighbours(startNode);

        while (openList.Count > 0)
        {
            currentNode = LowestFCostInList(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.isEndNode)
            {
                BacktrackPath();
                break;
            }

            HashSet<Node> neighbours = GetNeighbours(currentNode);

            foreach (Node item in neighbours)
            {
                openList.Add(item);
            }
        }
    }

    /// <summary>
    /// Get the lowest f cost in a given list of nodes
    /// </summary>
    Node LowestFCostInList(HashSet<Node> nodeList)
    {
        float lowestFcost = float.MaxValue;
        Node currentNode = new Node();
        foreach (var item in nodeList)
        {
            if (item.fCost < lowestFcost)
            {
                lowestFcost = item.fCost;
                currentNode = item;
            }
        }
        return currentNode;
    }

    /// <summary>
    /// Get neighbours of tile
    /// </summary>
    HashSet<Node> GetNeighbours(Node currentNode)
    {
        HashSet<Node> neighbours = new HashSet<Node>();
        for (int i = 0; i < _neighbours.Length; i++)
        {
            Vector3Int _neighbourPos = currentNode.position + _neighbours[i];

            // check if within bounds of the grid
            if (_neighbourPos.x > gridWidth - 1 || _neighbourPos.x < 0 || _neighbourPos.y > gridHeight - 1 || _neighbourPos.y < 0)
            {
                print("we continue the loop");
                continue;
            }

            Node _neighbour = Grid[_neighbourPos.x, _neighbourPos.y, _neighbourPos.z];

            // check if the neighbour does not exist in our open/closed/untraversable lists
            if (openList.Contains(_neighbour)) continue;
            if (closedList.Contains(_neighbour)) continue;
            if (untraversableTiles.Contains(_neighbour.position)) continue;

            // set the neighbours variables
            _neighbour.position = _neighbourPos;
            _neighbour.parent = currentNode.position;
            _neighbour.hCost = CalculateHCost(_neighbourPos, endNode);
            _neighbour.gCost = i < 4 ? currentNode.gCost + 10 : currentNode.gCost + 14;
            _neighbour.CalculateFCost();
            neighbours.Add(_neighbour);
        }
        return neighbours;
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
    /// Backtrack the path from the last node in the closed list which should be the end node
    /// </summary>
    void BacktrackPath()
    {
        Node currentNode = Grid[endNode.x, endNode.y, endNode.z];
        while (currentNode.position != Grid[startNode.x, startNode.y, startNode.z].position)
        {
            calculatedPath.Add(currentNode);
            currentNode = Grid[currentNode.parent.x, currentNode.parent.y, currentNode.parent.z];
        }
    }

    /// <summary>
    /// Retrieve the Collision data from a given grid
    /// </summary>
    void GetCollisionData()
    {
        for (int x = -_collisionMap.size.x; x < _collisionMap.size.x; x++)
        {
            for (int y = -_collisionMap.size.y; y < _collisionMap.size.y; y++)
            {
                Vector3Int _pos = new Vector3Int(x, y, 0);
                if (_collisionMap.GetTile(_pos))
                {
                    untraversableTiles.Add(_pos);
                }
            }
        }
    }
}
