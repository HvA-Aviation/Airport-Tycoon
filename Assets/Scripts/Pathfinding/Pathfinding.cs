using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;

public struct AStar : IJob
{
    public NativeHashMap<Vector3Int, Node> gridNodes;

    public NativeHashMap<Vector3Int, Node> openList;
    public NativeHashMap<Vector3Int, Node> closedList;
    public NativeArray<Node> backtrackedPath;
    public NativeArray<int> backTrackedPathLength;

    public NativeArray<Vector3Int> neighbourOffsets;

    public Node startNode;
    public Node endNode;

    public void Execute()
    {
        Node currentNode = startNode;
        currentNode.hCost = CalculateHCost(currentNode.position, endNode.position);
        currentNode.CalculateFCost();
        openList.Add(currentNode.position, currentNode);
        int openListCount = 1;

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
            currentNode = LowestFCostInList(openList, openListCount);
            closedList.TryAdd(currentNode.position, currentNode);

            // Check all neighbours of the current node
            for (int i = 0; i < neighbourOffsets.Length; i++)
            {
                if (closedList.ContainsKey(currentNode.position + neighbourOffsets[i]) ||
                    !gridNodes.ContainsKey(currentNode.position + neighbourOffsets[i])) continue;

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
                    openListCount++;
                }
            }
            openList.Remove(currentNode.position);
            openListCount--;

            // Exit out of loop if we reached the end node
            if (currentNode.position == endNode.position)
            {
                BacktrackPath();
                break;
            }
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
            int index = 1;
            while (true)
            {
                _currentNode = closedList[_currentNode.parent];
                backtrackedPath[index] = _currentNode;
                index++;
                if (_currentNode.position == startNode.position) break;
            }
            backTrackedPathLength[0] = index;
        }
    }

    /// <summary>
    /// Calculate the H cost for a node from point to endnode
    /// </summary>
    float CalculateHCost(Vector3Int currentNodePosition, Vector3Int endNodePosition)
    {
        float xCost = Mathf.Abs(endNodePosition.x - currentNodePosition.x);
        float yCost = Mathf.Abs(endNodePosition.y - currentNodePosition.y);

        return xCost * xCost + yCost * yCost;
    }

    /// <summary>
    /// Get the lowest f cost in a given list of nodes
    /// </summary>
    Node LowestFCostInList(NativeHashMap<Vector3Int, Node> nodeList, int nodeListCount)
    {
        int count = 0;
        float lowestFcost = float.MaxValue;
        Node currentNode = new Node();

        foreach (var item in nodeList)
        {
            if (item.Value.fCost < lowestFcost)
            {
                lowestFcost = item.Value.fCost;
                currentNode = item.Value;
            }
            if (count > nodeListCount) break;
            count++;
        }
        return currentNode;
    }
}
