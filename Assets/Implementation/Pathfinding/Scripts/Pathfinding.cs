using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Implementation.Pathfinding.Scripts
{
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
            Node _currentNode = startNode;
            _currentNode.hCost = CalculateHCost(_currentNode.position, endNode.position);
            _currentNode.CalculateFCost();
            openList.Add(_currentNode.position, _currentNode);

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
                _currentNode = LowestFCostInList(openList);
                closedList.TryAdd(_currentNode.position, _currentNode);

                // Check all neighbours of the current node
                for (int i = 0; i < neighbourOffsets.Length; i++)
                {
                    if (closedList.ContainsKey(_currentNode.position + neighbourOffsets[i]) ||
                        !gridNodes.ContainsKey(_currentNode.position + neighbourOffsets[i])) continue;

                    Node neighbour = new Node
                    {
                        position = _currentNode.position + neighbourOffsets[i],
                        parent = _currentNode.position,
                        gCost = _currentNode.gCost + (i < 4 ? 10 : 14),
                        hCost = CalculateHCost(_currentNode.position + neighbourOffsets[i], endNode.position),
                        traversable = gridNodes[_currentNode.position + neighbourOffsets[i]].traversable
                    };
                    neighbour.CalculateFCost();

                    if (!neighbour.traversable) continue;

                    if (openList.ContainsKey(neighbour.position) && neighbour.gCost < openList[neighbour.position].gCost)
                    {
                        openList[neighbour.position] = neighbour;
                    }
                    else if (!openList.ContainsKey(neighbour.position))
                    {
                        openList.TryAdd(neighbour.position, neighbour);
                    }
                }
                openList.Remove(_currentNode.position);

                // Exit out of loop if we reached the end node
                if (_currentNode.position == endNode.position)
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
                    if (index >= backtrackedPath.Length) break;
                    closedList.TryGetValue(_currentNode.parent, out _currentNode);
                    backtrackedPath[index++] = _currentNode;
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
}
