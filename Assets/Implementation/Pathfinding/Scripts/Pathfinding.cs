using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Implementation.Pathfinding.Scripts
{
    [BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public struct AStar : IJob
    {
        public NativeHashMap<Vector3Int, Node> gridNodes;

        public Heap openListHeap;
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
            openListHeap.Add(_currentNode);

            neighbourOffsets[0] = Vector3Int.up;
            neighbourOffsets[1] = Vector3Int.left;
            neighbourOffsets[2] = Vector3Int.right;
            neighbourOffsets[3] = Vector3Int.down;
            neighbourOffsets[4] = Vector3Int.up + Vector3Int.right;
            neighbourOffsets[5] = Vector3Int.up + Vector3Int.left;
            neighbourOffsets[6] = Vector3Int.down + Vector3Int.right;
            neighbourOffsets[7] = Vector3Int.down + Vector3Int.left;

            while (openListHeap.items.Length != 0)
            {
                // Assign current node to lowest F cost in open list
                _currentNode = openListHeap.Pop();
                closedList.TryAdd(_currentNode.position, _currentNode);

                // Check all neighbours of the current node
                for (int i = 0; i < neighbourOffsets.Length; i++)
                {
                    if (!gridNodes.ContainsKey(_currentNode.position + neighbourOffsets[i])) continue;
                    if (closedList.ContainsKey(_currentNode.position + neighbourOffsets[i])) continue;

                    Node neighbour = new Node
                    {
                        position = _currentNode.position + neighbourOffsets[i],
                        parent = _currentNode.position,
                        gCost = _currentNode.gCost + (i < 4 ? 10 : 14),
                        hCost = CalculateHCost(_currentNode.position + neighbourOffsets[i], endNode.position),
                        untraversable = gridNodes[_currentNode.position + neighbourOffsets[i]].untraversable
                    };

                    if (neighbour.untraversable) continue;

                    if (!openListHeap.Contains(neighbour.position))
                    {
                        openListHeap.Add(neighbour);
                    }

                    if (openListHeap.GetAtPositionIndex(neighbour.position).gCost > neighbour.gCost)
                    {
                        // new heap function to replace a current node in the tree because we've found a better path to it
                        openListHeap.ReplaceNode(neighbour.position, neighbour);
                    }
                }

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
            return Vector3Int.Distance(currentNodePosition, endNodePosition);
        }
    }
}
