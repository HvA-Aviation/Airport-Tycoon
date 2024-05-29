using System;
using System.Collections;
using System.Collections.Generic;
using Features.Managers;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Implementation.Pathfinding.Scripts
{
    public class NPCController : MonoBehaviour
    {
        [Header("Dependecies")]
        [SerializeField] private Grid _grid;

        [Header("Movement variables")]
        [SerializeField] private float _moveSpeed = 1f;

        private List<Node> _backtrackedPath = new List<Node>();
        private Vector3Int _endNode;
        private Node[] open, closed;

        /// <summary>
        /// Use this function to set the target of an NPC
        /// </summary>
        /// <param name="position">destination of path</param>
        /// <param name="checkIfTaskIsStillNeeded">Callback to check if task is still needed</param>
        /// <param name="onDestinationReached">Callback to handle what to do when the destination has been reached</param>
        public void SetTarget(Vector3Int position, Action checkIfTaskIsStillNeeded, Action onDestinationReached)
        {
            StopAllCoroutines();

            _backtrackedPath.Clear();

            _endNode = new Vector3Int(position.x, position.y, 0);

            FindPath(_endNode, GameManager.Instance.GridManager.NodeGrid);
            StartCoroutine(MoveToTarget(_backtrackedPath, checkIfTaskIsStillNeeded, onDestinationReached));
        }

        /// <summary>
        /// Move the NPC to the target
        /// </summary>
        private IEnumerator MoveToTarget(List<Node> path, Action checkIfTaskIsStillNeeded, Action onDestinationReached)
        {
            // if there is no path break out of this coroutine
            if (path.Count <= 0) yield break;

            for (int i = path.Count - 1; i >= 0; i--)
            {
                if (path[i].position == Vector3Int.zero) continue;
                while (Vector3.Distance(path[i].position, transform.position) > 0.1f)
                {
                    Vector3 direction = path[i].position - transform.position;
                    transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                transform.position = path[i].position;
                checkIfTaskIsStillNeeded.Invoke();
            }
            onDestinationReached.Invoke();
        }

        /// <summary>
        /// Find the path from current position to the end node
        /// </summary>
        void FindPath(Vector3Int destination, NativeHashMap<Vector3Int, Node> nodeGrid)
        {
            int arraySize = _grid.GridSize.x * _grid.GridSize.y;

            // Declare the variables
            Heap _openListHeap = new Heap(arraySize / 2);
            NativeHashMap<Vector3Int, Node> _closedList = new NativeHashMap<Vector3Int, Node>(arraySize / 2, Allocator.TempJob);
            NativeArray<Vector3Int> _neighbourOffsets = new NativeArray<Vector3Int>(8, Allocator.TempJob);
            NativeArray<Node> _backtrackedPath = new NativeArray<Node>(arraySize, Allocator.TempJob);
            NativeArray<int> _backtrackedPathLength = new NativeArray<int>(1, Allocator.TempJob);

            // Get the start and end nodes from the node grid
            nodeGrid.TryGetValue(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), out Node _startNode);
            nodeGrid.TryGetValue(destination, out Node _endNode);

            // Create a new instance of the AStar job and assign its variables
            AStar aStar = new AStar
            {
                gridNodes = nodeGrid,
                closedList = _closedList,
                openListHeap = _openListHeap,
                neighbourOffsets = _neighbourOffsets,
                startNode = _startNode,
                endNode = _endNode,
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

            open = _openListHeap.items.ToArray();
            closed = _closedList.GetValueArray(Allocator.Temp).ToArray();

            // Dispose all the NativeContainers to avoid memory leaks
            _openListHeap.DisposeOfLists();
            _closedList.Dispose();
            _neighbourOffsets.Dispose();
            _backtrackedPath.Dispose();
            _backtrackedPathLength.Dispose();
        }

        public float RoundToMultiple(float value, float roundTo)
        {
            return Mathf.RoundToInt(value / roundTo) * roundTo;
        }

        private void OnDrawGizmosSelected()
        {
            if (open == null || closed == null || _backtrackedPath == null) return;

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
}
