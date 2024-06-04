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
        [SerializeField] protected int _assignmentsShift;
        protected Vector3Int _assignment;
        protected Vector3Int _targetPosition;

        /// <summary>
        /// Gets the direction of the utility so the worker will stand behind it
        /// </summary>
        /// <param name="target">Position of the utility</param>
        /// <returns>The position behind the utility</returns>
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

        /// <summary>
        /// Starts a routine so the worker will work on the utility
        /// </summary>
        /// <param name="onDone">Sets the worker back in the task queue</param>
        public void Station(Action onDone)
        {
            StartCoroutine(WorkOn(onDone));
        }

        /// <summary>
        /// Works on the queue till the assigment shift has been met
        /// </summary>
        /// <param name="onDone">Sets the worker back in the task queue</param>
        protected IEnumerator WorkOn(Action onDone)
        {
            float workload = _grid.GetUtilityWorkLoad(_assignment);

            int times = 0;
            while (times < _assignmentsShift)
            {
                if (!CheckTaskExists(_assignment, onDone))
                    yield break;

                if (GameManager.Instance.QueueManager.HasQueuers(_assignment))
                {
                    if (GameManager.Instance.QueueManager.WorkOnQueue(_assignment, _workLoadSpeed, workload))
                    {
                        GameManager.Instance.QueueManager.RemoveFromQueue(_assignment);
                        times++;
                    }
                }

                yield return null;
            }

            onDone?.Invoke();
        }

        /// <summary>
        /// Move to grid location
        /// </summary>
        /// <param name="target">Target location</param>
        /// <param name="onReachedPosition">What to do on the target location</param>
        /// <param name="onDone">What to do when all is done</param>
        public void MoveTo(Vector3Int target, Action onReachedPosition, Action onDone)
        {
            //Checks if utility still exitst
            float workload = _grid.GetUtilityWorkLoad(target);
            if (workload == 0)
            {
                onDone.Invoke();
                return;
            }
            
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