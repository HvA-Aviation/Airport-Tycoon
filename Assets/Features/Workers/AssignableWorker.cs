using System;
using System.Collections;
using Features.Managers;
using Implementation.Pathfinding.Scripts;
using Implementation.TaskSystem;
using UnityEngine;
using Grid = Features.Building.Scripts.Grid.Grid;

namespace Features.Workers
{
    public abstract class AssignableWorker : Worker
    {
        [SerializeField] protected NPCController _npcController;
        [SerializeField] protected float _workLoadSpeed;
        [SerializeField] protected int _assignmentsShift;
        protected Vector3Int _assignment;
        protected Vector3Int _targetPosition;
        protected TaskCommand<AssignableWorker> _task;

        
        protected virtual void Start()
        {
            // Register it to the task system by setting it available.
            TaskManager().SetAvailable(this);
        }
        
        /// <summary>
        /// Gets the direction of the utility so the worker will stand behind it
        /// </summary>
        /// <param name="target">Position of the utility</param>
        /// <returns>The position behind the utility</returns>
        protected Vector3Int GetRotationAddition(Vector3Int target)
        {
            int rotation = GameManager.Instance.GridManager.Grid.GetRotation(target);
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

        public abstract TaskSystem<AssignableWorker> TaskManager();

        public void SetTask(TaskCommand<AssignableWorker> taskCommand)
        {
            _task = taskCommand;
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
            float workload = GameManager.Instance.GridManager.Grid.GetUtilityWorkLoad(_assignment);

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

            _task = null;
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
            float workload = GameManager.Instance.GridManager.Grid.GetUtilityWorkLoad(target);
            if (workload == 0)
            {
                _task = null;
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
            if (GameManager.Instance.GridManager.Grid.Get(target) == -1)
            {
                _task = null;
                onDone.Invoke();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stop all current routines and add the current task back to the task list
        /// </summary>
        public void Fire()
        {
            StopAllCoroutines();
            _npcController.StopAllCoroutines();
            if (_task != null)
                TaskManager().AddTask(_task);
        }
    }
}