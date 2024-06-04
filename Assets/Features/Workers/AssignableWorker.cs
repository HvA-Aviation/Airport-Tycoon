using System;
using System.Collections;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Features.Workers
{
    public class AssignableWorker : Worker
    {
        [SerializeField] protected NPCController _npcController;
        [SerializeField] protected Grid _grid;
        [SerializeField] protected float _workLoadSpeed;
        protected Vector3Int _assignment;
        protected Vector3Int _targetPosition;
        
        protected Vector3Int GetRotationAddition(Vector3Int target)
        {
            int rotation = _grid.GetRotation(target);
            Vector3Int rotationVector = Vector3Int.down;

            switch (rotation)
            {
                case 0:
                    rotationVector = Vector3Int.up;
                    break;
                case 1:
                    rotationVector = Vector3Int.right;
                    break;
                case 2:
                    rotationVector = Vector3Int.down;
                    break;
                case 3:
                    rotationVector = Vector3Int.left;
                    break;
            }

            return new Vector3Int(target.x, target.y, 0) + rotationVector;
        }

        public void Station(Action onDone)
        {
            StartCoroutine(WorkOn(10f, onDone));
        }
        
        protected IEnumerator WorkOn(float time, Action onDone)
        {
            int times = 0;
            while (times < 10)
            {
                if (GameManager.Instance.QueueManager.HasQueuers(_assignment))
                {
                    if (GameManager.Instance.QueueManager.WorkOnQueue(_assignment, _workLoadSpeed))
                    {
                        GameManager.Instance.QueueManager.RemoveFromQueue(_assignment);
                        times++;
                    }
                }

                yield return null;
            }

            onDone?.Invoke();
        }

        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            _assignment = target;
            _targetPosition = GetRotationAddition(target);
            
            _npcController.SetTarget(
                _targetPosition,
                () => CheckTaskExists(target, onDone),
                onReachedPosition);
        }

        /// <summary>
        /// Checks if task is still needed
        /// </summary>
        protected bool CheckTaskExists(Vector3Int target, Action onDone)
        {
            if (_grid.Get(target) == -1)
            {
                onDone.Invoke();
                return false;
            }

            return true;
        }
    }
}